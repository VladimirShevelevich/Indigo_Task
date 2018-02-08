using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Script controls letter gameobject which appears on the scene
/// </summary>
public class LetterButton : MonoBehaviour {

    public string letter;

    // Reference to attached text gameobject
    GameObject Text;

    private void Start()
    {
        // Assign object to reference
        Text = transform.Find("Text").gameObject;
        Text.GetComponent<Text>().text = letter;
    }

    public void OnPressButton()
    {
        // If button was pressed and play mode is active disable Image and Text 
        if (!GameController.instance.isPlayMode)
            return;
        GameController.instance.CheckLetter(letter);
        GetComponent<Image>().enabled = false;
        Text.SetActive(false);
    }

    public void ReturnButton()
    {
        // Enable Image and Text back
        GetComponent<Image>().enabled = true;
        Text.SetActive(true);
    }
}
