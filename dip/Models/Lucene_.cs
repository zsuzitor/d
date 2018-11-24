using dip.Models.Domain;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


//http://www.waveaccess.ru/blog/2014/september/02/%D0%BF%D0%BE%D0%BB%D0%BD%D0%BE%D1%82%D0%B5%D0%BA%D1%81%D1%82%D0%BE%D0%B2%D1%8B%D0%B9-%D0%BF%D0%BE%D0%B8%D1%81%D0%BA-%D1%81-%D0%B8%D1%81%D0%BF%D0%BE%D0%BB%D1%8C%D0%B7%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B5%D0%BC-apache-lucene.aspx


//ru analizer//tok//norm  https://github.com/apache/lucenenet/tree/master/src/Lucene.Net.Analysis.Common/Analysis/Ru


namespace dip.Models
{
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
                    var document = MapProduct(i);
                    writer.AddDocument(document);
                }
            }
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
            return new SimpleFSDirectory(new DirectoryInfo(@"E:\csharp\dip1\dip\dip\lucene"));//(@"~/lucene"));
        }


        static Analyzer GetAnalyzer()
        {
            
            return new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        }




        static Query GetQuery(string keywords)
        {
            using (var analyzer = GetAnalyzer())
            {
                var query = new BooleanQuery();

                var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Name", analyzer);
                var keywordsQuery = parser.Parse(keywords);
                query.Add(keywordsQuery, Occur.SHOULD);

                parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "Text", analyzer);
                keywordsQuery = parser.Parse(keywords);
                query.Add(keywordsQuery, Occur.SHOULD);

                parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextInp", analyzer);
                keywordsQuery = parser.Parse(keywords);
                query.Add(keywordsQuery, Occur.SHOULD);

                parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextOut", analyzer);
                keywordsQuery = parser.Parse(keywords);
                query.Add(keywordsQuery, Occur.SHOULD);

                parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextObj", analyzer);
                keywordsQuery = parser.Parse(keywords);
                query.Add(keywordsQuery, Occur.SHOULD);

                parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextApp", analyzer);
                keywordsQuery = parser.Parse(keywords);
                query.Add(keywordsQuery, Occur.SHOULD);

                parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "TextLit", analyzer);
                keywordsQuery = parser.Parse(keywords);
                query.Add(keywordsQuery, Occur.SHOULD);
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
        static public List<FEText> Search(string keywords, int limit, out int count)
        {
            using (var directory = GetDirectory())
            using (var searcher = new IndexSearcher(directory))
            {
                var query = GetQuery(keywords);
                var sort = GetSort();
                //var filter = GetFilter();
                var docs = searcher.Search(query, null, limit, sort);
                count = docs.TotalHits;
                var products = new List<FEText>();
                foreach (var scoreDoc in docs.ScoreDocs)
                {
                   //var g= scoreDoc.Score;
                    var doc = searcher.Doc(scoreDoc.Doc);
                    
                    var product = new FEText { IDFE = int.Parse(doc.Get("IDFE")),
                        Name = doc.Get("Name"),
                        Text = doc.Get("Text"),
                        TextApp = doc.Get("TextApp"),
                         TextInp= doc.Get("TextInp"),
                        TextLit = doc.Get("TextLit"),
                        TextObj = doc.Get("TextObj"),
                        TextOut = doc.Get("TextOut")
                    };
                    products.Add(product);
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