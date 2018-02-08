using UnityEngine.UI;
using UnityEngine;

public class LetterButton : MonoBehaviour {

    public string letter;
    GameObject Text;

    private void Start()
    {
        Text = transform.Find("Text").gameObject;
        Text.GetComponent<Text>().text = letter;
    }

    public void OnPressButton()
    {
        GameController.instance.CheckLetter(letter);
        GetComponent<Image>().enabled = false;
        Text.SetActive(false);
    }

    public void ReturnButton()
    {
        GetComponent<Image>().enabled = true;
        Text.SetActive(true);
    }
}
