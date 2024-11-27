using System.Collections.Generic;
using UnityEngine;

public class FreyrScript : MonoBehaviour
{
	private const float WAIT_TIME = 2f;

	private string[][] atlasPath = new string[24][]
	{
		new string[2] { "Achievement/res/", "AchievementAtlas" },
		new string[2] { "Arena/res/", "ArenaAtlas" },
		new string[2] { "Avatar&Decoration/res/", "ADAtlas" },
		new string[2] { "Bag/Res/", "BagUIAtlas" },
		new string[2] { "Bubble/Res/", "BubbleUIAtlas" },
		new string[2] { "HUD/Res/HUD/", "HUDAtlas_M" },
		new string[2] { "HUD/Res/Item/", "ItemIconAtlas" },
		new string[2] { "HUD/Res/ItemPopMenu/", "ItemPopMenuAtlas" },
		new string[2] { "IAP/res/", "IAPAtlas" },
		new string[2] { "InGameMenu/Res/", "InGameMenuAtlas" },
		new string[2] { "MainMenuX/Res/Choose/", "ChooseUIAtlas" },
		new string[2] { "MainMenuX/Res/Create/", "CreateUIAtlas" },
		new string[2] { "MainMenuX/Res/Setting/", "SettingUIAtlas" },
		new string[2] { "MainMenuX/Res/Start/", "StartUIAtlas" },
		new string[2] { "MainMenuX/Res/Usual/", "UsualUIAtlas" },
		new string[2] { "Option/Res/", "OptionAtlas" },
		new string[2] { "Portal/UI/res/", "PortalAtlas" },
		new string[2] { "Quest/Res/", "QuestUIAtlas" },
		new string[2] { "ShopUI/Res/", "ShopUIAtlas" },
		new string[2] { "SkillTree/Res/Tree/", "SkillTreeUIAtlas" },
		new string[2] { "Team/Res/", "TeamUIAtlas" },
		new string[2] { "Tutorial/Res/", "TutorialAtlas" },
		new string[2] { "Gamble/Res/", "GambleAtlas" },
		new string[2] { "Ads/UI/Res/", "AdsAtlas" }
	};

	private string[][] fontPath = new string[3][]
	{
		new string[2] { "Font/Normal/", "RPG_Font" },
		new string[2] { "Font/Special/", "RPG_S_Font" },
		new string[2] { "IAP/res/", "IAP_Font" }
	};

	private List<UIAtlas> list = new List<UIAtlas>();

	private void Awake()
	{
		string text = string.Empty;
		switch (AndroidConstant.version)
		{
		case AndroidConstant.Version.GooglePlay:
			text = "en";
			break;
		case AndroidConstant.Version.Kindle:
			text = AndroidPluginScript.GetLanguage();
			break;
		case AndroidConstant.Version.MM:
		case AndroidConstant.Version.KindleCn:
			text = "zh-Hans";
			break;
		}
		if (text.Equals("zh-Hans"))
		{
			LocalizationManager.GetInstance().currentLanguage = "zh-Hans";
		}
		else if (text.Equals("zh-Hant"))
		{
			LocalizationManager.GetInstance().currentLanguage = "zh-Hant";
		}
		else if (text.Equals("fr"))
		{
			LocalizationManager.GetInstance().currentLanguage = "fr";
		}
		else if (text.Equals("ja"))
		{
			LocalizationManager.GetInstance().currentLanguage = "ja";
		}
		else
		{
			LocalizationManager.GetInstance().currentLanguage = "en";
		}
	}

	private void Start()
	{
		GameObject gameObject = GameObject.Find("MithrilRewards");
		if (gameObject == null)
		{
			GameObject original = Resources.Load("IAP/MithrilRewards") as GameObject;
			gameObject = Object.Instantiate(original) as GameObject;
			gameObject.name = "MithrilRewards";
		}
		Object.DontDestroyOnLoad(gameObject);
		GameObject gameObject2 = GameObject.Find("MsgUI");
		if (gameObject2 == null)
		{
			GameObject original2 = ResourceLoad.GetInstance().LoadUI("UIUtil", "MsgUI");
			gameObject2 = Object.Instantiate(original2) as GameObject;
			gameObject2.name = "MsgUI";
		}
		Object.DontDestroyOnLoad(gameObject2);
		GameObject gameObject3 = GameObject.Find("Loading/LoadingNetUI");
		if (gameObject3 == null)
		{
			GameObject original3 = Resources.Load("Loading/LoadingNetUI") as GameObject;
			gameObject3 = Object.Instantiate(original3) as GameObject;
			gameObject3.name = "NetUI";
		}
		Object.DontDestroyOnLoad(gameObject3);
		Application.LoadLevel("StartMenu");
	}

	public void Refresh(SystemLanguage language)
	{
		string text = null;
		if (language == SystemLanguage.Chinese)
		{
			text = "CN";
			LocalizationManager.GetInstance().currentLanguage = SystemLanguage.Chinese.ToString();
		}
		else
		{
			text = "EN";
			LocalizationManager.GetInstance().currentLanguage = SystemLanguage.English.ToString();
		}
		Debug.Log(text);
		string[][] array = atlasPath;
		foreach (string[] array2 in array)
		{
			UIAtlas uIAtlas = LoadAtlas(array2[0] + array2[1]);
			UIAtlas uIAtlas2 = LoadAtlas(array2[0] + text + "/" + text + "Atlas");
			if (uIAtlas != null && uIAtlas2 != null)
			{
				uIAtlas.replacement = uIAtlas2;
				uIAtlas.MarkAsDirty();
			}
		}
		string[][] array3 = fontPath;
		foreach (string[] array4 in array3)
		{
			UIFont uIFont = LoadFont(array4[0] + array4[1]);
			UIFont uIFont2 = LoadFont(array4[0] + text + "/" + text + "Font");
			if (uIFont != null && uIFont2 != null)
			{
				uIFont.replacement = uIFont2;
				uIFont.MarkAsDirty();
			}
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
