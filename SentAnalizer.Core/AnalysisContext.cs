using System.Collections.Generic;
using System.IO;
using System;

namespace SentAnalizer.Core
{
    public class AnalysisContext
    {
        public List<IWord> Entities { get; set; }
        public List<IWord> SentimentWords { get; set; }
        private Word[] allwords;

        public AnalysisContext()
        {
            allwords = new Word[100000];
            Entities = new List<IWord>();
            SentimentWords = new List<IWord>();
        }

        public static AnalysisContext CreateContext()
        {
            AnalysisContext cont = new AnalysisContext();
            var lines = File.ReadAllLines(Constants.WordsFile);
            foreach(var line in lines)
            {
                var pp = line.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                if(pp.Length>1)
                {
                    var w = new Word() {Base = pp[1], id = int.Parse(pp[0])};
                    for (int i = 2; i < pp.Length; i++)
                        
                        w.Variations.AddLast(pp[i]);
                    cont.allwords[w.id] = w;
                }
            }
            lines = File.ReadAllLines(Constants.EntitiesFile);
            foreach(var l in lines)
            {
                var pp = l.Split(';');
                if (pp.Length == 1)
                    cont.Entities.Add(cont.allwords[int.Parse(pp[0])]);
                else
                {
                    var f = new Frase();
                    foreach(var p in pp)
                    {
                        f.Add(cont.allwords[int.Parse(p)]);
                    }
                    cont.Entities.Add(f);
                }
            }
            lines = File.ReadAllLines(Constants.FrasesFile);
            foreach (var l in lines)
            {
                var pp = l.Split(';');
                if (pp.Length == 1)
                    cont.SentimentWords.Add(cont.allwords[int.Parse(pp[0])]);
                else
                {
                    var f = new Frase();
                    foreach (var p in pp)
                    {
                        f.Add(cont.allwords[int.Parse(p)]);
                    }
                    cont.SentimentWords.Add(f);
                }
            }
            return cont;
        }
    }
}