using System.Collections.Generic;
using System.Linq;
using System;

namespace SentAnalizer.Core
{
    public interface IWord
    {
        string Base {get;}
        bool IsInText(string[] textwords);
    }

    public class Word : IWord
    {
        public int id { get; set; }
        public string Base { get; set; }
        public LinkedList<string> Variations { get; set; }

        public Word()
        {
            Variations = new LinkedList<string>();
        }

        #region IWord Members

        public bool IsInText(string[] textwords)
        {
            if (textwords.Any(s => s.Contains(Base, StringComparison.OrdinalIgnoreCase) 
                || Variations.Any(v => s.Contains(v, StringComparison.OrdinalIgnoreCase))))
                return true;
            else
                return false;
        }

        #endregion

        public override string ToString()
        {
            return Base;
        }
    }

    public class Frase : List<Word>, IWord
    {
        #region IWord Members

        public string Base
        {
            get { return this[0].Base; }
        }

        public bool IsInText(string[] textwords)
        {
            foreach (int firsInd in textwords.Select((w, i) => new{ind =i,word=w}).Where((w) => this[0].IsInText(new[] { w.word }))
                .Select(e=>e.ind))
            {
                var ind = firsInd + 1;
                int i = 1;
                for (i = 1; i < Count && this[i].IsInText(new[] { textwords[ind++] }); i++)
                { }
                if (i == Count)
                    return true;
            }
            return false;
        }

        #endregion

        public override string ToString()
        {
            string s = "";
            foreach (var w in this)
                s += w + " ";
            return s;
        }
    }
}