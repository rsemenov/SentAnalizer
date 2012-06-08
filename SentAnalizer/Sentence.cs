using System;
using System.Collections.Generic;

namespace SentAnalizer
{
    public class Sentence
    {
        public Sentiment Sentiment { get; set; }
        public string Text { get; set; }
        public List<int> Entities { get; private set; }
        public List<int> SentimentWords { get; private set; }
        private AnalysisContext context;

        public Sentence(AnalysisContext context, string sentence)
        {
            this.context = context;
            Text = sentence;
            Entities = new List<int>();
            SentimentWords = new List<int>();
        }

        public void GetWords()
        {
            var words = Text.Split(new[] {' ', ',', '.', '"', '-', '\'', '\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < context.Entities.Count; i++)
            {
                var word = context.Entities[i];
                if (word.IsInText(words))
                    Entities.Add(i);
            }
            for(int i=0;i<context.SentimentWords.Count;i++)
            {
                var word = context.SentimentWords[i];
                if (word.IsInText(words))
                    SentimentWords.Add(i);
            }
        }
    }
}