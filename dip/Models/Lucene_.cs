using dip.Models.Domain;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Ru;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using PagedList;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;



namespace dip.Models
{

    /// <summary>
    /// класс для полнотекстового поиска lucene
    /// </summary>
    public class Lucene_
    {

        /// <summary>
        /// метод для построение индекса
        /// </summary>
        static public void BuildIndex()
        {
            var feList = new List<FEText>();
            using (var db = new ApplicationDbContext())
                feList = db.FEText.ToList();
            using (var directory = GetDirectory())
            using (var analyzer = GetAnalyzer())
            using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                writer.DeleteAll();

                foreach (var i in feList)
                {
                    Lucene_.BuildIndexSolo(writer, i);
                }
            }
        }


        /// <summary>
        /// метод для обновления записи фэ
        /// </summary>
        /// <param name="idfe"></param>
        /// <param name="obj"></param>
        static public void UpdateDocument(string idfe, FEText obj)
        {
            using (var directory = GetDirectory())
            using (var analyzer = GetAnalyzer())
            using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                obj.ChangeForMap();
                writer.UpdateDocument(new Term("IDFE", idfe), MapProduct(obj), analyzer);
            }
        }


        /// <summary>
        /// метод для построения индекса одной записи фэ
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj"></param>
        static public void BuildIndexSolo(IndexWriter writer, FEText obj)
        {
            obj.ChangeForMap();
            var document = MapProduct(obj);
            writer.AddDocument(document);
        }


        /// <summary>
        /// метод для построения индекса одной записи фэ
        /// </summary>
        /// <param name="a"></param>
        static public void BuildIndexSolo(FEText a)
        {
            using (var directory = GetDirectory())
            using (var analyzer = GetAnalyzer())
            using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                Lucene_.BuildIndexSolo(writer, a);
            }
        }



        /// <summary>
        /// метод для строки(убрать стопслова и привести к нормальной форме)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ChangeForMap(string s)
        {
            string pattern = @"\s+а\s+|\s+без\s+|\s+более\s+|\s+бы\s+|\s+был\s+|" +
                @"\s+была\s+|\s+были\s+|\s+было\s+|\s+быть\s+|\s+в\s+|\s+вам\s+|\s+вас\s+|" +
                @"\s+весь\s+|\s+во\s+|\s+вот\s+|\s+все\s+|\s+всего\s+|\s+всех\s+|\s+вы\s+|" +
                @"\s+где\s+|\s+да\s+|\s+даже\s+|\s+для\s+|\s+до\s+|\s+его\s+|\s+ее\s+|\s+ей\s+|" +
                @"\s+ею\s+|\s+если\s+|\s+есть\s+|\s+еще\s+|\s+же\s+|\s+за\s+|\s+здесь\s+|\s+и\s+|" +
                @"\s+из\s+|\s+или\s+|\s+им\s+|\s+их\s+|\s+к\s+|\s+как\s+|\s+ко\s+|\s+когда\s+|" +
                @"\s+кто\s+|\s+ли\s+|\s+либо\s+|\s+мне\s+|\s+может\s+|\s+мы\s+|\s+на\s+|\s+надо\s+|" +
@"\s+наш\s+|\s+не\s+|\s+него\s+|\s+нее\s+|\s+нет\s+|\s+ни\s+|\s+них\s+|\s+но\s+|" +
@"\s+ну\s+|\s+о\s+|\s+об\s+|\s+однако\s+|\s+он\s+|\s+она\s+|\s+они\s+|\s+оно\s+|" +
@"\s+от\s+|\s+очень\s+|\s+по\s+|\s+под\s+|\s+при\s+|\s+с\s+|\s+со\s+|\s+так\s+|" +
@"\s+также\s+|\s+такой\s+|\s+там\s+|\s+те\s+|\s+тем\s+|\s+то\s+|\s+того\s+|" +
@"\s+тоже\s+|\s+той\s+|\s+только\s+|\s+том\s+|\s+ты\s+|\s+у\s+|\s+уже\s+|\s+хотя\s+|" +
@"\s+чего\s+|\s+чей\s+|\s+чем\s+|\s+что\s+|\s+чтобы\s+|\s+чье\s+|\s+чья\s+|\s+эта\s+|" +
@"\s+эти\s+|\s+это\s+|\s+я\s+|\s+этом\s+|\s+этого\s+";
            //pattern += @"|~|`|!|@|#|\$|%|\^|&|\*|\(|\)|_|\+|=|-|<|>|\?|,|\.|\/|\\|\{|\}|\[|\]|\|";
            s = Lucene_.DeletePunctuations(s);
            RegexOptions options = RegexOptions.IgnoreCase;
            string target = " ";
            Regex regex = new Regex(pattern, options);
            string result = regex.Replace(s, target);
            pattern = @"\s+";
            regex = new Regex(pattern);
            result = regex.Replace(result, target);
            var st = new RussianLightStemmer();
            string res = "";
            foreach (var i in result.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var num = st.Stem(i.ToArray(), i.Length);
                res += i.Substring(0, num) + " ";
            }
            res.Trim();
            return res;
        }


        /// <summary>
        /// Метод для удаления пунктуации
        /// </summary>
        /// <param name="s">строка в которой необходимо удаление</param>
        /// <returns></returns>
        public static string DeletePunctuations(string s)
        {
            string pattern = @";|:|~|`|!|@|#|\$|%|\^|&|\*|\(|\)|_|\+|=|-|<|>|\?|,|\.|\/|\\|\{|\}|\[|\]|\|";
            RegexOptions options = RegexOptions.IgnoreCase;
            string target = " ";
            Regex regex = new Regex(pattern, options);
            string res = regex.Replace(s, target);
            regex = new Regex(@"\s+", options);
            res = regex.Replace(res, target);
            return res;
        }

        /// <summary>
        /// Метод для удаления букв английского алфавита
        /// </summary>
        /// <param name="s">строка в которой необходимо удаление</param>
        /// <returns></returns>
        public static string DeleteEngl(string s)
        {
            string pattern = @"[A-Za-zÀ-ÿ]+";
            RegexOptions options = RegexOptions.IgnoreCase;
            string target = " ";
            Regex regex = new Regex(pattern, options);
            string res = regex.Replace(s, target);
            regex = new Regex(@"\s+", options);
            res = regex.Replace(res, target);
            return res;


        }


        /// <summary>
        /// метод для получения Document
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static Document MapProduct(FEText obj)
        {
            return obj.GetDocumentForLucene();
        }


        /// <summary>
        /// метод для получения Directory для lucene
        /// </summary>
        /// <returns></returns>
        static Lucene.Net.Store.Directory GetDirectory()
        {
            return new SimpleFSDirectory(new DirectoryInfo(HostingEnvironment.MapPath($"~/lucene")));
        }

        /// <summary>
        /// метод для получения анализатора
        /// </summary>
        /// <returns></returns>
        static Analyzer GetAnalyzer()
        {
            return new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        }



        /// <summary>
        /// метод для получения Query
        /// </summary>
        /// <param name="keyword">строка с словами для поиска</param>
        /// <returns></returns>
        static Query GetQuery(string keyword)
        {
            using (var analyzer = GetAnalyzer())
            {
                var query = new BooleanQuery();
                var keywords = keyword.Trim().Split(' ');
                foreach (var i in FEText.GetPropTextSearch())
                {
                    var phraseQuery = new PhraseQuery();
                    foreach (var i2 in keywords)
                    {
                        phraseQuery.Add(new Term(i, i2));
                    }
                    query.Add(phraseQuery, Occur.SHOULD);
                    var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, i, analyzer);
                    var keywordsQuery = parser.Parse(keyword);
                    query.Add(keywordsQuery, Occur.SHOULD);
                }


                return query; // +Name:ibanez -Brand:Fender Name:"electric guitar"    
            }
        }


        /// <summary>
        /// метод для получения объекта сортировки(Sort)
        /// </summary>
        /// <returns></returns>
        static Sort GetSort()
        {
            var fields = new[] { SortField.FIELD_SCORE };
            return new Sort(fields);
        }




        //TODO если в MapProduct будет нормализация то доставать только id и по ним искать
        /// <summary>
        /// метод для поиска lucene
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="limit"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        static public List<int> Search(string keywords, int limit, out int count)
        {
            using (var directory = GetDirectory())
            using (var searcher = new IndexSearcher(directory))
            {
                var query = GetQuery(keywords);
                var sort = GetSort();
                var docs = searcher.Search(query, null, limit, sort);
                count = docs.TotalHits;
                var products = new List<int>();
                foreach (var scoreDoc in docs.ScoreDocs)
                {
                    var doc = searcher.Doc(scoreDoc.Doc);
                    products.Add(int.Parse(doc.Get("IDFE")));
                }
                return products;
            }
        }
    }
}