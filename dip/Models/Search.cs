using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
    public class Search
    {

        public static List<int> GetListPhys(string type, string str, HttpContextBase HttpContext, int lastId = 0, int lucCount = 1)
        {
            //var res = new int[0];
            var res = new List<int>();
            //type = "fullTextSearch";
            var user = ApplicationUser.GetUser(ApplicationUser.GetUserId());
            switch (type)
            {
                case "lucene":
                    //var list_id=FEText.GetByText(str);
                    int count;
                    str = Lucene_.ChangeForMap(str);
                    //TODO убрать знаки препинания, стопслова
                    int limit = 0;
                    using (ApplicationDbContext db = new ApplicationDbContext())
                        limit = db.FEText.Count();
                    res = Lucene_.Search(str, limit, out count);//.Skip(Constants.CountForLoad * (lucCount - 1)).ToList();//.ToArray()
                   
                    res=user.CheckAccessPhys(res, HttpContext).Skip(Constants.CountForLoad * (lucCount - 1)).ToList();

                    //res = Lucene_.Search(str, 100, out count);
                    //res= res.Skip(Constants.CountForLoad * (lucCount - 1)).ToList();//.ToArray()

                    break;
                case "fullTextSearchF"://freetexttable

                    using (var db = new ApplicationDbContext())
                    {
                        //TODO вынести в функцию sql server и юзать уже из linq
                        //IEnumerable<int> resenum;//= res.AsEnumerable() ;
                         //res = db.Database.SqlQuery<int>($@"select IDFE from freetexttable(dbo.FeTexts,*,'{str
                         //   }')as t join dbo.FeTexts as y on t.[KEY] = y.IDFE order by RANK desc;")
                         //   .SkipWhile(x1 => lastId>0?( x1 != lastId) :false).Skip( lastId > 0 ?1:0 ).Take(Constants.CountForLoad).ToList();//.ToArray()


                        res = user.CheckAccessPhys(db.Database.SqlQuery<int>($@"select IDFE from freetexttable(dbo.FeTexts,*,'{str
                            }')as t join dbo.FeTexts as y on t.[KEY] = y.IDFE order by RANK desc;").ToList(), HttpContext).
                            SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();

                        //if (lastId > 0)
                        //{
                        //    resenum = resenum.SkipWhile(x1 => x1 > lastId);
                        //}
                        //res = resenum.Take(Constants.CountForLoad).ToList();



                    }

                    break;
                case "fullTextSearchCf"://формы слова

                    using (var db = new ApplicationDbContext())
                    {
                        //TODO вынести в функцию sql server и юзать уже из linq
                        //str = Lucene_.ChangeForMap(str);
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
                        //res = db.Database.SqlQuery<int>(strquery).
                        //    SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
                        //.SkipWhile(x1 => x1 > lastId).Take(Constants.CountForLoad).ToList();//.ToArray()

                        res = user.CheckAccessPhys(db.Database.SqlQuery<int>(strquery).ToList(), HttpContext).
                               SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();


                    }


                    break;
                case "fullTextSearchCl"://вхождения

                    using (var db = new ApplicationDbContext())
                    {
                        //TODO вынести в функцию sql server и юзать уже из linq
                        str = Lucene_.ChangeForMap(str);
                        string strquery = "select IDFE from CONTAINSTABLE(dbo.FeTexts,*,'ISABOUT (";
                        foreach (var i in str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            int index = i.IndexOf('#');
                            string newstr = i;
                            if (index >= 0)
                            {
                                strquery += $"\"{newstr}*\" WEIGHT(1),";
                            }
                            else
                                strquery += $"\"{newstr}*\" WEIGHT(0.3),";

                        }
                        strquery = strquery.Substring(0, strquery.Length - 1);
                        strquery += ")')as t join dbo.FeTexts as y on t.[KEY] = y.IDFE order by RANK desc;";
                        //res = db.Database.SqlQuery<int>(strquery)
                        //     .SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
                        //.SkipWhile(x1 => x1 > lastId).Take(Constants.CountForLoad).ToList();//.ToArray()

                        res = user.CheckAccessPhys(db.Database.SqlQuery<int>(strquery).ToList(), HttpContext).
                                   SkipWhile(x1 => lastId > 0 ? (x1 != lastId) : false).Skip(lastId > 0 ? 1 : 0).Take(Constants.CountForLoad).ToList();
                    }


                    break;
                   
            }
            return res;
        }


        /// <summary>
        /// вернуть только наиболее значимые слова
        /// </summary>
        /// <param name="logParamId"></param>
        /// <returns></returns>
        public static string StringSemanticParse(int logParamId)
        {
            var res = "";
            var q = $@"SELECT TOP(10) KEYP_TBL.keyphrase  
FROM SEMANTICKEYPHRASETABLE  
    (  
    LogParams,  
    Param,  
    {logParamId}  
    ) AS KEYP_TBL  
ORDER BY KEYP_TBL.score DESC;";
            var ldr=DataBase.DataBase.ExecuteQuery(q,null, "keyphrase");
            foreach (var i in ldr)
                res += i["keyphrase"].ToString()+" ";

            return res.Trim();
        }


        //public static List<int> GetListSimilarPhys(int id,int count=5)
        //{
        //    List<int> res = new List<int>();



        //    return res;
        //}

        }
}