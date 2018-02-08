using UnityEngine;

/// <summary>
/// The method by which new words will be chosen
/// </summary>
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
