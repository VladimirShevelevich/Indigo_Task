using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word
{
    public List<Letter> letters;

    public Word(string word)
    {
        char[] chars = word.ToCharArray();
        for (int i =0; i<chars.Length; i++)
        {
            letters.Add(new Letter(chars[i]));
        }
    }
}

public class Letter
{
    public char value;
    public bool isKnown;

    public Letter(char value)
    {
        this.value = value;
    }
}
