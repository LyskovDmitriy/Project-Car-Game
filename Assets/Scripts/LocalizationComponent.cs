using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationComponent : MonoBehaviour 
{

	public string key;


	void Start () 
	{
		Text textComponent = GetComponent<Text>();
		if (textComponent == null)
		{
			Debug.Log("LocalizationComponent attached to wrong object: " + name);
		}
		else
		{
			textComponent.text = LanguageManager.Instance.GetLocalizedText(key);
			textComponent.font = LanguageManager.Instance.GetCurrentLanguageFont();
		}
	}
}
