using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsList
{
	
	public List<string> words;


	public string GetRandomWord()
	{
		return words[Random.Range(0, words.Count)];
	}


	public WordsList()
	{
		words = new List<string>();
	}
}
	
public class Player
{
	
	public string name;
	public int score;


	public Player(string playerName, int playerScore)
	{
		name = playerName;
		score = playerScore;
	}
}

[System.Serializable]
public class LocalizationItem
{
	public string key;
	public string value;
}

[System.Serializable]
public class LanguageData
{
	public LocalizationItem[] items;
}

[System.Serializable]
public class LanguageFileAndFontData
{
	public SystemLanguage language;
	public Font font;
	public string localizedTextFileName;
}

[System.Serializable]
public class LocalizedWordsData
{
	public SystemLanguage language;
	public string cityWordsFileName;
	public string countrysideWordsFileName;
}