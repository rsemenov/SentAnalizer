using System.Linq;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;

namespace SentAnalizer
{
    class Program
    {

        static void Main(string[] args)
        {
            Stemmer st = new Stemmer();
            //while (true)
            {
              //  var res = st.Stem(Console.ReadLine());
               // Console.WriteLine(res);
            }
            var matrix = WordsMatrix.Build("Files\\input_learn_pos.txt");

            var cont = AnalysisContext.CreateContext();
            BaesianClassifier baes = new BaesianClassifier(cont);

            foreach (var line in File.ReadAllLines("Files\\input_learn_pos.txt"))
            {
                Sentence s = new Sentence(cont, line);
                s.GetWords();
                s.Sentiment = Sentiment.Positive;
                baes.Learn(s);
            }

            foreach (var line in File.ReadAllLines("Files\\input_learn_neg.txt"))
            {
                Sentence s = new Sentence(cont, line);
                s.GetWords();
                s.Sentiment = Sentiment.Negative;
                baes.Learn(s);
            }
            List<Tuple<IWord, Sentiment>> list = new List<Tuple<IWord, Sentiment>>();
            foreach (var line in File.ReadAllLines("Files\\input.txt"))
            {
                Sentence s = new Sentence(cont, line);
                s.GetWords();
                baes.Analize(s);
                Print(s, cont);
                foreach (var i in s.Entities)
                {
                    var word = cont.Entities[i];
                    list.Add(new Tuple<IWord, Sentiment>(word, s.Sentiment));
                }
            }
            Console.WriteLine("Результат анализа");
            foreach (var t in list)
                Console.WriteLine(t.Item1 + " -> " + t.Item2);
            
            Console.ReadLine();
        }

        static void Print(Sentence s, AnalysisContext context)
        {
            Console.WriteLine(s.Text);
            Console.WriteLine("Entities:");
            foreach(var i in s.Entities)
            {
                var word = context.Entities[i];
                Console.WriteLine("-->"+word.ToString());
            }
            Console.WriteLine("Sentiment:" + s.Sentiment);
            Console.WriteLine();
        }
    }
}
