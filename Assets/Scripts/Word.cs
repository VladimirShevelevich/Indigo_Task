using System.Collections.Generic;

/// <summary>
/// A word that should be guessed
/// </summary>
public class Word
{
    public List<Letter> letters = new List<Letter>();

    // String for displaying on console
    public string word;

    public Word(string word)
    {
        // Split the word and create letter object list
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

    // If letter was open or not
    public bool isKnown;

    public Letter(string value)
    {
        this.value = value;
    }
}
