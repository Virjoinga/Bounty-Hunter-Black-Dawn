using System.Collections.Generic;
using UnityEngine;

public class ExtendBulletBunttonScript : MonoBehaviour, UIMsgListener
{
	public WeaponType BulletType;

	public List<UISprite> ExtendLevels;

	public UILabel MaxBullet;

	public UILabel Price;

	public UISprite MoneyIcon;

	public UISprite BulletIcon;

	public static int[] ExtendPrice = new int[10] { 500, 4000, 13500, 32000, 62500, 200, 300, 500, 1000, 2000 };

	private Vector3 BulletLabelOriginScale = default(Vector3);

	private void Start()
	{
		GameObject gameObject = BulletIcon.transform.parent.Find("Label").gameObject;
		BulletLabelOriginScale = gameObject.transform.localScale;
	}

	private void OnEnable()
	{
		Refresh();
	}

	public void Refresh()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		byte bulletLevelByWeaponType = userState.GetBulletLevelByWeaponType(BulletType);
		for (int i = 0; i < ExtendLevels.Count; i++)
		{
			if (ExtendLevels[i] != null)
			{
				if (i < bulletLevelByWeaponType)
				{
					ExtendLevels[i].gameObject.SetActive(true);
				}
				else
				{
					ExtendLevels[i].gameObject.SetActive(false);
				}
			}
		}
		MaxBullet.text = "+" + userState.GetBulletInMagsByWeaponType(BulletType);
		if (bulletLevelByWeaponType == userState.GetMaxBulletLevelByWeaponType(BulletType))
		{
			Price.text = "-----";
		}
		else if ((bulletLevelByWeaponType < 5 && ExtendPrice[bulletLevelByWeaponType] > userState.GetCash()) || (bulletLevelByWeaponType >= 5 && ExtendPrice[bulletLevelByWeaponType] > GameApp.GetInstance().GetGlobalState().GetMithril()))
		{
			Price.text = "[FF0000]" + ExtendPrice[bulletLevelByWeaponType] + "[-]";
		}
		else
		{
			Price.text = ExtendPrice[bulletLevelByWeaponType].ToString();
		}
		if (bulletLevelByWeaponType < 5)
		{
			MoneyIcon.spriteName = "GP";
		}
		else
		{
			MoneyIcon.spriteName = "thirl2";
		}
		MoneyIcon.MakePixelPerfect();
	}

	private void OnClick()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		byte bulletLevelByWeaponType = userState.GetBulletLevelByWeaponType(BulletType);
		if (userState.GetBulletLevelByWeaponType(BulletType) >= userState.GetMaxBulletLevelByWeaponType(BulletType))
		{
			return;
		}
		if (bulletLevelByWeaponType < 5)
		{
			if (!TutorialManager.GetInstance().IsTutorialOk(TutorialManager.TutorialType.ShopBulletExtend))
			{
				ExtendByGold();
			}
			else
			{
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_GOLD_CONFIRMATION").Replace("%d", string.Empty + ExtendPrice[bulletLevelByWeaponType]), 3, 27);
			}
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_MITHRIL_CONFIRMATION").Replace("%d", string.Empty + ExtendPrice[bulletLevelByWeaponType]), 3, 28);
		}
	}

	private void ExtendByGold()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		byte bulletLevelByWeaponType = userState.GetBulletLevelByWeaponType(BulletType);
		if (userState.GetCash() >= ExtendPrice[bulletLevelByWeaponType])
		{
			userState.OperInfo.AddInfo((OperatingInfoType)(26 + BulletType - 1), 1);
			SetLabelUpdateDisable();
			AddMaxBulletByGold();
			ExtendEffect("SetLabelUpdateEnable");
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GOLD_NOT_ENOUGH"), 2, 99);
		}
	}

	private void ExtendByMithril()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		byte bulletLevelByWeaponType = userState.GetBulletLevelByWeaponType(BulletType);
		if (GameApp.GetInstance().GetGlobalState().GetMithril() >= ExtendPrice[bulletLevelByWeaponType])
		{
			userState.OperInfo.AddInfo((OperatingInfoType)(26 + BulletType - 1), 1);
			SetLabelUpdateDisable();
			AddMaxBulletByMithril();
			ExtendEffect("SetLabelUpdateEnable");
		}
		else
		{
			UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		byte bulletLevelByWeaponType = userState.GetBulletLevelByWeaponType(BulletType);
		if (whichMsg.EventId == 27)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				if (userState.GetCash() >= ExtendPrice[bulletLevelByWeaponType])
				{
					UIMsgBox.instance.CloseMessage();
					ExtendEffect("AddMaxBulletByGold");
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GOLD_NOT_ENOUGH"), 2, 99);
				}
			}
			else if (!ShopUIScript.mInstance.IsInBulletTutorial)
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else if (whichMsg.EventId == 28)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				if (GameApp.GetInstance().GetGlobalState().GetMithril() >= ExtendPrice[bulletLevelByWeaponType])
				{
					UIMsgBox.instance.CloseMessage();
					ExtendEffect("AddMaxBulletByMithril");
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
			}
			else
			{
				UIMsgBox.instance.CloseMessage();
			}
		}
		else if (whichMsg.EventId == 9)
		{
			if (buttonId == UIMsg.UIMsgButton.Ok)
			{
				UIMsgBox.instance.CloseMessage();
				UIIAP.Show(UIIAP.Type.IAP);
			}
		}
		else if (whichMsg.EventId == 99 && buttonId == UIMsg.UIMsgButton.Ok)
		{
			UIMsgBox.instance.CloseMessage();
			UIIAP.Show(UIIAP.Type.Exchange);
		}
	}

	private void ExtendEffect(string addMaxBulletFunctionName)
	{
		if (BulletIcon != null)
		{
			float num = 1.5f;
			GameObject gameObject = Object.Instantiate(BulletIcon.gameObject) as GameObject;
			gameObject.transform.parent = BulletIcon.transform.parent;
			gameObject.transform.position = BulletIcon.transform.position;
			gameObject.transform.localScale = BulletIcon.transform.localScale * 7f;
			TweenAlpha.Begin(gameObject, num, 0f);
			TweenScale tweenScale = gameObject.AddComponent<TweenScale>();
			tweenScale.animationCurve.AddKey(0.15f, 0.5f);
			tweenScale.from = gameObject.transform.localScale;
			tweenScale.to = BulletIcon.transform.localScale;
			tweenScale.eventReceiver = base.gameObject;
			tweenScale.callWhenFinished = addMaxBulletFunctionName;
			GameObject gameObject2 = gameObject.transform.parent.Find("Label").gameObject;
			gameObject2.transform.localScale = BulletLabelOriginScale;
			TweenScale tweenScale2 = TweenScale.Begin(gameObject2, num / 2f, gameObject2.transform.localScale * 1.5f);
			tweenScale2.style = UITweener.Style.Once;
			tweenScale2.delay = num / 1.5f;
			while (tweenScale2.animationCurve.keys.Length > 0)
			{
				tweenScale2.animationCurve.RemoveKey(0);
			}
			tweenScale2.animationCurve.AddKey(0f, 0f);
			tweenScale2.animationCurve.AddKey(0.5f, 1f);
			tweenScale2.animationCurve.AddKey(1f, 0f);
		}
	}

	private void AddMaxBulletByGold()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		byte bulletLevelByWeaponType = userState.GetBulletLevelByWeaponType(BulletType);
		userState.AddBulletLevelByWeaponType(BulletType, userState.GetBulletInMagsByWeaponType(BulletType));
		userState.Buy(ExtendPrice[bulletLevelByWeaponType]);
		Refresh();
	}

	private void AddMaxBulletByMithril()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		byte bulletLevelByWeaponType = userState.GetBulletLevelByWeaponType(BulletType);
		GameApp.GetInstance().GetGlobalState().BuyWithMithril(ExtendPrice[bulletLevelByWeaponType]);
		userState.AddBulletLevelByWeaponType(BulletType, userState.GetBulletInMagsByWeaponType(BulletType));
		Refresh();
	}

	private void SetLabelUpdateDisable()
	{
		BulletStateScript component = BulletIcon.transform.parent.GetComponent<BulletStateScript>();
		component.SetUpdateEnable(false);
	}

	private void SetLabelUpdateEnable()
	{
		BulletStateScript component = BulletIcon.transform.parent.GetComponent<BulletStateScript>();
		component.SetUpdateEnable(true);
	}
}
