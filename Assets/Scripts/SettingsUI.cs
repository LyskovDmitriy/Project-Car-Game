using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour 
{

	public GameObject gameSettingsScreen;
	public Dropdown playersNumberDropdown;
	public InputField[] inputFields;
	public Toggle cityWordsToggle;
	public Toggle countrysideWordsToggle;
	public int minNumberOfPlayers;
	public int maxNumberOfPlayers;
	public int defaultNumberOfPlayers;


	private GameUI gameUI;
	private int currentNumberOfPlayers;
	private bool cityWordsActive;
	private bool countrysideWordsActive;


	public void ActivateGameSettingsScreen()
	{
		gameSettingsScreen.SetActive(true);
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
		gameUI.StartGame();
		gameSettingsScreen.SetActive(false);
	}


	public void WordTogglePressed()
	{
		cityWordsActive = cityWordsToggle.isOn;
		countrysideWordsActive = countrysideWordsToggle.isOn;

		if (cityWordsActive && !countrysideWordsActive)
		{
			cityWordsToggle.interactable = false;
		}
		else if (!cityWordsActive && countrysideWordsActive)
		{
			countrysideWordsToggle.interactable = false;
		}
		else if (cityWordsActive && countrysideWordsActive)
		{
			cityWordsToggle.interactable = true;
			countrysideWordsToggle.interactable = true;
		}
	}


	void Awake()
	{
		gameUI = FindObjectOfType<GameUI>();
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

		cityWordsActive = true;
		countrysideWordsActive = true;
		cityWordsToggle.isOn = cityWordsActive;
		countrysideWordsToggle.isOn = countrysideWordsActive;
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

		PlayerPrefs.SetInt("City", cityWordsActive ? 1 : 0);
		PlayerPrefs.SetInt("Countryside", countrysideWordsActive ? 1 : 0);
	}
}
