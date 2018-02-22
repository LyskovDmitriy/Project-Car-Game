using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpUI : MonoBehaviour 
{

	public GameObject helpScreen;


	public void ActivateHelp()
	{
		helpScreen.SetActive(true);
	}


	public void DeactivateHelp()
	{
		helpScreen.SetActive(false);
	}
}
