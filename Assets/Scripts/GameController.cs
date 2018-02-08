using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using AssetBundles;

public class GameController : MonoBehaviour {

    public Settings settings;
    public GameObject letterPrefab;
    public Transform wordPosition;
    public Transform keyboard;
    public GameObject letterButtonPrefab;

    string workingText;
    WordTable wordTable;
    Word currentWord;

    public string testString;

    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    IEnumerator Start()
    {
        yield return GetTextFromBundle();
        wordTable = new WordTable(workingText);
        yield return  currentWord = wordTable.GetRandom();
        ShowNewWord();
        CreateKeyboard();
    }

    IEnumerator GetTextFromBundle()
    {
        yield return StartCoroutine(Initialize());
        yield return StartCoroutine(InstantiateGameObjectAsync("textfile", "alice30"));
    }
    protected IEnumerator Initialize()
    {
        AssetBundleManager.SetDevelopmentAssetBundleServer();
        var request = AssetBundleManager.Initialize();
        if (request != null)
            yield return StartCoroutine(request);
    }
    protected IEnumerator InstantiateGameObjectAsync(string assetBundleName, string assetName)
    {
        // Load asset from assetBundle.
        AssetBundleLoadAssetOperation request = AssetBundleManager.LoadAssetAsync(assetBundleName, assetName, typeof(TextAsset));
        if (request == null)
            yield break;
        yield return StartCoroutine(request);

        // Get the asset.
        TextAsset prefab = request.GetAsset<TextAsset>();
        workingText = prefab.text;
    }

    void CreateKeyboard()
    {
        for (char ch = 'A'; ch<='Z'; ch++)
        {
            Instantiate(letterButtonPrefab, keyboard).GetComponent<LetterButton>().letter = ch.ToString();
        }
    }

    void ShowNewWord()
    {
        for (int i=0; i<currentWord.letters.Count; i++)
        {
            GameObject newLetter = Instantiate(letterPrefab, wordPosition);
            SetChar(newLetter, currentWord.letters[i].value);
        }

        Debug.Log("Current word: "+ currentWord.word);
    }

    void SetChar(GameObject obj, string value)
    {
        obj.transform.Find("Text").GetComponent<Text>().text = value.ToUpper();
    }

    void OpenLetter(int pos)
    {
        currentWord.letters[pos].isKnown = true;
        wordPosition.GetChild(pos).Find("Black").gameObject.SetActive(false);
        if (WordIsOpen())
            Victory();
    }

    void Victory()
    {
        Debug.Log("Victory");
    }

    public void CheckLetter(string letter)
    {
        for (int i = 0; i < currentWord.letters.Count; i++)
        {
            if (currentWord.letters[i].value.Equals(letter,System.StringComparison.OrdinalIgnoreCase))
                OpenLetter(i);            
        }
    }

    bool WordIsOpen()
    {
        for (int i=0; i<currentWord.letters.Count; i++)
        {
            if (!currentWord.letters[i].isKnown)
                return false;
        }
        return true;
    }
}
