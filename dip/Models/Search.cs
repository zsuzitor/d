using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dip.Models
{
    public class Search
    {

        public static List<int> GetList(string type, string str, int lastId = 0, int lucCount = 1)
        {
            //var res = new int[0];
            var res = new List<int>();
            //type = "fullTextSearch";
            switch (type)
            {
                case "lucene":
                    //var list_id=FEText.GetByText(str);
                    int count;
                    str = Lucene_.ChangeForMap(str);
                    //TODO убрать знаки препинания, стопслова
                    res = Lucene_.Search(str, Constants.CountForLoad * lucCount, out count).Skip(Constants.CountForLoad * (lucCount - 1)).ToList();//.ToArray()
                    break;
                case "fullTextSearchF"://freetexttable

                    using (var db = new ApplicationDbContext())
                    {
                        //TODO вынести в функцию sql server и юзать уже из linq
                        res = db.Database.SqlQuery<int>($@"select IDFE from freetexttable(dbo.FeTexts,*,'{str
                            }')as t join dbo.FeTexts as y on t.[KEY] = y.IDFE order by RANK desc;").Where(x1 => x1 > lastId).Take(Constants.CountForLoad).ToList();//.ToArray()
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
                        res = db.Database.SqlQuery<int>(strquery).Where(x1 => x1 > lastId).Take(Constants.CountForLoad).ToList();//.ToArray()
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
                        res = db.Database.SqlQuery<int>(strquery).Where(x1 => x1 > lastId).Take(Constants.CountForLoad).ToList();//.ToArray()
                    }


                    break;
                   
            }
            return res;
        }

    }
}