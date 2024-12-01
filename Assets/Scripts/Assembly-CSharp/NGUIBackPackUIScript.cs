using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUIBackPackUIScript : UIGameMenuNormal, UIMsgListener
{
	public const int UNLOCK_FIRST_WEAPON_SLOT_GOLD = 15000;

	public const int UNLOCK_SECOND_WEAPON_SLOT_MITHRIL = 1000;

	public const int UNLOCK_FIRST_CHIP_SLOT_GOLD = 15000;

	public const int UNLOCK_SECOND_CHIP_SLOT_MITHRIL = 200;

	public const int UNLOCK_THIRD_CHIP_SLOT_MITHRIL = 1000;

	public static NGUIBackPackUIScript mInstance;

	public NGUICamera mNGUICamera;

	public NGUIItemEquipmentSlot mWeapon1Slot;

	public NGUIItemEquipmentSlot mWeapon2Slot;

	public NGUIItemEquipmentSlot mWeapon3Slot;

	public NGUIItemEquipmentSlot mWeapon4Slot;

	public NGUIItemEquipmentSlot mGrenadeSlot;

	public NGUIItemEquipmentSlot mShieldSlot;

	public NGUIItemEquipmentSlot mSlot1Slot;

	public NGUIItemEquipmentSlot mSlot2Slot;

	public NGUIItemEquipmentSlot mSlot3Slot;

	public NGUIItemEquipmentSlot mSlot4Slot;

	public NGUIStorage mBackPack;

	public GameObject ExtendBag;

	public GameObject ExtendBag2;

	public GameObject m2DUIObject;

	public GameObject mOtherObject;

	public GameObject Avatar;

	//public GameObject FreeMithrilButton;

	private float mLastUpdateAimItemTime;

	private ItemInfo itemInfo;

	public GameObject m_Character;

	private string mUnlockingSlotName = string.Empty;

	public NGUIUnlockEquipSlotPriceScript Weapon3SlotPrice;

	public NGUIUnlockEquipSlotPriceScript Weapon4SlotPrice;

	public NGUIUnlockEquipSlotPriceScript Chip2SlotPrice;

	public NGUIUnlockEquipSlotPriceScript Chip3SlotPrice;

	public NGUIUnlockEquipSlotPriceScript Chip4SlotPrice;

	public GameObject DescriptionLeftPoint;

	public GameObject DescriptionRightPoint;

	private bool mIsBagLocked;

	private string mExtendBagName;

	private string mExtendBagName2;

	public bool IsBackPackOpen { get; set; }

	public bool IsBagLocked
	{
		get
		{
			return mIsBagLocked;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		mInstance = this;
		itemInfo = GameApp.GetInstance().GetUserState().ItemInfoData;
		InitBackPack();
		IsBackPackOpen = true;
		if (GameApp.GetInstance().IsConnectedToInternet())
		{
			StartCoroutine(SetAds());
		}
		if (itemInfo.Bag_Extend_Time < 2)
		{
			if (ExtendBag != null && 2 - itemInfo.Bag_Extend_Time >= 2)
			{
				AddDelegate(ExtendBag, out mExtendBagName);
			}
			if (ExtendBag2 != null && 2 - itemInfo.Bag_Extend_Time >= 1)
			{
				AddDelegate(ExtendBag2, out mExtendBagName2);
			}
		}
	}

	private IEnumerator SetAds()
	{
		WWW getAppStatusWWW = new WWW("http://174.36.196.91:8088/AdvertServer/GetAppStatus?appcode=ag003&v=" + GlobalState.version);
		while (!getAppStatusWWW.isDone)
		{
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			byte[] appStatusBytes = getAppStatusWWW.bytes;
			BytesBuffer bb = new BytesBuffer(appStatusBytes);
			AdsManager.GetInstance().DownLoadAdsStatus(bb);
		}
		catch (Exception ex2)
		{
			Debug.Log(ex2.Message);
		}
		getAppStatusWWW = new WWW("http://174.36.196.91:8088/AdvertServer/GetAdvertControl?appcode=ig003&v=" + GlobalState.version + "&adcodes=ad001,ad002,ad003");
		while (!getAppStatusWWW.isDone)
		{
			yield return new WaitForSeconds(0.5f);
		}
		try
		{
			byte[] appStatusBytes2 = getAppStatusWWW.bytes;
			BytesBuffer bb2 = new BytesBuffer(appStatusBytes2);
			AdsManager.GetInstance().DownLoadAdCode(bb2);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	private void Start()
	{
		mLastUpdateAimItemTime = Time.time;
	}

	protected override void OnDestroy()
	{
		if (NGUIItemSlot.mDragingSlot != null)
		{
			NGUIItemSlot.mDragingSlot.OnDropFail();
		}
		GameApp.GetInstance().Save();
		base.OnDestroy();
		if (m_Character != null)
		{
			UnityEngine.Object.Destroy(m_Character);
			m_Character = null;
		}
		mInstance = null;
	}

	public void ShowBackPack()
	{
		Debug.Log("ShowBackPack");
		NGUITools.SetActive(m2DUIObject, true);
		InitBackPack();
		IsBackPackOpen = true;
	}

	public void HideBackPack()
	{
		NGUITools.SetActive(m2DUIObject, false);
		if (m_Character != null)
		{
			UnityEngine.Object.Destroy(m_Character);
			m_Character = null;
		}
		IsBackPackOpen = false;
	}

	private void InitBackPack()
	{
		mWeapon1Slot.SlotEnabled = true;
		mWeapon1Slot.SetItem(itemInfo.Weapon1);
		mWeapon2Slot.SlotEnabled = true;
		mWeapon2Slot.SetItem(itemInfo.Weapon2);
		UpdateUnlockWeaponPrice();
		UpdateUnlockChipPrice();
		if (itemInfo.IsWeapon3Enable)
		{
			mWeapon3Slot.SlotEnabled = true;
			mWeapon3Slot.SetItem(itemInfo.Weapon3);
			RemoveLock(mWeapon3Slot);
			Weapon3SlotPrice.gameObject.SetActive(false);
		}
		if (itemInfo.IsWeapon4Enable)
		{
			mWeapon4Slot.SlotEnabled = true;
			mWeapon4Slot.SetItem(itemInfo.Weapon4);
			RemoveLock(mWeapon4Slot);
			Weapon4SlotPrice.gameObject.SetActive(false);
		}
		mGrenadeSlot.SlotEnabled = true;
		mGrenadeSlot.SetItem(itemInfo.HandGrenade);
		mShieldSlot.SlotEnabled = true;
		mShieldSlot.SetItem(itemInfo.Shield);
		mSlot1Slot.SlotEnabled = true;
		mSlot1Slot.SetItem(itemInfo.Slot1);
		if (itemInfo.IsSlot2Enable)
		{
			mSlot2Slot.SlotEnabled = true;
			mSlot2Slot.SetItem(itemInfo.Slot2);
			RemoveLock(mSlot2Slot);
			Chip2SlotPrice.gameObject.SetActive(false);
		}
		if (itemInfo.IsSlot3Enable)
		{
			mSlot3Slot.SlotEnabled = true;
			mSlot3Slot.SetItem(itemInfo.Slot3);
			RemoveLock(mSlot3Slot);
			Chip3SlotPrice.gameObject.SetActive(false);
		}
		if (itemInfo.IsSlot4Enable)
		{
			mSlot4Slot.SlotEnabled = true;
			mSlot4Slot.SetItem(itemInfo.Slot4);
			RemoveLock(mSlot4Slot);
			Chip4SlotPrice.gameObject.SetActive(false);
		}
		mBackPack.ClearItem();
		mBackPack.maxItemCount = itemInfo.BackpackSlotCount;
		List<NGUIGameItem> items = mBackPack.items;
		for (int i = 0; i < itemInfo.BackpackSlotCount; i++)
		{
			items[i] = itemInfo.BackPackItems[i];
		}
		if (itemInfo.Bag_Extend_Time >= 1)
		{
			ExtendBag.SetActive(false);
		}
		if (itemInfo.Bag_Extend_Time >= 2)
		{
			ExtendBag2.SetActive(false);
		}
		CreateAvatar();
	}

	public void CreateAvatar()
	{
		if (m_Character != null)
		{
			UnityEngine.Object.Destroy(m_Character);
		}
		m_Character = AvatarBuilder.GetInstance().CreateUIAvatarWithWeapon(GameApp.GetInstance().GetUserState().GetRoleState(), Avatar, "UI", true);
	}

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (num != 0f && m_Character != null)
		{
			GameObject gameObject = m_Character.transform.Find("Entity").gameObject;
			AnimationState animationState = gameObject.GetComponent<Animation>()[GameApp.GetInstance().GetUserState().GetRoleState()
				.GetUIIdleAnimation()];
			float num2 = animationState.speed * num;
			animationState.time += num2;
			if (animationState.time > animationState.length)
			{
				animationState.time = 0f;
			}
		}
	}

	private void RemoveLock(NGUIItemEquipmentSlot slot)
	{
		Transform transform = slot.transform.Find("Lock");
		if (transform != null)
		{
			NGUITools.SetActive(transform.gameObject, false);
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (ExtendBag != null && IsThisObject(go, mExtendBagName))
		{
			ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
			if (GetComponentInChildren<NGUIBagTweenScript>().IsButtonEnable() && itemInfoData.Bag_Extend_Time < 2)
			{
				SetBackPackBlockState(true);
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_EXTEND_BAG").Replace("%d", Global.BAG_EXTEND_MITHRIL[itemInfoData.Bag_Extend_Time].ToString()), 3, 36);
			}
		}
		else if (ExtendBag2 != null && IsThisObject(go, mExtendBagName2))
		{
			ItemInfo itemInfoData2 = GameApp.GetInstance().GetUserState().ItemInfoData;
			if (GetComponentInChildren<NGUIBagTweenScript>().IsButtonEnable() && itemInfoData2.Bag_Extend_Time < 2)
			{
				SetBackPackBlockState(true);
				UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_EXTEND_BAG").Replace("%d", Global.BAG_EXTEND_MITHRIL[itemInfoData2.Bag_Extend_Time].ToString()), 3, 36);
			}
		}
	}

	public void OnButtonClick(UIMsg whichMsg, UIMsg.UIMsgButton buttonId)
	{
		if (buttonId == UIMsg.UIMsgButton.Ok)
		{
			UserState userState = GameApp.GetInstance().GetUserState();
			GlobalState globalState = GameApp.GetInstance().GetGlobalState();
			switch (whichMsg.EventId)
			{
			case 9:
				UIMsgBox.instance.CloseMessage();
				UIIAP.Show(UIIAP.Type.IAP);
				SetBackPackBlockState(false);
				break;
			case 99:
				UIMsgBox.instance.CloseMessage();
				UIIAP.Show(UIIAP.Type.Exchange);
				SetBackPackBlockState(false);
				break;
			case 33:
				UIMsgBox.instance.CloseMessage();
				break;
			case 36:
				if (GameApp.GetInstance().GetGlobalState().BuyWithMithril(Global.BAG_EXTEND_MITHRIL[itemInfo.Bag_Extend_Time]))
				{
					SetBackPackBlockState(false);
					UIMsgBox.instance.CloseMessage();
					itemInfo.Bag_Extend_Time++;
					if (itemInfo.Bag_Extend_Time >= 1)
					{
						ExtendBag.SetActive(false);
					}
					if (itemInfo.Bag_Extend_Time >= 2)
					{
						ExtendBag2.SetActive(false);
					}
					itemInfo.BackpackSlotCount += 5;
					for (int i = 0; i < 5; i++)
					{
						itemInfo.BackPackItems.Add(null);
					}
					mBackPack.ClearItem();
					mBackPack.maxItemCount = itemInfo.BackpackSlotCount;
					mBackPack.Refresh();
					for (int j = 0; j < itemInfo.BackpackSlotCount; j++)
					{
						mBackPack.items[j] = itemInfo.BackPackItems[j];
					}
				}
				else
				{
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
				break;
			case 21:
				UIMsgBox.instance.CloseMessage();
				if (userState.GetCash() >= 15000)
				{
					userState.Buy(15000);
					if (mUnlockingSlotName == mWeapon3Slot.name)
					{
						itemInfo.IsWeapon3Enable = true;
						mWeapon3Slot.SlotEnabled = true;
						RemoveLock(mWeapon3Slot);
						Weapon3SlotPrice.gameObject.SetActive(false);
					}
					else if (mUnlockingSlotName == mWeapon4Slot.name)
					{
						itemInfo.IsWeapon4Enable = true;
						mWeapon4Slot.SlotEnabled = true;
						RemoveLock(mWeapon4Slot);
						Weapon4SlotPrice.gameObject.SetActive(false);
					}
					SetBackPackBlockState(false);
					UpdateUnlockWeaponPrice();
				}
				else
				{
					SetBackPackBlockState(true);
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GOLD_NOT_ENOUGH"), 2, 99);
				}
				break;
			case 22:
				UIMsgBox.instance.CloseMessage();
				if (globalState.BuyWithMithril(1000))
				{
					if (mUnlockingSlotName == mWeapon3Slot.name)
					{
						itemInfo.IsWeapon3Enable = true;
						mWeapon3Slot.SlotEnabled = true;
						RemoveLock(mWeapon3Slot);
						Weapon3SlotPrice.gameObject.SetActive(false);
					}
					else if (mUnlockingSlotName == mWeapon4Slot.name)
					{
						itemInfo.IsWeapon4Enable = true;
						mWeapon4Slot.SlotEnabled = true;
						RemoveLock(mWeapon4Slot);
						Weapon4SlotPrice.gameObject.SetActive(false);
					}
					SetBackPackBlockState(false);
					UpdateUnlockWeaponPrice();
				}
				else
				{
					SetBackPackBlockState(true);
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
				break;
			case 23:
				UIMsgBox.instance.CloseMessage();
				if (userState.GetCash() >= 15000)
				{
					userState.Buy(15000);
					if (mUnlockingSlotName == mSlot2Slot.name)
					{
						itemInfo.IsSlot2Enable = true;
						mSlot2Slot.SlotEnabled = true;
						RemoveLock(mSlot2Slot);
						Chip2SlotPrice.gameObject.SetActive(false);
					}
					else if (mUnlockingSlotName == mSlot3Slot.name)
					{
						itemInfo.IsSlot3Enable = true;
						mSlot3Slot.SlotEnabled = true;
						RemoveLock(mSlot3Slot);
						Chip3SlotPrice.gameObject.SetActive(false);
					}
					else if (mUnlockingSlotName == mSlot4Slot.name)
					{
						itemInfo.IsSlot4Enable = true;
						mSlot4Slot.SlotEnabled = true;
						RemoveLock(mSlot4Slot);
						Chip4SlotPrice.gameObject.SetActive(false);
					}
					SetBackPackBlockState(false);
					UpdateUnlockChipPrice();
				}
				else
				{
					SetBackPackBlockState(true);
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_GOLD_NOT_ENOUGH"), 2, 99);
				}
				break;
			case 24:
				UIMsgBox.instance.CloseMessage();
				if (globalState.BuyWithMithril(200))
				{
					if (mUnlockingSlotName == mSlot2Slot.name)
					{
						itemInfo.IsSlot2Enable = true;
						mSlot2Slot.SlotEnabled = true;
						RemoveLock(mSlot2Slot);
						Chip2SlotPrice.gameObject.SetActive(false);
					}
					else if (mUnlockingSlotName == mSlot3Slot.name)
					{
						itemInfo.IsSlot3Enable = true;
						mSlot3Slot.SlotEnabled = true;
						RemoveLock(mSlot3Slot);
						Chip3SlotPrice.gameObject.SetActive(false);
					}
					else if (mUnlockingSlotName == mSlot4Slot.name)
					{
						itemInfo.IsSlot4Enable = true;
						mSlot4Slot.SlotEnabled = true;
						RemoveLock(mSlot4Slot);
						Chip4SlotPrice.gameObject.SetActive(false);
					}
					SetBackPackBlockState(false);
					UpdateUnlockChipPrice();
				}
				else
				{
					SetBackPackBlockState(true);
					UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_MITHRIL_NOT_ENOUGH"), 2, 9);
				}
				break;
			case 25:
				UIMsgBox.instance.CloseMessage();
				if (globalState.BuyWithMithril(1000))
				{
					if (mUnlockingSlotName == mSlot2Slot.name)
					{
						itemInfo.IsSlot2Enable = true;
						mSlot2Slot.SlotEnabled = true;
						RemoveLock(mSlot2Slot);
						Chip2SlotPrice.gameObject.SetActive(false);
					}
					else if (mUnlockingSlotName == mSlot3Slot.name)
					{
						itemInfo.IsSlot3Enable = true;
						mSlot3Slot.SlotEnabled = true;
						RemoveLock(mSlot3Slot);
						Chip3SlotPrice.gameObject.SetActive(false);
					}
					else if (mUnlockingSlotName == mSlot4Slot.name)
					{
						itemInfo.IsSlot4Enable = true;
						mSlot4Slot.SlotEnabled = true;
						RemoveLock(mSlot4Slot);
						Chip4SlotPrice.gameObject.SetActive(false);
					}
					SetBackPackBlockState(false);
					UpdateUnlockChipPrice();
				}
				break;
			}
		}
		else
		{
			UIMsgBox.instance.CloseMessage();
			SetBackPackBlockState(false);
		}
		mUnlockingSlotName = string.Empty;
	}

	private void UpdateUnlockWeaponPrice()
	{
		switch (UnlockedWeaponSlotCount())
		{
		case 0:
			Weapon3SlotPrice.CurrencyIcon.spriteName = "gold2";
			Weapon3SlotPrice.Price.text = 15000.ToString();
			Weapon4SlotPrice.CurrencyIcon.spriteName = "gold2";
			Weapon4SlotPrice.Price.text = 15000.ToString();
			break;
		case 1:
			Weapon3SlotPrice.CurrencyIcon.spriteName = "mithril";
			Weapon3SlotPrice.Price.text = 1000.ToString();
			Weapon4SlotPrice.CurrencyIcon.spriteName = "mithril";
			Weapon4SlotPrice.Price.text = 1000.ToString();
			break;
		}
	}

	private void UpdateUnlockChipPrice()
	{
		switch (UnlockedChipSlotCount())
		{
		case 0:
			Chip2SlotPrice.CurrencyIcon.spriteName = "gold2";
			Chip2SlotPrice.Price.text = 15000.ToString();
			Chip3SlotPrice.CurrencyIcon.spriteName = "gold2";
			Chip3SlotPrice.Price.text = 15000.ToString();
			Chip4SlotPrice.CurrencyIcon.spriteName = "gold2";
			Chip4SlotPrice.Price.text = 15000.ToString();
			break;
		case 1:
			Chip2SlotPrice.CurrencyIcon.spriteName = "mithril";
			Chip2SlotPrice.Price.text = 200.ToString();
			Chip3SlotPrice.CurrencyIcon.spriteName = "mithril";
			Chip3SlotPrice.Price.text = 200.ToString();
			Chip4SlotPrice.CurrencyIcon.spriteName = "mithril";
			Chip4SlotPrice.Price.text = 200.ToString();
			break;
		case 2:
			Chip2SlotPrice.CurrencyIcon.spriteName = "mithril";
			Chip2SlotPrice.Price.text = 1000.ToString();
			Chip3SlotPrice.CurrencyIcon.spriteName = "mithril";
			Chip3SlotPrice.Price.text = 1000.ToString();
			Chip4SlotPrice.CurrencyIcon.spriteName = "mithril";
			Chip4SlotPrice.Price.text = 1000.ToString();
			break;
		}
	}

	private void UnlockFirstWeaponSlot()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_GOLD_CONFIRMATION").Replace("%d", 15000.ToString()), 3, 21);
	}

	private void UnlockSecondWeaponSlot()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_MITHRIL_CONFIRMATION").Replace("%d", 1000.ToString()), 3, 22);
	}

	private void UnlockFirstChipSlot()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_GOLD_CONFIRMATION").Replace("%d", 15000.ToString()), 3, 23);
	}

	private void UnlockSecondChipSlot()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_MITHRIL_CONFIRMATION").Replace("%d", 200.ToString()), 3, 24);
	}

	private void UnlockThirdChipSlot()
	{
		UIMsgBox.instance.ShowMessage(this, LocalizationManager.GetInstance().GetString("MSG_USE_MITHRIL_CONFIRMATION").Replace("%d", 1000.ToString()), 3, 25);
	}

	public void UnlockSlot(GameObject gameObject)
	{
		mUnlockingSlotName = gameObject.name;
		if (!itemInfo.IsWeapon3Enable && gameObject.name.Equals(mWeapon3Slot.name))
		{
			UnlockWeaponSlot();
		}
		else if (!itemInfo.IsWeapon4Enable && gameObject.name.Equals(mWeapon4Slot.name))
		{
			UnlockWeaponSlot();
		}
		else if (!itemInfo.IsSlot2Enable && gameObject.name.Equals(mSlot2Slot.name))
		{
			UnlockChipSlot();
		}
		else if (!itemInfo.IsSlot3Enable && gameObject.name.Equals(mSlot3Slot.name))
		{
			UnlockChipSlot();
		}
		else if (!itemInfo.IsSlot4Enable && gameObject.name.Equals(mSlot4Slot.name))
		{
			UnlockChipSlot();
		}
	}

	public void SetBackPackLockState(bool locked)
	{
		if (locked)
		{
			mIsBagLocked = true;
			mWeapon1Slot.GetComponent<Collider>().enabled = false;
			mWeapon2Slot.GetComponent<Collider>().enabled = false;
			mWeapon3Slot.GetComponent<Collider>().enabled = false;
			mWeapon4Slot.GetComponent<Collider>().enabled = false;
			mGrenadeSlot.GetComponent<Collider>().enabled = false;
			mSlot1Slot.GetComponent<Collider>().enabled = false;
			mSlot2Slot.GetComponent<Collider>().enabled = false;
			mSlot3Slot.GetComponent<Collider>().enabled = false;
			mSlot4Slot.GetComponent<Collider>().enabled = false;
			//FreeMithrilButton.GetComponent<Collider>().enabled = false;
			ExtendBag.GetComponent<Collider>().enabled = false;
			ExtendBag2.GetComponent<Collider>().enabled = false;
		}
		else
		{
			mIsBagLocked = false;
			mWeapon1Slot.GetComponent<Collider>().enabled = true;
			mWeapon2Slot.GetComponent<Collider>().enabled = true;
			mWeapon3Slot.GetComponent<Collider>().enabled = true;
			mWeapon4Slot.GetComponent<Collider>().enabled = true;
			mGrenadeSlot.GetComponent<Collider>().enabled = true;
			mSlot1Slot.GetComponent<Collider>().enabled = true;
			mSlot2Slot.GetComponent<Collider>().enabled = true;
			mSlot3Slot.GetComponent<Collider>().enabled = true;
			mSlot4Slot.GetComponent<Collider>().enabled = true;
			//FreeMithrilButton.GetComponent<Collider>().enabled = true;
			ExtendBag.GetComponent<Collider>().enabled = true;
			ExtendBag2.GetComponent<Collider>().enabled = true;
		}
	}

	public void SetBackPackBlockState(bool locked)
	{
		if (locked)
		{
			mIsBagLocked = true;
			mWeapon1Slot.GetComponent<Collider>().enabled = false;
			mWeapon2Slot.GetComponent<Collider>().enabled = false;
			mWeapon3Slot.GetComponent<Collider>().enabled = false;
			mWeapon4Slot.GetComponent<Collider>().enabled = false;
			mShieldSlot.GetComponent<Collider>().enabled = false;
			mGrenadeSlot.GetComponent<Collider>().enabled = false;
			mSlot1Slot.GetComponent<Collider>().enabled = false;
			mSlot2Slot.GetComponent<Collider>().enabled = false;
			mSlot3Slot.GetComponent<Collider>().enabled = false;
			mSlot4Slot.GetComponent<Collider>().enabled = false;
			//FreeMithrilButton.GetComponent<Collider>().enabled = false;
			Collider[] componentsInChildren = mBackPack.transform.GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren)
			{
				collider.enabled = false;
			}
			ExtendBag.GetComponent<Collider>().enabled = false;
			ExtendBag2.GetComponent<Collider>().enabled = false;
		}
		else
		{
			mIsBagLocked = false;
			mWeapon1Slot.GetComponent<Collider>().enabled = true;
			mWeapon2Slot.GetComponent<Collider>().enabled = true;
			mWeapon3Slot.GetComponent<Collider>().enabled = true;
			mWeapon4Slot.GetComponent<Collider>().enabled = true;
			mShieldSlot.GetComponent<Collider>().enabled = true;
			mGrenadeSlot.GetComponent<Collider>().enabled = true;
			mSlot1Slot.GetComponent<Collider>().enabled = true;
			mSlot2Slot.GetComponent<Collider>().enabled = true;
			mSlot3Slot.GetComponent<Collider>().enabled = true;
			mSlot4Slot.GetComponent<Collider>().enabled = true;
			//FreeMithrilButton.GetComponent<Collider>().enabled = true;
			Collider[] componentsInChildren2 = mBackPack.transform.GetComponentsInChildren<Collider>();
			foreach (Collider collider2 in componentsInChildren2)
			{
				collider2.enabled = true;
			}
			ExtendBag.GetComponent<Collider>().enabled = true;
			ExtendBag2.GetComponent<Collider>().enabled = true;
		}
	}

	private int UnlockedWeaponSlotCount()
	{
		int result = 0;
		if (!itemInfo.IsWeapon3Enable && !itemInfo.IsWeapon4Enable)
		{
			result = 0;
		}
		else if ((itemInfo.IsWeapon3Enable && !itemInfo.IsWeapon4Enable) || (!itemInfo.IsWeapon3Enable && itemInfo.IsWeapon4Enable))
		{
			result = 1;
		}
		else if (itemInfo.IsWeapon3Enable && itemInfo.IsWeapon4Enable)
		{
			result = 2;
		}
		return result;
	}

	private int UnlockedChipSlotCount()
	{
		int num = 0;
		if (!itemInfo.IsSlot2Enable && !itemInfo.IsSlot3Enable && !itemInfo.IsSlot4Enable)
		{
			return 0;
		}
		if ((!itemInfo.IsSlot2Enable && !itemInfo.IsSlot3Enable && itemInfo.IsSlot4Enable) || (!itemInfo.IsSlot2Enable && itemInfo.IsSlot3Enable && !itemInfo.IsSlot4Enable) || (itemInfo.IsSlot2Enable && !itemInfo.IsSlot3Enable && !itemInfo.IsSlot4Enable))
		{
			return 1;
		}
		if ((!itemInfo.IsSlot2Enable && itemInfo.IsSlot3Enable && itemInfo.IsSlot4Enable) || (itemInfo.IsSlot2Enable && !itemInfo.IsSlot3Enable && itemInfo.IsSlot4Enable) || (itemInfo.IsSlot2Enable && itemInfo.IsSlot3Enable && !itemInfo.IsSlot4Enable))
		{
			return 2;
		}
		return 3;
	}

	private void UnlockWeaponSlot()
	{
		if (!itemInfo.IsWeapon3Enable && !itemInfo.IsWeapon4Enable)
		{
			UnlockFirstWeaponSlot();
		}
		else
		{
			UnlockSecondWeaponSlot();
		}
	}

	private void UnlockChipSlot()
	{
		switch (UnlockedChipSlotCount())
		{
		case 0:
			UnlockFirstChipSlot();
			break;
		case 1:
			UnlockSecondChipSlot();
			break;
		case 2:
			UnlockThirdChipSlot();
			break;
		}
	}

	private void OnClick()
	{
		if (ItemDescriptionScript.mInstance != null && ItemDescriptionScript.mInstance.gameObject.activeSelf)
		{
			ItemDescriptionScript.mInstance.SetObserveItem(null);
		}
	}

	private void OnDrop(GameObject go)
	{
		if (NGUIItemSlot.mDragingSlot != null)
		{
			NGUIItemSlot.mDragingSlot.OnDropFail();
		}
	}
}
