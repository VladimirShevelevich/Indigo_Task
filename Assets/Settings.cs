using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings file")]
public class Settings : ScriptableObject {

    public int minLength;
    public int attempts;
}
