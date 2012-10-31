using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SentAnalizer.Core
{
    class WordsMatrix
    {
        public Dictionary<string, Dictionary<string, int>> dict = new Dictionary<string, Dictionary<string, int>>();

        public static WordsMatrix Build(string file)
        {
            WordsMatrix wm = new WordsMatrix();
            Stemmer stem = new Stemmer();
            string text = File.ReadAllText(file).Replace("\n","").Replace("\r","").Replace("\"","").Replace("[","").Replace("]","");
            var sentenses = text.Split(new[] {'.'});
            foreach(var sent in sentenses)
            {
                var words = sent.Split(new[] {' ', '-', ':', ';'}, StringSplitOptions.RemoveEmptyEntries).Where(s=>s.Length>2)
                    .Select(s=>stem.Stem(s)).ToArray();
                foreach (var word in words)
                {
                    if (!wm.dict.ContainsKey(word))
                    {
                        wm.dict.Add(word, new Dictionary<string, int>());
                    }
                    for (int i = 0; i < words.Count(); i++)
                    {
                        var w1 = words[i];
                        if (w1 != word)
                        {
                            if (!wm.dict[word].ContainsKey(w1))
                            {
                                wm.dict[word].Add(w1, i);
                            }
                            else
                            {
                                wm.dict[word][w1] = (wm.dict[word][w1] + i)/2;
                            }
                        }
                    }
                }
            }
            return wm;
        }
    }
}
