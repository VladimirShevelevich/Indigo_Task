using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class WordTable {

    Dictionary<string, int> dict;
    string[] words;

    public WordTable(string text)
    {
        dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        words = text.Split(new[] { ' ', ',', ':', '?', '!', '\'', '.','-','(',')','`',';','\n','"','_','*',']','[' }, StringSplitOptions.RemoveEmptyEntries);
        CreateTable();
    }

    void CreateTable()
    {
        int minLength = GameController.instance.settings.minimumLength;
        for (int i = 0; i < words.Length; i++)
        {
            String word = words[i];
            if (word.Length >= minLength)
            {
                if (dict.ContainsKey(word))
                    dict[words[i]]++;
                else
                    dict.Add(word, 1);
            }
        }
    }

    public Word GetWord()
    {
        if (dict.Count == 0)
            return null;
        switch (GameController.instance.settings.choiceMethod)
        {
            case ChoiceMethod.random:
                return GetRandom();
            case ChoiceMethod.mostFrequent:
                return GetMax();
            case ChoiceMethod.lessFrequent:
                return GetMin();
            default:
                return null;
        }
    }

    public void RefreshTable()
    {
        CreateTable();
    }

    Word GetRandom()
    {
        var random = new System.Random().Next(dict.Count);
        string randomKey = dict.ElementAt(random).Key;
        dict.Remove(randomKey);
        return(new Word(randomKey));
    }

    Word GetMax()
    {
        int maxValue = dict.Values.Max();
        string maxKey = dict.FirstOrDefault(x => x.Value == maxValue).Key;
        dict.Remove(maxKey);
        return (new Word(maxKey));
    }

    Word GetMin()
    {
        int minValue = dict.Values.Min();
        string minKey = dict.FirstOrDefault(x => x.Value == minValue).Key;
        dict.Remove(minKey);
        return (new Word(minKey));
    }
}
