using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationComponent : MonoBehaviour 
{

	public string key;
	public bool useLanguageSettingsOnly = false;


	void Start () 
	{
		Text textComponent = GetComponent<Text>();
		if (textComponent == null)
		{
			Debug.Log("LocalizationComponent attached to wrong object: " + name);
		}
		else
		{
			if (!useLanguageSettingsOnly)
			{
				textComponent.text = LanguageManager.Instance.GetLocalizedText(key);
			}
			textComponent.font = LanguageManager.Instance.GetCurrentLanguageFont();
			textComponent.lineSpacing = LanguageManager.Instance.GetCurrentLanguageLineSpacing();
		}
	}
}
