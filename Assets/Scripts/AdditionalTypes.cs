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
