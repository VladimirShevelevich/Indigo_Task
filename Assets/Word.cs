using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word
{
    public List<Letter> letters = new List<Letter>();
    public string word;

    public Word(string word)
    {
        this.word = word;
        char[] chars = word.ToCharArray();
        for (int i =0; i<chars.Length; i++)
        {
            letters.Add(new Letter(chars[i].ToString()));
        }
    }
}

public class Letter
{
    public string value;
    public bool isKnown;

    public Letter(string value)
    {
        this.value = value;
    }
}
