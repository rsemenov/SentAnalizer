using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SentAnalizer.Core
{
  
public class Stemmer {  
  
    private static  Regex PERFECTIVEGROUND = new Regex("((ив|ивши|ившись|ыв|ывши|ывшись)|((?<=[ая])(в|вши|вшись)))$");  
  
    private static Regex REFLEXIVE = new Regex("(с[яь])$");  
  
    private static Regex ADJECTIVE = new Regex("(ее|ие|ые|ое|ими|ыми|ей|ий|ый|ой|ем|им|ым|ом|его|ого|ему|ому|их|ых|ую|юю|ая|яя|ою|ею)$");  
  
    private static Regex PARTICIPLE = new Regex("((ивш|ывш|ующ)|((?<=[ая])(ем|нн|вш|ющ|щ)))$");  
  
    private static Regex VERB = new Regex("((ила|ыла|ена|ейте|уйте|ите|или|ыли|ей|уй|ил|ыл|им|ым|ен|ило|ыло|ено|ят|ует|уют|ит|ыт|ены|ить|ыть|ишь|ую|ю)|((?<=[ая])(ла|на|ете|йте|ли|й|л|ем|н|ло|но|ет|ют|ны|ть|ешь|нно)))$");  
  
    private static Regex NOUN = new Regex("(а|ев|ов|ие|ье|е|иями|ями|ами|еи|ии|и|ией|ей|ой|ий|й|иям|ям|ием|ем|ам|ом|о|у|ах|иях|ях|ы|ь|ию|ью|ю|ия|ья|я)$");  
  
    private static Regex RVRE = new Regex( "^(.*?[аеиоуыэюя])(.*)$");  
  
    private static Regex DERIVATIONAL = new Regex(".*[^аеиоуыэюя]+[аеиоуыэюя].*ость?$");  
  
    private static Regex DER = new Regex("ость?$");  
  
    private static Regex SUPERLATIVE = new Regex("(ейше|ейш)$");  
  
    private static Regex I = new Regex("и$");  
    private static Regex P = new Regex("ь$");  
    private static Regex NN = new Regex("нн$");  
  
    public String Stem(String word) 
    {  
        word = word.ToLowerInvariant();  
        word = word.Replace('ё', 'е');
        var m = RVRE.Match(word);  
        if (m.Success) {  
            String pre = m.Groups[1].Value;  
            String rv = m.Groups[2].Value;
            String temp = PERFECTIVEGROUND.Replace(rv, "", 1);  
            if (temp.Equals(rv)) {
                rv = REFLEXIVE.Replace(rv, "", 1);
                temp = ADJECTIVE.Replace(rv, "", 1);  
                if (!temp.Equals(rv)) {  
                    rv = temp;
                    rv = PARTICIPLE.Replace(rv, "", 1);  
                } else {
                    temp = VERB.Replace(rv, "", 1);  
                    if (temp.Equals(rv)) {  
                        rv = NOUN.Replace(rv,"",1);  
                    } else {  
                        rv = temp;  
                    }  
                }  
  
            } else {  
                rv = temp;  
            }  
  
            rv = I.Replace(rv,"",1);  
  
            if (DERIVATIONAL.Match(rv).Success) {
                rv = DER.Replace(rv, "", 1);  
            }

            temp = P.Replace(rv, "", 1);  
            if (temp.Equals(rv)) {
                rv = SUPERLATIVE.Replace(rv, "", 1);
                rv = NN.Replace(rv, "н", 1);  
            }else{  
                rv = temp;  
            }  
            word = pre + rv;  
  
        }  
  
        return word;  
    }  
  
}  
}
