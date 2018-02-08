using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using AssetBundles;

public class GameController : MonoBehaviour {

    #region FIELDS
    public Settings settings;
    public GameObject letterPrefab;
    public GameObject letterButtonPrefab;
    public Transform wordPosition;
    public Transform keyboard;
    public Text attemptsLeftText;
    public Text scoreText;
    public Text resultText;
    int attemptLeft;
    int score;
    [HideInInspector]
    public bool isPlayMode;

    string workingText;
    WordTable wordTable;
    Word currentWord;

    public static GameController instance;
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    IEnumerator Start()
    {
        // Load text from the asset bundle
        yield return GetTextFromBundle();

        // Create new Dictionary for text
        wordTable = new WordTable(workingText);       
        
        // Get new word from the dictionary
        StartCoroutine(ShowNewWord());

        CreateKeyboard();
    }

    IEnumerator GetTextFromBundle()
    {
        //Initialize bundle
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

        // Assign loaded text to 'workingText' string
        TextAsset prefab = request.GetAsset<TextAsset>();
        workingText = prefab.text;
    }

    void CreateKeyboard()
    {
        // Create buttons object and set a letter
        for (char ch = 'A'; ch<='Z'; ch++)
        {
            Instantiate(letterButtonPrefab, keyboard).GetComponent<LetterButton>().letter = ch.ToString();
        }
    }

    IEnumerator ShowNewWord()
    {
        // Delete an old word
        foreach (Transform child in wordPosition.transform)
        {
            Destroy(child.gameObject);
        }

        // Get word from the dictionary
        yield return currentWord = wordTable.GetWord();

        // If there is no words restart the game
        if (currentWord == null)
        {
            StartCoroutine(GameOver("Victory! Words are over!"));
            yield break;
        }

        // Restore the keyboard
        RefreshKeyboard();

        // Create letter objects and set a value
        for (int i=0; i<currentWord.letters.Count; i++)
        {
            GameObject newLetter = Instantiate(letterPrefab, wordPosition);
            SetChar(newLetter, currentWord.letters[i].value);
        }
        isPlayMode = true;
        Debug.Log("Current word: "+ currentWord.word);

        // Restore attempts to maximum
        UpdateAttempts(settings.maximumAttempts);
    }

    void UpdateAttempts(int amount)
    {
        // if there is no attempts call GameOver method
        attemptLeft = amount;
        if (attemptLeft >= 0)
            attemptsLeftText.text = "Attempts: " + attemptLeft;
        else
            StartCoroutine(GameOver("Game Over"));
    }

    void UpdateScore(int amount)
    {
        score = amount;
        scoreText.text = "Score: " + score;
    }

    void SetChar(GameObject obj, string value)
    {
        // Get text object and set a letter
        obj.transform.Find("Text").GetComponent<Text>().text = value.ToUpper();
    }

    void OpenLetter(int pos)
    {
        // remove black squere and change the letter status
        currentWord.letters[pos].isKnown = true;
        wordPosition.GetChild(pos).Find("Black").gameObject.SetActive(false);

        // Check if all letters are open call Victory() method
        if (WordIsOpen())
            StartCoroutine(Victory());
    }

    IEnumerator Victory()
    {
        isPlayMode = false;

        // Add score
        UpdateScore(score += attemptLeft);
        ShowMessage("Victory!");
        yield return new WaitForSeconds(1.5f);
        ShowMessage("");

        // Call a new word
        StartCoroutine(ShowNewWord());
    }

    IEnumerator GameOver(string message)
    {
        isPlayMode = false;
        ShowMessage(message);
        yield return new WaitForSeconds(2);
        ShowMessage("");

        // Set score to zero
        UpdateScore(0);

        // Restore the keyboard
        wordTable.RefreshTable();

        // Call a new word
        StartCoroutine(ShowNewWord());
    }

    void ShowMessage(string result)
    {
        resultText.text = result;
    }

    void RefreshKeyboard()
    {
        // Get buttons components and call Return() method
        for (int i = 0; i<keyboard.transform.childCount; i++)
        {
            keyboard.transform.GetChild(i).GetComponent<LetterButton>().ReturnButton();
        }
    }

    public void CheckLetter(string letter)
    {
        //Check if pressed letter is equal to any of the words letter ignoring case. 
        //If succeeded open the letter, otherwise reduce attempts amount
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
        // Check if every letter has 'isKnown' status
        for (int i=0; i<currentWord.letters.Count; i++)
        {
            if (!currentWord.letters[i].isKnown)
                return false;
        }
        return true;
    }
}
