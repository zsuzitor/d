/*файл класса для осуществления поисковых запросов
Авдосев Станислав Алексеевич (zsuzitor) © 2019
E-mail: avdosevstas@mail.ru*/

using dip.Models.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace dip.Models
{

    /// <summary>
    /// класс для полнотекстового поиска
    /// </summary>
    public class Search
    {
        /// <summary>
        /// метод для полнотекстового поиска
        /// </summary>
        /// <param name="type">тип поиска</param>
        /// <param name="str"> строка для поиска</param>
        /// <param name="HttpContext"></param>
        /// <param name="lastId">последний загруженный id</param>
        /// <param name="lucCount">количество загруженных записей для lucene</param>
        /// <returns>null если нужно попробовать позже</returns>
        public static List<int> GetListPhys(string type, string str, HttpContextBase HttpContext, int lastId = 0, int lucCount = 1)
        {
            var res = new List<int>();
            var user = ApplicationUser.GetUser(ApplicationUser.GetUserId());

            str = Lucene_.DeleteEngl(str);

            switch (type)
            {
                case "lucene":
                    res = luceneSearch(user, str, HttpContext, lucCount);

                    break;
                case "fullTextSearchF"://freetexttable
                    res = fullTextSearchF(user, str, HttpContext, lastId);

                    break;
                case "fullTextSearchCf"://формы слова
                    res = fullTextSearchCf(user, str, HttpContext, lastId);

                    break;
                case "fullTextSearchCl"://вхождения
                    res = fullTextSearchCl(user, str, HttpContext, lastId);
                    break;

                case "fullTextSearchNear"://слова ищутся рядом
                    res = fullTextSearchNear(user, str, HttpContext, lastId);
                    break;


                case "fullTextSearchSemantic"://семантический поиск
                    res = fullTextSearchSemantic(user, str, HttpContext, lastId);
                    break;

                case "NGrammSearch"://n граммы
                    res = NGrammSearch(user, str, HttpContext, lastId);
                    break;

                case "fullTextSearchMainSemanticWord":
                    res = fullTextSearchMainSemanticWord(user, str, HttpContext, lastId);
                    break;
            }
            return res;
        }




        /// <summary>
        /// метод для поиска lucene
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="str">строка для поиска</param>
        /// <param name="HttpContext"></param>
        /// <param name="lucCount">количество загрузок</param>
        /// <returns></returns>
        public static List<int> luceneSearch(ApplicationUser user, string str, HttpContextBase HttpContext, int lucCount = 1)
        {
            List<int> res = new List<int>();
            int count;
            str = Lucene_.ChangeForMap(str);
            //TODO убрать знаки препинания, стопслова
            int limit = 0;
            using (ApplicationDbContext db = new ApplicationDbContext())
                limit = db.FEText.Count();
            res = Lucene_.Search(str, limit, out count);
            res = user.CheckAccessPhys(res, HttpContext).Skip(Constants.CountForLoad * (lucCount - 1)).Take(Constants.CountForLoad).ToList();
            return res;
        }


        /// <summary>
        /// метод для поиска
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="str">строка для поиска</param>
        /// <param name="HttpContext"></param>
        /// <param name="lastId">id последнейзагруженной записи</param>
        /// <returns></returns>
        public static List<int> fullTextSearchF(ApplicationUser user, string str, HttpContextBase HttpContext, int lastId = 0)
        {
            List<int> res = new List<int>();


            using (var db = new ApplicationDbContext())
                res = user.CheckAccessPhys(db.Database.SqlQuery<int>($@"select IDFE from freetexttable(dbo.FeTexts,*,'{str
                    }')as t join dbo.FeTexts as y on t.[KEY] = y.IDFE order by RANK desc;").ToList(), HttpContext).
                    SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
            return res;
        }

        /// <summary>
        /// метод для поиска
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="str">строка для поиска</param>
        /// <param name="HttpContext"></param>
        /// <param name="lastId">id последнейзагруженной записи</param>
        /// <returns></returns>
        public static List<int> fullTextSearchCf(ApplicationUser user, string str, HttpContextBase HttpContext, int lastId = 0)
        {
            List<int> res = new List<int>();

            string strquery = "select IDFE from CONTAINSTABLE(dbo.FeTexts,*,'ISABOUT (";
            foreach (var i in str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int index = i.IndexOf('#');
                string newstr = i;
                if (index >= 0)
                {
                    newstr = i.Substring(++index);
                    strquery += $"FORMSOF(INFLECTIONAL,\"{newstr}\") WEIGHT(1),";
                }
                else
                    strquery += $"FORMSOF(INFLECTIONAL,\"{newstr}\") WEIGHT(0.3),";
            }
            strquery = strquery.Substring(0, strquery.Length - 1);
            strquery += ")')as t join dbo.FeTexts as y on t.[KEY] = y.IDFE order by RANK desc;";

            if (strquery.Length > 4000)
                return res;


            using (var db = new ApplicationDbContext())
                res = user.CheckAccessPhys(db.Database.SqlQuery<int>(strquery).ToList(), HttpContext).
                       SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
            return res;
        }


        /// <summary>
        /// метод для поиска
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="str">строка для поиска</param>
        /// <param name="HttpContext"></param>
        /// <param name="lastId">id последнейзагруженной записи</param>
        /// <returns></returns>
        public static List<int> fullTextSearchCl(ApplicationUser user, string str, HttpContextBase HttpContext, int lastId = 0)
        {
            List<int> res = new List<int>();
            str = Lucene_.ChangeForMap(str);
            if (str.Length > 2000)
                str = str.Take(2000).ToString();
            string strquery = "select IDFE from CONTAINSTABLE(dbo.FeTexts,*,'ISABOUT (";
            foreach (var i in str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int index = i.IndexOf('#');
                string newstr = i;
                if (index >= 0)
                    strquery += $"\"{newstr}*\" WEIGHT(1),";
                else
                    strquery += $"\"{newstr}*\" WEIGHT(0.3),";
            }
            strquery = strquery.Substring(0, strquery.Length - 1);
            strquery += ")')as t join dbo.FeTexts as y on t.[KEY] = y.IDFE order by RANK desc;";

            if (strquery.Length > 4000)
                return res;


            using (var db = new ApplicationDbContext())
                res = user.CheckAccessPhys(db.Database.SqlQuery<int>(strquery).ToList(), HttpContext).
                           SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
            return res;
        }


        /// <summary>
        /// метод для поиска
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="str">строка для поиска</param>
        /// <param name="HttpContext"></param>
        /// <param name="lastId">id последнейзагруженной записи</param>
        /// <returns></returns>
        public static List<int> fullTextSearchNear(ApplicationUser user, string str, HttpContextBase HttpContext, int lastId = 0)
        {
            List<int> res = new List<int>();
            str = Lucene_.ChangeForMap(str);
            var massWords = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (massWords.Length < 2)
                return res;
            if (massWords.Length > 60)
                massWords = massWords.Take(60).ToArray();
            string strquery = "select [KEY] from CONTAINSTABLE(dbo.FeTexts,*,'NEAR(";
            for (int i = 0; i < massWords.Length; ++i)
            {
                strquery += "\"" + massWords[i] + "*\"";
                if (i < massWords.Length - 1)
                    strquery += ",";
            }
            strquery += ")')order by RANK desc;";


            using (var db = new ApplicationDbContext())
                res = user.CheckAccessPhys(db.Database.SqlQuery<int>(strquery).ToList(), HttpContext).
                           SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
            return res;
        }

        /// <summary>
        ///  метод для поиска
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="str">строка для поиска</param>
        /// <param name="HttpContext"></param>
        /// <param name="lastId">id последнейзагруженной записи</param>
        /// <returns></returns>
        public static List<int> fullTextSearchSemantic(ApplicationUser user, string str, HttpContextBase HttpContext, int lastId = 0)
        {
            List<int> res = new List<int>();
            IList<string> roles = HttpContext.GetOwinContext()
                                         .GetUserManager<ApplicationUserManager>()?.GetRoles(user.Id);
            if (!roles.Contains(RolesProject.admin.ToString()))
                return res;
            FEText fesemantic = null;
            using (var db = new ApplicationDbContext())
            {
                fesemantic = db.FEText.FirstOrDefault(x1 => x1.IDFE == Constants.FEIDFORSEMANTICSEARCH);
                if (fesemantic == null)
                    return null;
                if (fesemantic.Text != Constants.FeSemanticNullText)
                    return null;
                fesemantic.Text = str;
                db.SaveChanges();
                try
                {
                    var strquery = $@"
                    select   txt.IDFE
                    from
                           semanticsimilaritytable (FETexts, *, {Constants.FEIDFORSEMANTICSEARCH}  ) as data
                                inner join FETexts as
                                txt on data.matched_document_key = txt.IDFE
                    order by data.score desc";

                    Thread.Sleep(1000);

                    //ждем обновления записи
                    for (int i = 0; i < 5; ++i)
                    {
                        var tmp = db.FEText.FirstOrDefault(x1 => x1.IDFE == Constants.FEIDFORSEMANTICSEARCH);
                        if (tmp.Text != Constants.FeSemanticNullText)
                            break;
                        else
                            Thread.Sleep(500);
                    }
                    //ждем обновления индекса
                    res = db.Database.SqlQuery<int>(strquery).ToList();
                    for (int i = 0; i < 5 && res.Count == 0; ++i)
                    {
                        Thread.Sleep(500);
                        res = db.Database.SqlQuery<int>(strquery).ToList();
                    }
                    if (res.Count > 0)
                    {
                        res = user.CheckAccessPhys(res, HttpContext).ToList().
                              SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
                    }
                    fesemantic.Text = Constants.FeSemanticNullText;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    fesemantic.Text = Constants.FeSemanticNullText;
                    db.SaveChanges();
                }
            }
            return res;
        }

        /// <summary>
        /// метод для поиска
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="str">строка для поиска</param>
        /// <param name="HttpContext"></param>
        /// <param name="lastId">id последнейзагруженной записи</param>
        /// <returns></returns>
        public static List<int> NGrammSearch(ApplicationUser user, string str, HttpContextBase HttpContext, int lastId = 0)
        {
            List<int> res = new List<int>();
            using (var db = new ApplicationDbContext())
            {
                Dictionary<int, string> dictGrammReg = new Dictionary<int, string>();
                str = Lucene_.ChangeForMap(str);
                var massWords = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (massWords.Length > 20)
                    massWords = massWords.Take(20).ToArray();

                for (int i = 1; i <= massWords.Length; ++i)//количество
                {
                    string gr = "(";
                    for (int i3 = 0; i3 + i <= massWords.Length; ++i3)//позиция с которой начинаем
                    {
                        gr += "(";
                        for (int i2 = i3; i2 < i3 + i; ++i2)//сама выборка
                        {
                            gr += massWords[i2] + ".{0,5}";
                        }
                        gr += ")|";
                    }
                    gr = gr.Substring(0, gr.Length - 1);
                    gr += ")";
                    dictGrammReg[i] = gr;
                }
                dictGrammReg[1] = "газ.{0,5}";
                Dictionary<int, int> dictWeight = new Dictionary<int, int>();

                int feCount = db.FEText.Count();
                int countOneLoad = 1000;
                for (int i2 = 0; i2 <= feCount / countOneLoad; ++i2)
                {
                    var listfe = db.FEText.OrderBy(x1 => x1.IDFE).Skip(i2 * countOneLoad).Take(countOneLoad).Select(x1 => new { id = x1.IDFE, text = x1.Text }).ToList();
                    foreach (var i in listfe)
                    {
                        for (int numgr = 1; numgr < dictGrammReg.Count + 1; ++numgr)
                        {
                            Regex regex = new Regex(dictGrammReg[numgr]);
                            var resreg = regex.Matches(i.text);
                            if (!dictWeight.ContainsKey(i.id))
                                dictWeight[i.id] = resreg.Count * numgr;
                            else
                                dictWeight[i.id] += resreg.Count * numgr;
                        }
                    }
                }
                //var TODO = dictWeight.Where(x1 => x1.Value > 0).OrderByDescending(x1 => x1.Value).Select(x1 => x1.Key).ToList();
                res = user.CheckAccessPhys(dictWeight.Where(x1 => x1.Value > 0).OrderByDescending(x1 => x1.Value).Select(x1 => x1.Key).ToList(), HttpContext).
                    SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
            }
            return res;
        }


        /// <summary>
        /// метод для поиска
        /// </summary>
        /// <param name="user">пользователь</param>
        /// <param name="str">строка для поиска</param>
        /// <param name="HttpContext"></param>
        /// <param name="lastId">id последнейзагруженной записи</param>
        /// <returns></returns>
        public static List<int> fullTextSearchMainSemanticWord(ApplicationUser user, string str, HttpContextBase HttpContext, int lastId = 0)
        {
            List<int> res = new List<int>();
            str = Lucene_.ChangeForMap(str);
            var massWords = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<int, string> dict = new Dictionary<int, string>();
            string strquery = @"SELECT  document_key,keyphrase
FROM SEMANTICKEYPHRASETABLE
    (
    FETexts,
    Text
    ) AS KEYP_TBL  ";

            var ldr = DataBase.DataBase.ExecuteQuery(strquery, null, "document_key", "keyphrase");
            foreach (var i in ldr)
            {
                int feid = int.Parse(i["document_key"].ToString());
                string keyp = i["keyphrase"].ToString();
                if (dict.ContainsKey(feid))
                    dict[feid] += " " + keyp;
                else
                    dict[feid] = keyp;
            }
            string regStr = "";
            foreach (var i in massWords)
            {
                regStr += "(" + i + ")|";
            }
            regStr = regStr.Substring(0, regStr.Length - 1);
            Regex regex = new Regex(regStr);
            Dictionary<int, int> dictSearch = new Dictionary<int, int>();
            foreach (var i in dict)
            {
                var coll = regex.Matches(i.Value);
                dictSearch[i.Key] = coll.Count;
            }
            //var TODO = dictSearch.Where(x1 => x1.Value > 0).OrderBy(x1 => x1.Value).Select(x1 => x1.Key).ToList();
            res = user.CheckAccessPhys(dictSearch.Where(x1 => x1.Value > 0).OrderBy(x1 => x1.Value).Select(x1 => x1.Key).ToList(), HttpContext).
                       SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
            return res;
        }
    }
}