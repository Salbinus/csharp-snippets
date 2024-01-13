using System;
using System.Collections.Generic;
using System.Globalization;

class WordsToNumberConverter
{
    private static Dictionary<string, int> basicNumbers = new Dictionary<string, int>
    {
        {"null", 0}, {"eins", 1}, {"zwei", 2}, {"drei", 3}, {"vier", 4}, {"fünf", 5},
        {"sechs", 6}, {"sieben", 7}, {"acht", 8}, {"neun", 9}, {"zehn", 10},
        {"elf", 11}, {"zwölf", 12}, {"dreizehn", 13}, {"vierzehn", 14}, {"fünfzehn", 15},
        {"sechzehn", 16}, {"siebzehn", 17}, {"achtzehn", 18}, {"neunzehn", 19},
        {"zwanzig", 20}, {"dreißig", 30}, {"vierzig", 40}, {"fünfzig", 50},
        {"sechzig", 60}, {"siebzig", 70}, {"achtzig", 80}, {"neunzig", 90}
    };

    private static Dictionary<string, int> multipliers = new Dictionary<string, int>
    {
        {"hundert", 100}, {"tausend", 1000}, {"million", 1000000}
    };

    public static decimal WortZuZahl(string wort)
    {
        decimal result = 0;
        int current = 0;
        decimal decimalPart = 0;
        bool processingDecimalPart = false;
        var words = wort.Split(new[] { ' ', 'komma' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
        {
            if (word.Equals("komma"))
            {
                processingDecimalPart = true;
                result += current;
                current = 0;
                continue;
            }

            if (basicNumbers.ContainsKey(word))
            {
                current += basicNumbers[word];
            }
            else if (multipliers.ContainsKey(word))
            {
                current = current == 0 ? multipliers[word] : current * multipliers[word];
                if (word == "tausend" || word == "million")
                {
                    result += current;
                    current = 0;
                }
            }
            else if (word.Contains("und"))
            {
                var parts = word.Split(new[] { "und" }, StringSplitOptions.RemoveEmptyEntries);
                current += basicNumbers[parts[1]] * 10 + basicNumbers[parts[0]];
            }

            if (processingDecimalPart)
            {
                decimalPart = decimalPart + current;
                decimalPart = decimalPart / 10;
                current = 0;
            }
        }

        return result + current + decimalPart;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine(WordsToNumberConverter.WortZuZahl("zwei Millionen dreihunderttausendachtundvierzig komma neun acht"));
        // Sollte 2300048.98 ausgeben
    }
}