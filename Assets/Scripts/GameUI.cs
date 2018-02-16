using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour 
{

	public PlayerGameField[] playerFields;
	public GameObject finishGameScreen;
	public Text winnerText;
	//to display score when the game is finished
	public Text[] playerNameTexts;
	public Text[] playerScoreTexts;

	public Text wordText;
	public string dataFileName;


	private WordsList wordsList;
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
		
		for (int i = 0; i < players.Count; i++)
		{
			if (i == 0)
			{
				winnerText.text = players[i].name + " won!";
			}
			playerNameTexts[i].text = players[i].name;
			playerScoreTexts[i].text = players[i].score.ToString();
		}
	}


	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}


	public void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}


	void LoadWordsData() //needs to be checked on each platform
	{
		string filePath = "";
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
			filePath = Path.Combine(Application.streamingAssetsPath, dataFileName);
		#elif UNITY_ANDROID
			filePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", dataFileName);
			WWW www = new WWW(filePath);
			while (!www.isDone) {}
			wordsList = JsonUtility.FromJson<WordsList>(www.text);
		return;
		#elif Unity_IOS
			filePath Path.Combine(Application.dataPath + "/Raw", dataFileName);
		#endif


		if (File.Exists(filePath))
		{
			string dataAsJSON = File.ReadAllText(filePath);
			wordsList = JsonUtility.FromJson<WordsList>(dataAsJSON);
		}
		else
		{
			Debug.Log("Can't find file with words");
		}
	}


	void Start () 
	{
		PlayerGameField.onScoreChange += SetRandomWord;
		int playerNumber = PlayerPrefs.GetInt("Number of players");

		for (int i = 0; i < playerNumber; i++)
		{
			playerFields[i].gameObject.SetActive(true);
			playerNameTexts[i].gameObject.SetActive(true);
			playerScoreTexts[i].gameObject.SetActive(true);
			string playerNameKey = "Player" + (i + 1);
			playerFields[i].SetName(PlayerPrefs.GetString(playerNameKey));
		}
		for (int i = playerNumber; i < playerFields.Length; i++)
		{
			playerFields[i].gameObject.SetActive(false);
			playerNameTexts[i].gameObject.SetActive(false);
			playerScoreTexts[i].gameObject.SetActive(false);
		}

		lastWord = "";

		LoadWordsData();
		SetRandomWord();
	}


	void OnDestroy()
	{
		PlayerGameField.onScoreChange -= SetRandomWord;
	}
}
