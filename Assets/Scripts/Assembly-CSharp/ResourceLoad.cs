using UnityEngine;

internal class ResourceLoad
{
	private static ResourceLoad instance = new ResourceLoad();

	public static ResourceLoad GetInstance()
	{
		return instance;
	}

	public GameObject LoadUI(string path, string name)
	{
		string empty = string.Empty;
		empty = (LocalizationManager.GetInstance().currentLanguage.Equals("zh-Hans") ? "CN" : (LocalizationManager.GetInstance().currentLanguage.Equals("ja") ? "JP" : (LocalizationManager.GetInstance().currentLanguage.Equals("fr") ? "FR" : ((!LocalizationManager.GetInstance().currentLanguage.Equals("zh-Hant")) ? "EN" : "TW"))));
		GameObject gameObject = null;
		if (GameApp.GetInstance().IsIphone4())
		{
			gameObject = Resources.Load(path + "/UI/" + empty + "/SD" + name) as GameObject;
		}
		if (gameObject == null)
		{
			gameObject = Resources.Load(path + "/UI/" + empty + "/" + name) as GameObject;
		}
		return gameObject;
	}

	public GameObject LoadAtlas(string path1, string path2)
	{
		string empty = string.Empty;
		empty = (LocalizationManager.GetInstance().currentLanguage.Equals("zh-Hans") ? "CN" : (LocalizationManager.GetInstance().currentLanguage.Equals("ja") ? "JP" : (LocalizationManager.GetInstance().currentLanguage.Equals("fr") ? "FR" : ((!LocalizationManager.GetInstance().currentLanguage.Equals("zh-Hant")) ? "EN" : "TW"))));
		GameObject gameObject = null;
		if (GameApp.GetInstance().IsIphone4())
		{
			gameObject = Resources.Load(path1 + "/Res/" + path2 + "SD/" + empty + "/" + empty + "Atlas") as GameObject;
		}
		if (gameObject == null)
		{
			gameObject = Resources.Load(path1 + "/Res/" + path2 + "/" + empty + "/" + empty + "Atlas") as GameObject;
		}
		return gameObject;
	}

	private void ReferenceAtlas(SystemLanguage language, string path, string name)
	{
		ReferenceAtlas(language.ToString(), path, name);
	}

	private void ReferenceAtlas(string language, string path, string name)
	{
		string text = null;
		text = (language.Equals("zh-Hans") ? "CN" : (language.Equals("ja") ? "JP" : (language.Equals("fr") ? "FR" : ((!language.Equals("zh-Hant")) ? "EN" : "TW"))));
		UIAtlas uIAtlas = LoadAtlas(path + name);
		UIAtlas uIAtlas2 = LoadAtlas(path + text + "/" + text + "Atlas");
		if (uIAtlas != null && uIAtlas2 != null)
		{
			uIAtlas.replacement = uIAtlas2;
		}
	}

	private void ReferenceFont(SystemLanguage language, string path, string name)
	{
		ReferenceFont(language.ToString(), path, name);
	}

	private void ReferenceFont(string language, string path, string name)
	{
		string text = null;
		text = (language.Equals(SystemLanguage.Chinese.ToString()) ? "CN" : (language.Equals(SystemLanguage.Japanese.ToString()) ? "JP" : (language.Equals(SystemLanguage.French.ToString()) ? "FR" : ((!language.Equals(SystemLanguage.Thai.ToString())) ? "EN" : "TW"))));
		UIFont uIFont = LoadFont(path + name);
		UIFont uIFont2 = LoadFont(path + text + "/" + text + "Font");
		if (uIFont != null && uIFont2 != null)
		{
			uIFont.replacement = uIFont2;
		}
	}

	private UIAtlas LoadAtlas(string path)
	{
		GameObject gameObject = Resources.Load(path) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("Can't find atlas " + path);
		}
		else
		{
			UIAtlas component = gameObject.GetComponent<UIAtlas>();
			if (!(component == null))
			{
				return component;
			}
			Debug.Log(path + "is not atlas");
		}
		return null;
	}

	private UIFont LoadFont(string path)
	{
		GameObject gameObject = Resources.Load(path) as GameObject;
		if (gameObject == null)
		{
			Debug.Log("Can't find font " + path);
		}
		else
		{
			UIFont component = gameObject.GetComponent<UIFont>();
			if (!(component == null))
			{
				return component;
			}
			Debug.Log(path + "is not font");
		}
		return null;
	}
}
