using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
	public const string English = "en";

	public const string Simple_Chinese = "zh-Hans";

	public const string Traditional_Chinese = "zh-Hant";

	public const string French = "fr";

	public const string Japanese = "ja";

	private static LocalizationManager instance;

	private Dictionary<string, string> mDictionary = new Dictionary<string, string>();

	private string mLanguage;

	public string currentLanguage
	{
		get
		{
			return mLanguage;
		}
		set
		{
			if (!(mLanguage != value))
			{
				return;
			}
			if (!string.IsNullOrEmpty(value))
			{
				mLanguage = value;
				TextAsset textAsset = Resources.Load("Localization/" + value, typeof(TextAsset)) as TextAsset;
				if (textAsset != null)
				{
					mDictionary.Clear();
					Load(textAsset);
					return;
				}
			}
			mDictionary.Clear();
		}
	}

	public static LocalizationManager GetInstance()
	{
		if (instance == null)
		{
			instance = new LocalizationManager();
		}
		return instance;
	}

	public void Free()
	{
		mDictionary.Clear();
	}

	public void Load(TextAsset asset)
	{
		ByteReader byteReader = new ByteReader(asset);
		mDictionary = byteReader.ReadDictionary();
	}

	public string GetString(string key)
	{
		string value = ((!mDictionary.TryGetValue(key, out value)) ? key : value);
		return value.Replace("<n>", "\n");
	}
}
