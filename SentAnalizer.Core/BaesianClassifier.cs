using System.Collections.Generic;
using System.IO;

namespace SentAnalizer.Core
{
    public class BaesianClassifier
    {
        private Dictionary<IWord, int[]> positiveDict = new Dictionary<IWord, int[]>();
        private Dictionary<IWord, int[]> negativeDict = new Dictionary<IWord, int[]>();
        private AnalysisContext context;
        private int allCount = 0, positiveSentances, negtiveSentances;

        public AnalysisContext Context {get { return context; }}

        public BaesianClassifier(AnalysisContext context)
        {
            this.context = context;
            foreach(var w in context.SentimentWords)
            {
                positiveDict.Add(w, new int[2]);
                negativeDict.Add(w, new int[2]);
            }
        }

        public void Learn(Sentence sentence)
        {
            if(sentence.Sentiment == Sentiment.Positive)
            {
                positiveSentances++;
                allCount++;
                for (int i = 0; i < context.SentimentWords.Count; i++)
                {
                    var w = context.SentimentWords[i];
                    if (!positiveDict.ContainsKey(w))
                        positiveDict.Add(w, new int[2]);
                    if (sentence.SentimentWords.Contains(i))
                        positiveDict[w][1]++;
                    else
                        positiveDict[w][0]++;
                }
            }
            else
            {
                negtiveSentances++;
                allCount++;
                for (int i = 0; i < context.SentimentWords.Count; i++)
                {
                    var w = context.SentimentWords[i];
                    if (!negativeDict.ContainsKey(w))
                        negativeDict.Add(w, new int[2]);
                    if (sentence.SentimentWords.Contains(i))
                        negativeDict[w][1]++;
                    else
                        negativeDict[w][0]++;
                }
            }
        }

        public void Analize(Sentence sentence)
        {
            var distpos = GetDist(sentence, positiveDict, positiveSentances);
            var distneg = GetDist(sentence, negativeDict, negtiveSentances);
            if (distpos > distneg)
                sentence.Sentiment = Sentiment.Positive;
            else
                sentence.Sentiment = Sentiment.Negative;
        }

        double GetDist(Sentence sentence, Dictionary<IWord,int[]> dict, int classcount)
        {
            double dist = classcount / (allCount * 1.0);
            for (int i = 0; i < context.SentimentWords.Count; i++)
            {
                var word = context.SentimentWords[i];
                if (sentence.SentimentWords.Contains(i))
                    dist *= dict[word][1] / (classcount * 1.0);
                else
                    dist *= dict[word][0] / (classcount * 1.0);
            }
            return dist;
        }

        public void Teach()
        {
            foreach (var line in File.ReadAllLines(Constants.LearnPositiveFile))
            {
                Sentence s = new Sentence(context, line);
                s.GetWords();
                s.Sentiment = Sentiment.Positive;
                Learn(s);
            }

            foreach (var line in File.ReadAllLines(Constants.LearnNegativeFile))
            {
                Sentence s = new Sentence(context, line);
                s.GetWords();
                s.Sentiment = Sentiment.Negative;
                Learn(s);
            }
        }
    }
}