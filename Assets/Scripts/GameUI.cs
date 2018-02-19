using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour 
{

	public GameObject gameScreen;
	public GameObject finishGameScreen;
	public PlayerGameField[] playerFields;
	public Text wordText;
	public LocalizedWordsData[] localizedWords;


	private FinishGameUI finishGameUI;
	private WordsList wordsList;
	private SystemLanguage defaultLanguage = SystemLanguage.Russian;
	private string lastWord;


	public void SetRandomWord()
	{
		if (wordsList != null)
		{
			while (true)
			{
				string word = wordsList.GetRandomWord();

				if (word == lastWord)
				{
					continue;
				}
				else
				{
					wordText.text = word;
					lastWord = word;
					break;
				}
			}
		}
	}


	public void FinishGame()
	{
		gameScreen.SetActive(false);
		finishGameScreen.SetActive(true);

		List<Player> players = new List<Player>();
		for (int i = 0; (i < playerFields.Length) && playerFields[i].gameObject.activeSelf; i++)
		{
			Player player = new Player(playerFields[i].Name, playerFields[i].CurrentScore);
			players.Add(player);
		}

		players.Sort((Player x, Player y) =>
			{
				return y.score.CompareTo(x.score); //to sort descending
			});
		
		finishGameUI.FinishGame(players);
	}


	public void StartGame()
	{
		gameScreen.SetActive(true);
		int playerNumber = PlayerPrefs.GetInt("Number of players");

		for (int i = 0; i < playerNumber; i++)
		{
			playerFields[i].gameObject.SetActive(true);
			string playerNameKey = "Player" + (i + 1);
			playerFields[i].SetName(PlayerPrefs.GetString(playerNameKey));
		}
		for (int i = playerNumber; i < playerFields.Length; i++)
		{
			playerFields[i].gameObject.SetActive(false);
		}

		lastWord = "";

		LoadWordsData();
		SetRandomWord();
	}


	void LoadWordsData() //needs to be checked on each platform
	{
		LocalizedWordsData currentWordsData = GetWordsData(Application.systemLanguage);
		string cityFileName = currentWordsData.cityWordsFileName;
		string countrysideFileName = currentWordsData.countrysideWordsFileName;

		bool isCityActive = PlayerPrefs.GetInt("City") == 1;
		bool isCountrysideActive = PlayerPrefs.GetInt("Countryside") == 1;
		string cityPath = "";
		string countrysidePath = "";
		WordsList cityWords = null;
		WordsList countrysideWords = null;

	#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (isCityActive)
		{
			cityPath = Path.Combine(Application.streamingAssetsPath, cityFileName);

			if (File.Exists(cityPath))
			{
				string dataAsJSON = File.ReadAllText(cityPath);
				cityWords = JsonUtility.FromJson<WordsList>(dataAsJSON);
			}
		}
		if (isCountrysideActive)
		{
			countrysidePath = Path.Combine(Application.streamingAssetsPath, countrysideFileName);

			if (File.Exists(countrysidePath))
			{
				string dataAsJSON = File.ReadAllText(countrysidePath);
				countrysideWords = JsonUtility.FromJson<WordsList>(dataAsJSON);
			}
		}
	#elif UNITY_ANDROID
		cityPath = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", cityFileName);
		countrysidePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", countrysideFileName);
		WWW city = null;
		WWW countryside = null;
		if (isCityActive)
		{
			city = new WWW(cityPath);
			while (!city.isDone) {}
			cityWords = JsonUtility.FromJson<WordsList>(city.text);
		}
		if (isCountrysideActive)
		{
			countryside = new WWW(countrysidePath);
			while (!countryside.isDone) {}
			countrysideWords = JsonUtility.FromJson<WordsList>(countryside.text);
		}
	#elif Unity_IOS
			filePath = Path.Combine(Application.dataPath + "/Raw", dataFileName);
	#endif

		if (isCityActive)
		{
			wordsList.words = cityWords.words;
		}
		if (isCountrysideActive)
		{
			if (!isCityActive)
			{
				wordsList.words = countrysideWords.words;
			}
			else
			{
				wordsList.words.AddRange(countrysideWords.words);
			}
		}
	}


	void Awake () 
	{
		finishGameUI = FindObjectOfType<FinishGameUI>();
		PlayerGameField.onScoreChange += SetRandomWord;
		wordsList = new WordsList();
	}


	void OnDestroy()
	{
		PlayerGameField.onScoreChange -= SetRandomWord;
	}


	LocalizedWordsData GetWordsData(SystemLanguage languageToFind)
	{
		for (int i = 0; i < localizedWords.Length; i++)
		{
			if (localizedWords[i].language == languageToFind)
			{
				return localizedWords[i];
			}
		}

		return GetWordsData(defaultLanguage);
	}
}
