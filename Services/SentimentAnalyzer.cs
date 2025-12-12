namespace SentimentAPI.Services;

public class SentimentAnalyzer
{
    public string Analyze(string text)
    {
        
     var positivas = new[] {"excelente", "genial","fantastico","bueno","increible","buen", "positivo"};
     var negativas = new[] {"malo","terrible","problema","defecto","horrible","descompuesto","feo", "mal"};
     var lowerText = text.ToLower();
     
     if(positivas.Any(w => lowerText.Contains(w)))
     return "positivo";

     if(negativas.Any(w => lowerText.Contains(w)))
     return "negativo";

     return "neutral";


    }
}