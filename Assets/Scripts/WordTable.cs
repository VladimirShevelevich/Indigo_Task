using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class WordTable {

    Dictionary<string, int> dict;

    public WordTable(string text)
    {
        dict = new Dictionary<string, int>();
        string[] words = text.Split(new[] { ' ', ',', ':', '?', '!', '\'', '.' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < words.Length; i++)
        {
            String word = words[i];
            if (dict.ContainsKey(word))
                dict[words[i]]++;
            else
                dict.Add(word, 1);
        }
    }

    public string GetMax()
    {
        int maxValue = dict.Values.Max();
        string maxKey = dict.FirstOrDefault(x => x.Value == maxValue).Key;
        dict.Remove(maxKey);
        return maxKey;
    }

    public string GetMin()
    {
        int minValue = dict.Values.Min();
        string minKey = dict.FirstOrDefault(x => x.Value == minValue).Key;
        dict.Remove(minKey);
        return minKey;
    }
}
