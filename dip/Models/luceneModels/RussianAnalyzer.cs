
//using Lucene.Net.Analysis.Core;
//using Lucene.Net.Analysis.Miscellaneous;
//using Lucene.Net.Analysis.Snowball;
//using Lucene.Net.Analysis.Standard;
//using Lucene.Net.Analysis.Util;
//using Lucene.Net.Support;
//using Lucene.Net.Util;
//using System;
//using System.IO;
//using System.Text;

//namespace Lucene.Net.Analysis.Ru
//{
//    /*
//	 * Licensed to the Apache Software Foundation (ASF) under one or more
//	 * contributor license agreements.  See the NOTICE file distributed with
//	 * this work for additional information regarding copyright ownership.
//	 * The ASF licenses this file to You under the Apache License, Version 2.0
//	 * (the "License"); you may not use this file except in compliance with
//	 * the License.  You may obtain a copy of the License at
//	 *
//	 *     http://www.apache.org/licenses/LICENSE-2.0
//	 *
//	 * Unless required by applicable law or agreed to in writing, software
//	 * distributed under the License is distributed on an "AS IS" BASIS,
//	 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	 * See the License for the specific language governing permissions and
//	 * limitations under the License.
//	 */

//    /// <summary>
//    /// <see cref="Analyzer"/> for Russian language. 
//    /// <para>
//    /// Supports an external list of stopwords (words that
//    /// will not be indexed at all).
//    /// A default set of stopwords is used unless an alternative list is specified.
//    /// </para>
//    /// <para>You must specify the required <see cref="LuceneVersion"/>
//    /// compatibility when creating <see cref="RussianAnalyzer"/>:
//    /// <list type="bullet">
//    ///     <item><description> As of 3.1, <see cref="StandardTokenizer"/> is used, Snowball stemming is done with
//    ///        <see cref="SnowballFilter"/>, and Snowball stopwords are used by default.</description></item>
//    /// </list>
//    /// </para>
//    /// </summary>
//    public sealed class RussianAnalyzer : StopwordAnalyzerBase
//    {
//        /// <summary>
//        /// List of typical Russian stopwords. (for backwards compatibility) </summary>
//        /// @deprecated (3.1) Remove this for LUCENE 5.0 
//        [Obsolete("(3.1) Remove this for LUCENE 5.0")]
//        private static readonly string[] RUSSIAN_STOP_WORDS_30 = new string[] {
//            "а", "без", "более", "бы", "был", "была", "были", "было", "быть", "в",
//            "вам", "вас", "весь", "во", "вот", "все", "всего", "всех", "вы", "где",
//            "да", "даже", "для", "до", "его", "ее", "ей", "ею", "если", "есть",
//            "еще", "же", "за", "здесь", "и", "из", "или", "им", "их", "к", "как",
//            "ко", "когда", "кто", "ли", "либо", "мне", "может", "мы", "на", "надо",
//            "наш", "не", "него", "нее", "нет", "ни", "них", "но", "ну", "о", "об",
//            "однако", "он", "она", "они", "оно", "от", "очень", "по", "под", "при",
//            "с", "со", "так", "также", "такой", "там", "те", "тем", "то", "того",
//            "тоже", "той", "только", "том", "ты", "у", "уже", "хотя", "чего", "чей",
//            "чем", "что", "чтобы", "чье", "чья", "эта", "эти", "это", "я"
//        };

//        /// <summary>
//        /// File containing default Russian stopwords. </summary>
//        public const string DEFAULT_STOPWORD_FILE = "russian_stop.txt";

//        private class DefaultSetHolder
//        {
//            /// @deprecated (3.1) remove this for Lucene 5.0 
//            [Obsolete("(3.1) remove this for Lucene 5.0")]
//            internal static readonly CharArraySet DEFAULT_STOP_SET_30 = CharArraySet.UnmodifiableSet(new CharArraySet(LuceneVersion.LUCENE_CURRENT, Arrays.AsList(RUSSIAN_STOP_WORDS_30), false));
//            internal static readonly CharArraySet DEFAULT_STOP_SET;

//            static DefaultSetHolder()
//            {
//                try
//                {
//                    DEFAULT_STOP_SET = WordlistLoader.GetSnowballWordSet(
//                        IOUtils.GetDecodingReader(typeof(SnowballFilter), DEFAULT_STOPWORD_FILE, Encoding.UTF8),
//#pragma warning disable 612, 618
//                        LuceneVersion.LUCENE_CURRENT);
//#pragma warning restore 612, 618
//                }
//                catch (IOException ex)
//                {
//                    // default set should always be present as it is part of the
//                    // distribution (JAR)
//                    throw new Exception("Unable to load default stopword set", ex);
//                }
//            }
//        }

//        private readonly CharArraySet stemExclusionSet;

//        /// <summary>
//        /// Returns an unmodifiable instance of the default stop-words set.
//        /// </summary>
//        /// <returns> an unmodifiable instance of the default stop-words set. </returns>
//        public static CharArraySet DefaultStopSet
//        {
//            get
//            {
//                return DefaultSetHolder.DEFAULT_STOP_SET;
//            }
//        }

//        public RussianAnalyzer(LuceneVersion matchVersion)
//#pragma warning disable 612, 618
//            : this(matchVersion, matchVersion.OnOrAfter(LuceneVersion.LUCENE_31) ?
//                  DefaultSetHolder.DEFAULT_STOP_SET : DefaultSetHolder.DEFAULT_STOP_SET_30)
//#pragma warning restore 612, 618
//        {
//        }

//        /// <summary>
//        /// Builds an analyzer with the given stop words
//        /// </summary>
//        /// <param name="matchVersion">
//        ///          lucene compatibility version </param>
//        /// <param name="stopwords">
//        ///          a stopword set </param>
//        public RussianAnalyzer(LuceneVersion matchVersion, CharArraySet stopwords)
//            : this(matchVersion, stopwords, CharArraySet.EMPTY_SET)
//        {
//        }

//        /// <summary>
//        /// Builds an analyzer with the given stop words
//        /// </summary>
//        /// <param name="matchVersion">
//        ///          lucene compatibility version </param>
//        /// <param name="stopwords">
//        ///          a stopword set </param>
//        /// <param name="stemExclusionSet"> a set of words not to be stemmed </param>
//        public RussianAnalyzer(LuceneVersion matchVersion, CharArraySet stopwords, CharArraySet stemExclusionSet)
//            : base(matchVersion, stopwords)
//        {
//            this.stemExclusionSet = CharArraySet.UnmodifiableSet(CharArraySet.Copy(matchVersion, stemExclusionSet));
//        }

//        /// <summary>
//        /// Creates
//        /// <see cref="TokenStreamComponents"/>
//        /// used to tokenize all the text in the provided <see cref="TextReader"/>.
//        /// </summary>
//        /// <returns> <see cref="TokenStreamComponents"/>
//        ///         built from a <see cref="StandardTokenizer"/> filtered with
//        ///         <see cref="StandardFilter"/>, <see cref="LowerCaseFilter"/>, <see cref="StopFilter"/>
//        ///         , <see cref="SetKeywordMarkerFilter"/> if a stem exclusion set is
//        ///         provided, and <see cref="SnowballFilter"/> </returns>
//        protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
//        {
//#pragma warning disable 612, 618
//            if (m_matchVersion.OnOrAfter(LuceneVersion.LUCENE_31))
//#pragma warning restore 612, 618
//            {
//                Tokenizer source = new StandardTokenizer(m_matchVersion, reader);
//                TokenStream result = new StandardFilter(m_matchVersion, source);
//                result = new LowerCaseFilter(m_matchVersion, result);
//                result = new StopFilter(m_matchVersion, result, m_stopwords);
//                if (stemExclusionSet.Count > 0)
//                {
//                    result = new SetKeywordMarkerFilter(result, stemExclusionSet);
//                }
//                result = new SnowballFilter(result, new Tartarus.Snowball.Ext.RussianStemmer());
//                return new TokenStreamComponents(source, result);
//            }
//            else
//            {
//#pragma warning disable 612, 618
//                Tokenizer source = new RussianLetterTokenizer(m_matchVersion, reader);
//#pragma warning restore 612, 618
//                TokenStream result = new LowerCaseFilter(m_matchVersion, source);
//                result = new StopFilter(m_matchVersion, result, m_stopwords);
//                if (stemExclusionSet.Count > 0)
//                {
//                    result = new SetKeywordMarkerFilter(result, stemExclusionSet);
//                }
//                result = new SnowballFilter(result, new Tartarus.Snowball.Ext.RussianStemmer());
//                return new TokenStreamComponents(source, result);
//            }
//        }
//    }
//}





















////----------------------------------------------------------------------------------------------------------------------------------------------------

















////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Web;

//////namespace dip.Models.luceneModels
//////{
//////    public class RussianAnalyzer
//////    {
//////    }
//////}






//using System.Collections;
//using Lucene.Net.Analysis;
//using Version = Lucene.Net.Util.Version;
//using System.IO;

//namespace Lucene.Net.Analysis.Ru
//{
//    /// <summary>
//    /// Analyzer for Russian language. Supports an external list of stopwords (words that
//    /// will not be indexed at all).
//    /// A default set of stopwords is used unless an alternative list is specified.
//    /// </summary>
//    public sealed class RussianAnalyzer : Analyzer
//    {
//        /// <summary>
//        /// List of typical Russian stopwords.
//        /// </summary>
//        private static readonly String[] RUSSIAN_STOP_WORDS = {
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

//        private static class DefaultSetHolder
//        {
//            internal static readonly ISet<string> DEFAULT_STOP_SET = CharArraySet.UnmodifiableSet(new CharArraySet((IEnumerable<string>)RUSSIAN_STOP_WORDS, false));
//        }

//        /// <summary>
//        /// Contains the stopwords used with the StopFilter.
//        /// </summary>
//        private readonly ISet<string> stopSet;

//        private readonly Version matchVersion;


//        public RussianAnalyzer(Version matchVersion)
//            : this(matchVersion, DefaultSetHolder.DEFAULT_STOP_SET)
//        {
//        }

//        /*
//         * Builds an analyzer with the given stop words.
//         * @deprecated use {@link #RussianAnalyzer(Version, Set)} instead
//         */
//        public RussianAnalyzer(Version matchVersion, params string[] stopwords)
//            : this(matchVersion, StopFilter.MakeStopSet(stopwords))
//        {

//        }

//        /*
//         * Builds an analyzer with the given stop words
//         * 
//         * @param matchVersion
//         *          lucene compatibility version
//         * @param stopwords
//         *          a stopword set
//         */
//        public RussianAnalyzer(Version matchVersion, ISet<string> stopwords)
//        {
//            stopSet = CharArraySet.UnmodifiableSet(CharArraySet.Copy(stopwords));
//            this.matchVersion = matchVersion;
//        }

//        /*
//         * Builds an analyzer with the given stop words.
//         * TODO: create a Set version of this ctor
//         * @deprecated use {@link #RussianAnalyzer(Version, Set)} instead
//         */
//        public RussianAnalyzer(Version matchVersion, IDictionary<string, string> stopwords)
//            : this(matchVersion, stopwords.Keys.ToArray())
//        {
//        }

//        /*
//         * Creates a {@link TokenStream} which tokenizes all the text in the 
//         * provided {@link Reader}.
//         *
//         * @return  A {@link TokenStream} built from a 
//         *   {@link RussianLetterTokenizer} filtered with 
//         *   {@link RussianLowerCaseFilter}, {@link StopFilter}, 
//         *   and {@link RussianStemFilter}
//         */
//        public override TokenStream TokenStream(String fieldName, TextReader reader)
//        {
//            TokenStream result = new RussianLetterTokenizer(reader);
//            result = new LowerCaseFilter(result);
//            result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(matchVersion),
//                                    result, stopSet);
//            result = new RussianStemFilter(result);
//            return result;
//        }

//        private class SavedStreams
//        {
//            protected internal Tokenizer source;
//            protected internal TokenStream result;
//        };

//        /*
//         * Returns a (possibly reused) {@link TokenStream} which tokenizes all the text 
//         * in the provided {@link Reader}.
//         *
//         * @return  A {@link TokenStream} built from a 
//         *   {@link RussianLetterTokenizer} filtered with 
//         *   {@link RussianLowerCaseFilter}, {@link StopFilter}, 
//         *   and {@link RussianStemFilter}
//         */
//        public override TokenStream ReusableTokenStream(String fieldName, TextReader reader)
//        {
//            SavedStreams streams = (SavedStreams)PreviousTokenStream;
//            if (streams == null)
//            {
//                streams = new SavedStreams();
//                streams.source = new RussianLetterTokenizer(reader);
//                streams.result = new LowerCaseFilter(streams.source);
//                streams.result = new StopFilter(StopFilter.GetEnablePositionIncrementsVersionDefault(matchVersion),
//                                                streams.result, stopSet);
//                streams.result = new RussianStemFilter(streams.result);
//                PreviousTokenStream = streams;
//            }
//            else
//            {
//                streams.source.Reset(reader);
//            }
//            return streams.result;
//        }
//    }
//}




