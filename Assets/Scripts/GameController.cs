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
    public Text attemptsLeftText;
    int attemptLeft;
    public Text scoreText;
    int score;
    public Text resultText;

    [HideInInspector]
    public bool isPlayMode;

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
        StartCoroutine(ShowNewWord());
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

    IEnumerator ShowNewWord()
    {
        foreach(Transform child in wordPosition.transform)
        {
            Destroy(child.gameObject);
        }

        yield return currentWord = wordTable.GetRandom();
        RefreshKeyboard();

        for (int i=0; i<currentWord.letters.Count; i++)
        {
            GameObject newLetter = Instantiate(letterPrefab, wordPosition);
            SetChar(newLetter, currentWord.letters[i].value);
        }
        isPlayMode = true;
        Debug.Log("Current word: "+ currentWord.word);
        UpdateAttempts(settings.maximumAttempts);
    }

    void UpdateAttempts(int amount)
    {
        attemptLeft = amount;
        if (attemptLeft >= 0)
            attemptsLeftText.text = "Attempts: " + attemptLeft;
        else
            StartCoroutine(GameOver());
    }

    void UpdateScore(int amount)
    {
        score = amount;
        scoreText.text = "Score: " + score;
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
            StartCoroutine(Victory());
    }

    IEnumerator Victory()
    {
        isPlayMode = false;
        UpdateScore(score += attemptLeft);
        ShowMessage("Victory!");
        yield return new WaitForSeconds(1.5f);
        ShowMessage("");
        StartCoroutine(ShowNewWord());
    }

    IEnumerator GameOver()
    {
        isPlayMode = false;
        ShowMessage("Game Over!");
        yield return new WaitForSeconds(1.5f);
        ShowMessage("");
        UpdateScore(0);
        StartCoroutine(ShowNewWord());
    }

    void ShowMessage(string result)
    {
        resultText.text = result;
    }

    void RefreshKeyboard()
    {
        for (int i = 0; i<keyboard.transform.childCount; i++)
        {
            keyboard.transform.GetChild(i).GetComponent<LetterButton>().ReturnButton();
        }
    }

    public void CheckLetter(string letter)
    {
        bool success = false;
        for (int i = 0; i < currentWord.letters.Count; i++)
        {
            if (currentWord.letters[i].value.Equals(letter, System.StringComparison.OrdinalIgnoreCase))
            {
                OpenLetter(i);
                success = true;
            }
        }
        if (!success)
            UpdateAttempts(attemptLeft-1);
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
