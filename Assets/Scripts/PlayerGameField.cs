using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameField : MonoBehaviour 
{

	public static event System.Action onScoreChange;


	public string Name { get; private set; }
	public int CurrentScore { get; private set; }


	public Text playerNameText;
	public Text scoreText;


	public void AddPont()
	{
		CurrentScore++;
		scoreText.text = CurrentScore.ToString();
		if (onScoreChange != null)
		{
			onScoreChange();
		}
	}


	public void SetName(string playerName)
	{
		Name = playerName;
		playerNameText.text = Name;
	}


	void OnEnable()
	{
		CurrentScore = 0;
		scoreText.text = CurrentScore.ToString();
	}
}
