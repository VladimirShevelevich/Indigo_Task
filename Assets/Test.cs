using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Test : MonoBehaviour {


    string myText;
    Dictionary<string, int> dict;


    void Start ()
    {

        //AddToTable();
        //var random = new System.Random().Next(dict.Count);
        //Debug.Log(dict.ElementAt(random).Key);
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowMax();
    }

    void ShowMax()
    {
        int maxValue = dict.Values.Max();
        string maxKey = dict.FirstOrDefault(x => x.Value == maxValue).Key;
        Debug.Log(maxKey);
        dict.Remove(maxKey);
    }

    string [] SplitText(string s)
    {
        return s.Split(new[] { ' ', ',', ':', '?', '!', '\'', '.' }, StringSplitOptions.RemoveEmptyEntries);
    }

    void AddToTable()
    {
        dict = new Dictionary<string, int>();
        string[] stringArray = SplitText(myText);
        for (int i=0; i < stringArray.Length; i++)
        {
            String word = stringArray[i];
            if (dict.ContainsKey(word))
                dict[stringArray[i]]++;
            else
                dict.Add(word, 1);
        }
    }
}
