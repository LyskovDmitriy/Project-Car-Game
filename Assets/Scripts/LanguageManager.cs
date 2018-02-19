using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LanguageManager : MonoBehaviour 
{

	public static LanguageManager Instance{ get; private set; }
	

	public LanguageFileAndFontData[] languageFiles;


	private Dictionary<string, string> currentLanguageDictionary;
	private LanguageFileAndFontData currentLanguageData;
	private SystemLanguage defaultLanguage = SystemLanguage.English;
	private string missingKeyText = "Language mistake";


	public string GetLocalizedText(string key)
	{
		if (currentLanguageDictionary.ContainsKey(key))
		{
			return currentLanguageDictionary[key];
		}
		else
		{
			return missingKeyText;
		}
	}


	public Font GetCurrentLanguageFont()
	{
		return currentLanguageData.font;
	}


	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
			
		LoadLocalizedText();
	}


	void LoadLocalizedText()
	{
		string fileName = GetLanguageFileName(Application.systemLanguage);
		string languageDataAsJSON = "";
		string filePath = "";
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		filePath = Path.Combine(Application.streamingAssetsPath, fileName);
		languageDataAsJSON = File.ReadAllText(filePath);
		#elif UNITY_ANDROID
		filePath = Path.Combine("jar:file://" + Application.dataPath + "!/assets/", fileName);
		WWW data = new WWW(filePath);
		while (!data.isDone) {}
		languageDataAsJSON = data.text;
		#elif Unity_IOS
		filePath = Path.Combine(Application.dataPath + "/Raw", dataFileName);
		#endif
		LanguageData currentLanguageData = JsonUtility.FromJson<LanguageData>(languageDataAsJSON);
		currentLanguageDictionary = TranslateLanguageDataIntoDictionary(currentLanguageData);
	}


	string GetLanguageFileName(SystemLanguage languageToFind)
	{
		for (int i = 0; i < languageFiles.Length; i++)
		{
			if (languageFiles[i].language == languageToFind)
			{
				currentLanguageData = languageFiles[i];
				return languageFiles[i].localizedTextFileName;
			}
		}

		return GetLanguageFileName(defaultLanguage);
	}


	Dictionary<string, string> TranslateLanguageDataIntoDictionary(LanguageData data)
	{
		Dictionary<string, string> textDictionary = new Dictionary<string, string>();
		for (int i = 0; i < data.items.Length; i++)
		{
			if (!textDictionary.ContainsKey(data.items[i].key))
			{
				textDictionary.Add(data.items[i].key, data.items[i].value);
			}
			else
			{
				Debug.Log("Duplicated key in localized data");
			}
		}
		return textDictionary;
	}
}
