using System;
using System.IO;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

namespace SentAnalizer.Core
{
    public class SentAnalizerAPI
    {
        private static BaesianClassifier baes;

        public static BaesianClassifier Classifier
        {
            get { return baes; }
        }

        [DllExport("Init", CallingConvention = CallingConvention.StdCall)]
        public static int Init()
        {
            try
            {
                var context = AnalysisContext.CreateContext();
                baes = new BaesianClassifier(context);
                return 0;
            }
            catch (Exception e)
            {
                File.AppendAllText("error.txt",e.ToString());
                return 1;
            }
        }

        [DllExport("Analize", CallingConvention = CallingConvention.StdCall)]
        public static int Analize([MarshalAs(UnmanagedType.LPWStr)]string text)
        {
            var sentance = new Sentence(Classifier.Context, text);
            Classifier.Analize(sentance);
            return (int)sentance.Sentiment;
        }

    }
}