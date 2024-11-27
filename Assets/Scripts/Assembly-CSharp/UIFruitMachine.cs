using System.Collections.Generic;
using UnityEngine;

public class UIFruitMachine : UIGameMenu
{
	public const string KEY_ITEM_INDEX = "ItemIndex";

	public const string KEY_ITEM = "Item";

	public static FruitMachineNotWork STATE_NOT_WORK = new FruitMachineNotWork();

	public static FruitMachineWorking STATE_WORKING = new FruitMachineWorking();

	public static FruitMachineWaitAfterWork STATE_WAIT_AFTER_WORK = new FruitMachineWaitAfterWork();

	public static FruitMachineGetItem STATE_GET_ITEM = new FruitMachineGetItem();

	public static RateState STATE_RATE = new RateState();

	public FruitMachineBar fruitMachineBar;

	public FruitMachineNeonLightConsole fruitMachineNeonLightConsole;

	public GameObject fruitMachineBonusContainer;

	public GameObject fruitMachineItemLightContainer;

	public GameObject fruitMachineItemGuidingLightContainer;

	public GameObject fruitMachineEsc;

	public GameObject fruitMachineReset;

	public GameObject fruitMachineSkip;

	public GameObject fruitMachineCongratulations;

	public UILabel costLabel;

	public UILabel mithrilLabel;

	public UILabel goldLabel;

	public UISprite[] costIcon;

	public UILabel resetCostLabel;

	private FruitMachineBonus[] mFruitMachineBonusList;

	private FruitMachineLight[] mFruitMachineItemLightList;

	private FruitMachineLight[] mFruitMachineItemGuidingLightList;

	private FruitMachineState mCurrentState;

	private GambleConfig mGambleConfig;

	public GambleType gambleType;

	private static GambleType staticGambleType;

	private static byte prevPhase;

	private static bool isShow = false;

	public static void Show(GambleType gambleType)
	{
		if (!isShow)
		{
			isShow = true;
			staticGambleType = gambleType;
			prevPhase = 6;
			GameApp.GetInstance().GetUIStateManager().FrGoToPhase(34, false, false, true);
			GambelManager.GetInstance().PauseTimer();
		}
	}

	public static bool IsShow()
	{
		return isShow;
	}

	public static void Close()
	{
		if (!isShow)
		{
			return;
		}
		isShow = false;
		if (GameApp.GetInstance().GetUserState().m_questStateContainer.QuestItemsAvailable(10502))
		{
			GameApp.GetInstance().GetUserState().ItemInfoData.AddStoryItem(10502);
			GameApp.GetInstance().GetUserState().m_questStateContainer.OnQuestProgressItemCollection(10502);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				PickUpQuestItemRequest request = new PickUpQuestItemRequest(10502);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(prevPhase, false, false, true);
		GambelManager.GetInstance().ResumeTimer();
	}

	protected override byte InitMask()
	{
		return 0;
	}

	protected override void Awake()
	{
		base.Awake();
		gambleType = staticGambleType;
		fruitMachineCongratulations.SetActive(false);
		mFruitMachineBonusList = fruitMachineBonusContainer.GetComponentsInChildren<FruitMachineBonus>(true);
		mFruitMachineItemLightList = fruitMachineItemLightContainer.GetComponentsInChildren<FruitMachineLight>(true);
		mFruitMachineItemGuidingLightList = fruitMachineItemGuidingLightContainer.GetComponentsInChildren<FruitMachineLight>(true);
		SetMenuCloseOnDestroy(true);
		mGambleConfig = GambelManager.GetInstance().GetGambleConfig(gambleType);
		FruitMachineBonus[] array = mFruitMachineBonusList;
		foreach (FruitMachineBonus fruitMachineBonus in array)
		{
			fruitMachineBonus.SetFruitMachineConfig((FruitMachineConfig)mGambleConfig);
		}
		if (gambleType == GambleType.GoldFruitMachine)
		{
			UISprite[] array2 = costIcon;
			foreach (UISprite uISprite in array2)
			{
				uISprite.spriteName = "gold5";
			}
			mFruitMachineBonusList[2].gameObject.SetActive(false);
			mFruitMachineBonusList[3].gameObject.SetActive(false);
			AddDelegate(mFruitMachineBonusList[0].gameObject);
			AddDelegate(mFruitMachineBonusList[1].gameObject);
		}
		else
		{
			UISprite[] array3 = costIcon;
			foreach (UISprite uISprite2 in array3)
			{
				uISprite2.spriteName = "mithril5";
			}
			FruitMachineBonus[] array4 = mFruitMachineBonusList;
			foreach (FruitMachineBonus fruitMachineBonus2 in array4)
			{
				AddDelegate(fruitMachineBonus2.gameObject);
			}
		}
		UISprite[] array5 = costIcon;
		foreach (UISprite uISprite3 in array5)
		{
			uISprite3.MakePixelPerfect();
		}
		resetCostLabel.text = mGambleConfig.GetResetCost();
		RefreshItems();
		AddDelegate(fruitMachineBar.gameObject);
		AddDelegate(fruitMachineEsc);
		AddDelegate(fruitMachineReset);
		AddDelegate(fruitMachineSkip);
	}

	private void Start()
	{
		mCurrentState = STATE_NOT_WORK;
		FruitMachineBundle bundle = new FruitMachineBundle(this, null);
		mCurrentState.Start(bundle);
	}

	private void Update()
	{
		mCurrentState.Update();
		costLabel.text = mGambleConfig.GetCost();
		mithrilLabel.text = string.Empty + GameApp.GetInstance().GetGlobalState().GetMithril();
		goldLabel.text = string.Empty + GameApp.GetInstance().GetUserState().GetCash();
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		mCurrentState.Click(go);
	}

	protected override void OnDragThumb(GameObject go, Vector2 delta)
	{
		base.OnDragThumb(go, delta);
		mCurrentState.Drag(go, delta);
	}

	protected override void OnPressThumb(GameObject go, bool isPressed)
	{
		base.OnPressThumb(go, isPressed);
		mCurrentState.Press(go, isPressed);
	}

	public void RefreshItems()
	{
		RefreshItems(mGambleConfig.GetItemList());
	}

	private void RefreshItems(List<FruitMachineItemAbility> itemAbitlityList)
	{
		FruitMachineLight[] array = mFruitMachineItemLightList;
		foreach (FruitMachineLight fruitMachineLight in array)
		{
			ItemIcon componentInChildren = fruitMachineLight.gameObject.GetComponentInChildren<ItemIcon>();
			if (componentInChildren != null)
			{
				Object.Destroy(componentInChildren.gameObject);
			}
		}
		for (int j = 0; j < itemAbitlityList.Count; j++)
		{
			GameObject gameObject = null;
			gameObject = ((!itemAbitlityList[j].IsUnKnown) ? GameApp.GetInstance().GetLootManager().CreateIcon(itemAbitlityList[j].Quality, itemAbitlityList[j].SmallIconName) : GameApp.GetInstance().GetLootManager().CreateUnknownIcon());
			gameObject.transform.parent = mFruitMachineItemLightList[j].transform;
			gameObject.transform.localPosition = new Vector3(0f, 0f, -1f);
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		}
	}

	public void GoToNextState(FruitMachineState state)
	{
		GoToNextState(state, null);
	}

	public void GoToNextState(FruitMachineState state, FruitMachineIntent intent)
	{
		if (state != null)
		{
			mCurrentState.Exit();
			mCurrentState = state;
			FruitMachineBundle bundle = new FruitMachineBundle(this, intent);
			mCurrentState.Start(bundle);
		}
	}

	public FruitMachineLight[] GetFruitMachineItemLightList()
	{
		return mFruitMachineItemLightList;
	}

	public FruitMachineLight[] GetFruitMachineItemGuidingLightList()
	{
		return mFruitMachineItemGuidingLightList;
	}

	public void LightOutAllItemLight()
	{
		FruitMachineLight[] array = mFruitMachineItemLightList;
		foreach (FruitMachineLight fruitMachineLight in array)
		{
			fruitMachineLight.LightOut();
		}
		FruitMachineLight[] array2 = mFruitMachineItemGuidingLightList;
		foreach (FruitMachineLight fruitMachineLight2 in array2)
		{
			fruitMachineLight2.LightOut();
		}
	}

	public GambleConfig GetGambleConfig()
	{
		return mGambleConfig;
	}

	public FruitMachineBonus[] GetFruitMachineBonusList()
	{
		return mFruitMachineBonusList;
	}
}
