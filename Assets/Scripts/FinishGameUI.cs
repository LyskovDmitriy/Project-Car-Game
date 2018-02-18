using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishGameUI : MonoBehaviour 
{
	
	public GameObject finishGameScreen;
	public Text winnerText;
	public Text[] playerNameTexts;
	public Text[] playerScoreTexts;


	private SettingsUI settingsUI;
	private GameUI gameUI;


	public void FinishGame(List<Player> players)
	{
		for (int i = 0; i < players.Count; i++)
		{
			if (i == 0)
			{
				winnerText.text = players[i].name + " won!";
			}
			playerNameTexts[i].gameObject.SetActive(true);
			playerScoreTexts[i].gameObject.SetActive(true);
			playerNameTexts[i].text = players[i].name;
			playerScoreTexts[i].text = players[i].score.ToString();
		}
		for (int i = players.Count; i < playerNameTexts.Length; i++)
		{
			playerNameTexts[i].gameObject.SetActive(false);
			playerScoreTexts[i].gameObject.SetActive(false);
		}
	}


	public void Restart()
	{
		finishGameScreen.SetActive(false);
		gameUI.StartGame();
	}


	public void ChangeSettings()
	{
		settingsUI.ActivateGameSettingsScreen();
		finishGameScreen.SetActive(false);
	}


	void Awake()
	{
		gameUI = FindObjectOfType<GameUI>();
		settingsUI = FindObjectOfType<SettingsUI>();
	}
}
