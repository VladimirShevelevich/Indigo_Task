using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChoiceMethod
{
    random,
    mostFrequent,
    lessFrequent
}

[CreateAssetMenu(fileName = "Settings file")]
public class Settings : ScriptableObject {

    public int minimumLength;
    public int maximumAttempts;
    public ChoiceMethod choiceMethod;
}
