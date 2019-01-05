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


//http://www.waveaccess.ru/blog/2014/september/02/%D0%BF%D0%BE%D0%BB%D0%BD%D0%BE%D1%82%D0%B5%D0%BA%D1%81%D1%82%D0%BE%D0%B2%D1%8B%D0%B9-%D0%BF%D0%BE%D0%B8%D1%81%D0%BA-%D1%81-%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5%D0%BC-apache-lucene.aspx


//ru analizer//tok//norm  https://github.com/apache/lucenenet/tree/master/src/Lucene.Net.Analysis.Common/Analysis/Ru
//тоже самое но с ридером https://lucenenet.apache.org/docs/3.0.3/d1/d8f/_russian_analyzer_8cs_source.html

// https://github.com/apache/lucenenet

namespace dip.Models
{



    //static string[] Search(string searchTerm)
    //{
    //    Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Snowball.SnowballAnalyzer("English");
    //    //Lucene.Net.Analysis.Analyzer analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer();
    //    Lucene.Net.QueryParsers.QueryParser parser = new Lucene.Net.QueryParsers.QueryParser(Lucene.Net.Util.Version.LUCENE_29, "text", analyzer);
    //    Lucene.Net.Search.Query query = parser.Parse(searchTerm);

    //    Lucene.Net.Search.Searcher searcher = new Lucene.Net.Search.IndexSearcher(Lucene.Net.Store.FSDirectory.Open(new DirectoryInfo("./index/")), true);
    //    var topDocs = searcher.Search(query, null, 10);

    //    List<string> results = new List<string>();

    //    foreach (var scoreDoc in topDocs.scoreDocs)
    //    {
    //        results.Add(searcher.Doc(scoreDoc.doc).Get("raw"));
    //    }

    //    return results.ToArray();
    //}








    public class Lucene_
    {


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


        static public void BuildIndexSolo(IndexWriter writer, FEText obj)
        {

            obj.ChangeForMap();
            var document = MapProduct(obj);
            writer.AddDocument(document);
        }


        static public void BuildIndexSolo(FEText a)
        {

            using (var directory = GetDirectory())
            using (var analyzer = GetAnalyzer())
            using (var writer = new IndexWriter(directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {

                Lucene_.BuildIndexSolo(writer, a);

            }
        }




        public static  string ChangeForMap(string s)
        {
            // string s = "Мама  мыла  раму. ";
            //string pattern = @"\s+";
            string pattern = "";

            pattern = @"\s+а\s+|\s+без\s+|\s+более\s+|\s+бы\s+|\s+был\s+|" +
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
            pattern += @"|~|`|!|@|#|\$|%|\^|&|\*|\(|\)|_|\+|=|-|<|>|\?|,|\.|\/|\\|\{|\}|\[|\]|\|";

            RegexOptions options = RegexOptions.IgnoreCase;
            //pattern = "";

            // pattern += @"|\W";

            string target = " ";
            Regex regex = new Regex(pattern, options);
            string result = regex.Replace(s, target);
            pattern = @"\s+";
            regex = new Regex(pattern);
            result = regex.Replace(result, target);


            var st = new RussianLightStemmer();
            //foreach (var i in obj.Text.Split(' '))
            //{
            //    Lucene_.ChangeForMap(i);
            //    //var num = st.Stem(i.ToArray(), i.Length);
            //    // var word = i.Substring(0, num);
            //}
            string res = "";
            foreach (var i in result.Split(new char[] { ' ' },StringSplitOptions.RemoveEmptyEntries))
            {


                var num = st.Stem(i.ToArray(), i.Length);
                res += i.Substring(0, num)+" ";
                //res += i+" ";
            }
            res.Trim();


            return res;





            //result = regex.Replace(result, target);
            //result = regex.Replace(result, target);



            //String[] RUSSIAN_STOP_WORDS = {
            //                                                                   "а", "без", "более", "бы", "был", "была", "были",
            //                                                                   "было", "быть", "в",
            //                                                                   "вам", "вас", "весь", "во", "вот", "все", "всего",
            //                                                                   "всех", "вы", "где",
            //                                                                   "да", "даже", "для", "до", "его", "ее", "ей", "ею",
            //                                                                   "если", "есть",
            //                                                                   "еще", "же", "за", "здесь", "и", "из", "или", "им",
            //                                                                   "их", "к", "как",
            //                                                                   "ко", "когда", "кто", "ли", "либо", "мне", "может",
            //                                                                   "мы", "на", "надо",
            //                                                                   "наш", "не", "него", "нее", "нет", "ни", "них", "но",
            //                                                                   "ну", "о", "об",
            //                                                                   "однако", "он", "она", "они", "оно", "от", "очень",
            //                                                                   "по", "под", "при",
            //                                                                   "с", "со", "так", "также", "такой", "там", "те", "тем"
            //                                                                   , "то", "того",
            //                                                                   "тоже", "той", "только", "том", "ты", "у", "уже",
            //                                                                   "хотя", "чего", "чей",
            //                                                                   "чем", "что", "чтобы", "чье", "чья", "эта", "эти",
            //                                                                   "это", "я"
            //                                                               };
            //string hh = "";
            //foreach (var i in RUSSIAN_STOP_WORDS)
            //    hh += @"\s+" + i + @"\s+|";

            //string h1 = hh;


            //h1 += "|.|";
            //h1 += @"|\W";
            //\s    -- пробельный
            // \d   - цифра
            // \W   -  любой не буква\цифра\_
            //return "";






        }





        //TODO мб нормализация
        static Document MapProduct(FEText obj)
        {
            var document = new Document();
            document.Add(new NumericField("IDFE", Field.Store.YES, true).SetIntValue(obj.IDFE));
            document.Add(new Field("Name", obj.Name, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("Text", obj.Text, Field.Store.YES, Field.Index.ANALYZED));

            document.Add(new Field("TextInp", obj.TextInp, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("TextOut", obj.TextOut, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("TextObj", obj.TextObj, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("TextApp", obj.TextApp, Field.Store.YES, Field.Index.ANALYZED));
            document.Add(new Field("TextLit", obj.TextLit, Field.Store.YES, Field.Index.ANALYZED));
            //document.Add(new Field("Brand", obj.Brand, Field.Store.YES, Field.Index.NOT_ANALYZED));
            return document;
        }


        static Lucene.Net.Store.Directory GetDirectory()
        {
            //
            return new SimpleFSDirectory(new DirectoryInfo(HostingEnvironment.MapPath($"~/lucene")));
            //return new SimpleFSDirectory(new DirectoryInfo(@"E:\csharp\dip1\dip\dip\lucene"));//(@"~/lucene"));
        }


        static Analyzer GetAnalyzer()
        {
            
            return new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        }




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
                //var phraseQuery = new PhraseQuery();
                //phraseQuery.Add(new Term("Name", "electric"));
                //phraseQuery.Add(new Term("Name", "guitar"));
                //query.Add(phraseQuery, Occur.SHOULD);



                //var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Name", analyzer);
                //var keywordsQuery = parser.Parse(keyword);
                //query.Add(keywordsQuery, Occur.SHOULD);

                //parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Text", analyzer);
                //keywordsQuery = parser.Parse(keyword);
                //query.Add(keywordsQuery, Occur.SHOULD);

                //parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextInp", analyzer);
                //keywordsQuery = parser.Parse(keyword);
                //query.Add(keywordsQuery, Occur.SHOULD);

                //parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextOut", analyzer);
                //keywordsQuery = parser.Parse(keyword);
                //query.Add(keywordsQuery, Occur.SHOULD);

                //parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextObj", analyzer);
                //keywordsQuery = parser.Parse(keyword);
                //query.Add(keywordsQuery, Occur.SHOULD);

                //parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextApp", analyzer);
                //keywordsQuery = parser.Parse(keyword);
                //query.Add(keywordsQuery, Occur.SHOULD);

                //parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextLit", analyzer);
                //keywordsQuery = parser.Parse(keyword);
                //query.Add(keywordsQuery, Occur.SHOULD);





                //Term - 1парам - поле в котором ищем
                // var termQuery = new TermQuery(new Term("Brand", "Fender"));
                //var phraseQuery = new PhraseQuery();
                //phraseQuery.Add(new Term("Name", "electric"));
                //phraseQuery.Add(new Term("Name", "guitar"));
                //query.Add(keywordsQuery, Occur.MUST);//обязательно
                //query.Add(termQuery, Occur.MUST_NOT);//исключаем
                //query.Add(phraseQuery, Occur.SHOULD);//не обязательно, но влияет на релевантность
                return query; // +Name:ibanez -Brand:Fender Name:"electric guitar"    
            }
        }

        static Sort GetSort()
        {
            var fields = new[] {  SortField.FIELD_SCORE };//new SortField("Brand", SortField.STRING),
            return new Sort(fields); // sort by brand, then by score 
        }


        //#steamm

        //public static string Steam()
        //{
        //    var s = new PorterStemmer();



        //}






//        public static String removeStopWordsAndGetNorm(String text, String[] stopWords, Normalizer normalizer) 
//        {
//    //        TokenStream tokenStream = new ClassicTokenizer(Version.LUCENE_44, new StringReader(text));
//    //        tokenStream = new StopFilter(Version.LUCENE_44, tokenStream, StopFilter.makeStopSet(Version.LUCENE_44, stopWords, true));
//    //        tokenStream = new LowerCaseFilter(Version.LUCENE_44, tokenStream);
//    //        tokenStream = new StandardFilter(Version.LUCENE_44, tokenStream);
//    //        tokenStream.reset();
//    //        String result = "";
//    //        while (tokenStream.incrementToken())
//    //        {
//    //            CharTermAttribute token = tokenStream.getAttribute(CharTermAttribute.class); 
//    // try 
//    // { 
//    //  //normalizer.getNormalForm(...) - stemmer or lemmatizer 
//    //  result += normalizer.getNormalForm(token.toString()) + " "; 
//    // } 
//    // catch(Exception e) 
//    // { 
//    //  //if something went wrong 
//    // } 
//    //} 
//}









        //static Filter GetFilter()
        //{
        //    return NumericRangeFilter.NewIntRange("Id", 2, 5, true, false); // [2; 5) range 
        //} 




        //public static void GoSearch(string str)
        //{
        //    BuildIndex();
        //    int count;
        //    var products = Search(str, 100, out count);
        //    foreach (var product in products)
        //        Console.WriteLine("ID={0}; Name={1}; Brand={2}; Color={3}", product.Id, product.Name, product.Brand, product.Color);
        //    Console.WriteLine("Total: {0}", count);
        //    Console.ReadKey();
        //}


        //TODO если в MapProduct будет нормализация то доставать только id и по ним искать
        static public List<int> Search(string keywords, int limit, out int count)//Dictionary<int,double>
        {
            using (var directory = GetDirectory())
            using (var searcher = new IndexSearcher(directory))
            {
                var query = GetQuery(keywords);
                var sort = GetSort();
                //var filter = GetFilter();
                var docs = searcher.Search(query, null, limit, sort);
                count = docs.TotalHits;
                var products = new List<int>();//new Dictionary<int, double>();
                foreach (var scoreDoc in docs.ScoreDocs)
                {
                  // var g= scoreDoc.Score;
                    var doc = searcher.Doc(scoreDoc.Doc);
                    
                    //var product = new FEText { IDFE = int.Parse(doc.Get("IDFE")),
                    //    Name = doc.Get("Name"),
                    //    Text = doc.Get("Text"),
                    //    TextApp = doc.Get("TextApp"),
                    //     TextInp= doc.Get("TextInp"),
                    //    TextLit = doc.Get("TextLit"),
                    //    TextObj = doc.Get("TextObj"),
                    //    TextOut = doc.Get("TextOut")
                    //};
                    //doc.Boost
                    products.Add(int.Parse(doc.Get("IDFE")));//,g
                }
                return products;
            }
        }


        static public void rus()
        {
            //Lucene.Net.Analysis.Ru.RussianAnalyzer r = new Lucene.Net.Analysis.Ru.RussianAnalyzer();

        }

        }
}