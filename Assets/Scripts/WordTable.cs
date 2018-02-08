using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Class containing dictionary with words and the number of each
/// </summary>
public class WordTable {

    Dictionary<string, int> dict;

    // List of all words gotten after splitting
    string[] words;

    public WordTable(string text)
    {
        // Initialize dictinary that ignores case
        dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        // Split the text and add to array
        words = text.Split(new[] { ' ', ',', ':', '?', '!', '\'', '.','-','(',')','`',';','\n','"','_','*',']','[' }, StringSplitOptions.RemoveEmptyEntries);
        CreateTable();
    }

    void CreateTable()
    {
        // For every word longer that minimum Length add to the dictionary
        int minLength = GameController.instance.settings.minimumLength;
        for (int i = 0; i < words.Length; i++)
        {
            // If the word is not in the dictionary add it there, otherwise increase its value
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

    // According to the Choice method return a word
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

    // Create a new dictiocary from existing array 'words'
    public void RefreshTable()
    {
        CreateTable();
    }

    // Get a random value of dictionary count return appropriate element and remove it from the dictionary
    Word GetRandom()
    {
        var random = new System.Random().Next(dict.Count);
        string randomKey = dict.ElementAt(random).Key;
        dict.Remove(randomKey);
        return(new Word(randomKey));
    }

    // Get the most mentioned word, return it and remove from the dictionary
    Word GetMax()
    {
        int maxValue = dict.Values.Max();
        string maxKey = dict.FirstOrDefault(x => x.Value == maxValue).Key;
        dict.Remove(maxKey);
        return (new Word(maxKey));
    }

    // Get the rarest word, return it and remove from the dictionary
    Word GetMin()
    {
        int minValue = dict.Values.Min();
        string minKey = dict.FirstOrDefault(x => x.Value == minValue).Key;
        dict.Remove(minKey);
        return (new Word(minKey));
    }
}
