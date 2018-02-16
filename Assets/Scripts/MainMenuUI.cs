using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour 
{

	public GameObject gameSettingsScreen;
	public Dropdown playersNumberDropdown;
	public InputField[] inputFields;
	public int minNumberOfPlayers;
	public int maxNumberOfPlayers;
	public int defaultNumberOfPlayers;


	private int currentNumberOfPlayers;


	public void ActivateGameSettingsScreen()
	{
		gameSettingsScreen.SetActive(true);
		//TODO can load previous game settings
	}


	public void ChangeNumberOfPlayers(int number) //change number of fields where players enter names
	{
		currentNumberOfPlayers = number + minNumberOfPlayers;
		for (int i = 0; i < currentNumberOfPlayers; i++) //activate chosen number of players
		{
			inputFields[i].gameObject.SetActive(true);
		}
		for (int i = currentNumberOfPlayers; i < maxNumberOfPlayers; i++) //deactivate the others
		{
			inputFields[i].gameObject.SetActive(false);
		}
	}


	public void CloseGameSettingsScreen()
	{
		gameSettingsScreen.SetActive(false);
	}


	public void StartGame()
	{
		SavePlayerData();
		SceneManager.LoadScene("Game");
	}


	void Start()
	{
		playersNumberDropdown.ClearOptions();
		//create options for the dropdown
		List<string> numberOfPlayersOptions = new List<string>();
		for (int i = minNumberOfPlayers; i <= maxNumberOfPlayers; i++)
		{
			numberOfPlayersOptions.Add(i.ToString());
		}
		playersNumberDropdown.AddOptions(numberOfPlayersOptions);
		ChangeNumberOfPlayers(defaultNumberOfPlayers - minNumberOfPlayers);

		currentNumberOfPlayers = defaultNumberOfPlayers;
	}


	void SavePlayerData()
	{
		PlayerPrefs.SetInt("Number of players", currentNumberOfPlayers);

		for (int i = 0; i < currentNumberOfPlayers; i++)
		{
			string playerString = "Player" + (i + 1);
			if (inputFields[i].text == "")
			{
				PlayerPrefs.SetString(playerString, playerString);
			}
			else
			{
				PlayerPrefs.SetString(playerString, inputFields[i].text);
			}
		}
	}
}
