using UnityEngine;

public abstract class NGUIItemSlot : MonoBehaviour
{
	public UISprite icon;

	public UISprite backgroundIcon;

	public UILabel label;

	public AudioClip grabSound;

	public AudioClip placeSound;

	public AudioClip errorSound;

	public ItemDescriptionScript Description;

	protected NGUIGameItem mItem;

	private string mText = string.Empty;

	private static NGUIGameItem mDraggedItem;

	public static NGUIItemSlot mSelectedSlot;

	public static NGUIItemSlot mDragingSlot;

	protected bool needUpdate = true;

	protected GameObject QualityEffectObject;

	protected abstract NGUIGameItem observedItem { get; }

	protected abstract NGUIGameItem Replace(NGUIGameItem item);

	protected virtual void OnClick()
	{
		if (Description != null)
		{
			if (observedItem != null)
			{
				SetDescriptionPosition();
				Description.SetObserveItem(observedItem.baseItem);
			}
			else
			{
				Description.SetObserveItem(null);
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (mDraggedItem != null || mItem == null)
		{
			return;
		}
		if (mSelectedSlot != null)
		{
			UIFrameManager.GetInstance().DeleteFrame(mSelectedSlot.gameObject);
		}
		mSelectedSlot = this;
		SetDescriptionPosition();
		Description.SetObserveItem(null);
		mDragingSlot = this;
		NGUICamera.currentTouch.clickNotification = NGUICamera.ClickNotification.BasedOnDelta;
		mDraggedItem = Replace(null);
		NGUITools.PlaySound(grabSound);
		UpdateCursor();
		if (NGUIBackPackUIScript.mInstance != null && mDraggedItem != null)
		{
			switch (mDraggedItem.baseItem.equipmentSlot)
			{
			case NGUIBaseItem.EquipmentSlot.Weapon:
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mWeapon1Slot.gameObject, new Vector2(0f, 0f), -0.5f);
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mWeapon2Slot.gameObject, new Vector2(0f, 0f), -0.5f);
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mWeapon3Slot.gameObject, new Vector2(0f, 0f), -0.5f);
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mWeapon4Slot.gameObject, new Vector2(0f, 0f), -0.5f);
				break;
			case NGUIBaseItem.EquipmentSlot.WeaponG:
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mGrenadeSlot.gameObject, new Vector2(0f, 0f), -0.5f);
				break;
			case NGUIBaseItem.EquipmentSlot.Shield:
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mShieldSlot.gameObject, new Vector2(0f, 0f), -0.5f);
				break;
			case NGUIBaseItem.EquipmentSlot.SkillSlot:
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mSlot1Slot.gameObject, new Vector2(0f, 0f), -0.5f);
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mSlot2Slot.gameObject, new Vector2(0f, 0f), -0.5f);
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mSlot3Slot.gameObject, new Vector2(0f, 0f), -0.5f);
				UIFrameManager.GetInstance().CreateFrame(NGUIBackPackUIScript.mInstance.mSlot4Slot.gameObject, new Vector2(0f, 0f), -0.5f);
				break;
			}
		}
	}

	private void OnDrop(GameObject go)
	{
		if (mDraggedItem == null)
		{
			return;
		}
		NGUIGameItem nGUIGameItem = Replace(mDraggedItem);
		NGUIItemSlot nGUIItemSlot = null;
		if (mDraggedItem == nGUIGameItem)
		{
			NGUITools.PlaySound(errorSound);
			mDragingSlot.Replace(mDraggedItem);
			mSelectedSlot = mDragingSlot;
			Description.SetObserveItem(null);
			nGUIItemSlot = mDragingSlot;
		}
		else if (nGUIGameItem != null)
		{
			NGUITools.PlaySound(grabSound);
			NGUIGameItem nGUIGameItem2 = mDragingSlot.Replace(nGUIGameItem);
			if (nGUIGameItem2 == nGUIGameItem)
			{
				mDragingSlot.Replace(mDraggedItem);
				Replace(nGUIGameItem);
				nGUIItemSlot = mSelectedSlot;
			}
			else
			{
				mSelectedSlot = this;
				Description.SetObserveItem(null);
				nGUIItemSlot = this;
			}
		}
		else
		{
			NGUITools.PlaySound(placeSound);
			mSelectedSlot = this;
			SetDescriptionPosition();
			Description.SetObserveItem(null);
			nGUIItemSlot = this;
		}
		CreateFrameWhenSelected(nGUIItemSlot);
		mDraggedItem = null;
		mDragingSlot = null;
		UpdateCursor();
	}

	public void OnDropFail()
	{
		if (mDraggedItem != null)
		{
			Replace(mDraggedItem);
			if (NGUIBackPackUIScript.mInstance != null)
			{
				UIFrameManager.GetInstance().DeleteFrame(NGUIBackPackUIScript.mInstance.gameObject);
				CreateFrameWhenSelected(this);
			}
			mDraggedItem = null;
			mDragingSlot = null;
			UpdateCursor();
			mSelectedSlot = this;
			Description.SetObserveItem(null);
		}
	}

	private void OnSelect(bool selected)
	{
		if (selected)
		{
			if (NGUIBackPackUIScript.mInstance != null)
			{
				UIFrameManager.GetInstance().DeleteFrame(NGUIBackPackUIScript.mInstance.gameObject);
			}
			mSelectedSlot = this;
			if (mItem != null)
			{
				CreateFrameWhenSelected(this);
			}
			if (mDraggedItem != null)
			{
				OnDropFail();
			}
		}
	}

	private void UpdateCursor()
	{
		if (mDraggedItem != null && mDraggedItem.baseItem != null)
		{
			NGUICursor.Set(mDraggedItem.baseItem.iconAtlas, mDraggedItem.baseItem.iconName);
		}
		else
		{
			NGUICursor.Clear();
		}
	}

	private void Update()
	{
		NGUIGameItem nGUIGameItem = observedItem;
		if (mItem == nGUIGameItem && !needUpdate)
		{
			return;
		}
		mItem = nGUIGameItem;
		needUpdate = false;
		NGUIBaseItem baseItem = ((nGUIGameItem == null) ? null : nGUIGameItem.baseItem);
		if (label != null)
		{
			string text = ((nGUIGameItem == null) ? null : nGUIGameItem.name);
			if (string.IsNullOrEmpty(mText))
			{
				mText = label.text;
			}
			label.text = ((text == null) ? mText : text);
		}
		if (icon != null)
		{
			UpdateIcons(baseItem);
		}
	}

	protected virtual void UpdateIcons(NGUIBaseItem baseItem)
	{
		if (baseItem == null || baseItem.iconAtlas == null)
		{
			icon.gameObject.SetActive(false);
			backgroundIcon.gameObject.SetActive(false);
		}
		else
		{
			icon.atlas = baseItem.iconAtlas;
			icon.spriteName = baseItem.iconName;
			icon.gameObject.SetActive(true);
			backgroundIcon.gameObject.SetActive(true);
			backgroundIcon.atlas = baseItem.iconAtlas;
			backgroundIcon.spriteName = baseItem.GetBackGroundColorStringByQuality();
		}
		if ((baseItem == null || (baseItem.Quality != ItemQuality.Legendary && baseItem.Quality != ItemQuality.Epic)) && QualityEffectObject != null)
		{
			Object.Destroy(QualityEffectObject);
			QualityEffectObject = null;
		}
		if (baseItem != null && QualityEffectObject == null)
		{
			if (baseItem.Quality == ItemQuality.Legendary)
			{
				GameObject original = Resources.Load("RPG_effect/RPG_UI_Orange001") as GameObject;
				QualityEffectObject = Object.Instantiate(original) as GameObject;
				QualityEffectObject.transform.parent = base.transform;
				QualityEffectObject.transform.localPosition = Vector3.zero + Vector3.forward * -100f;
				QualityEffectObject.transform.localScale = base.transform.Find("Background").localScale + new Vector3(7f, 7f, 7f);
			}
			else if (baseItem.Quality == ItemQuality.Epic)
			{
				GameObject original2 = Resources.Load("RPG_effect/RPG_UI_Purple001") as GameObject;
				QualityEffectObject = Object.Instantiate(original2) as GameObject;
				QualityEffectObject.transform.parent = base.transform;
				QualityEffectObject.transform.localPosition = Vector3.zero + Vector3.forward * -100f;
				QualityEffectObject.transform.localScale = base.transform.Find("Background").localScale + new Vector3(5f, 5f, 5f);
			}
			if (QualityEffectObject != null && !base.GetComponent<Collider>().enabled)
			{
				QualityEffectObject.SetActive(false);
			}
		}
	}

	public virtual void EquipToSlot()
	{
	}

	public virtual void ThrowItem()
	{
		if (mItem != null)
		{
			short itemId = (short)mItem.baseItemID;
			Vector3 position = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetTransform()
				.position;
			position.y += 2f;
			Vector3 dropVelocity = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetTransform()
				.forward.normalized * Random.Range(1f, 2.5f);
			dropVelocity.y += Random.Range(2f, 4f);
			GameApp.GetInstance().GetLootManager().SpawnItemByNGUIBaseItem(mItem.baseItem, position, dropVelocity);
			Replace(null);
			mSelectedSlot = null;
			if (Description != null)
			{
				Description.SetObserveItem(null);
			}
			GameApp.GetInstance().GetUserState().m_questStateContainer.OnQuestProgressItemCollection(itemId);
		}
	}

	public bool HaveWeapon()
	{
		int num = 0;
		foreach (Weapon weapon in GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetWeaponList())
		{
			if (weapon != null)
			{
				num++;
			}
		}
		Debug.Log(num);
		if (num != 0)
		{
			return true;
		}
		return false;
	}

	public NGUIGameItem GetItem()
	{
		return observedItem;
	}

	public bool IsWeapon()
	{
		if (observedItem != null && observedItem.baseItem != null && (observedItem.baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Weapon || observedItem.baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.WeaponG))
		{
			return true;
		}
		return false;
	}

	public bool IsUsableItem()
	{
		if (mItem != null && mItem.baseItem != null && mItem.baseItem.equipmentSlot == NGUIBaseItem.EquipmentSlot.Pill)
		{
			return true;
		}
		return false;
	}

	public virtual void UseItem()
	{
		if (GameApp.GetInstance().GetUserState().ItemInfoData.HasPillInBag())
		{
			GameApp.GetInstance().GetUserState().ItemInfoData.UsePill(mItem);
			Replace(null);
			mSelectedSlot = null;
			if (Description != null)
			{
				Description.SetObserveItem(null);
			}
		}
	}

	protected void CreateFrameWhenSelected(NGUIItemSlot slot)
	{
		if (!(NGUIBackPackUIScript.mInstance != null))
		{
			return;
		}
		UIFrameManager.GetInstance().DeleteFrame(NGUIBackPackUIScript.mInstance.gameObject);
		if (!(slot != null))
		{
			return;
		}
		UIFrameManager.GetInstance().CreateFrame(slot.gameObject, new Vector2(0f, 0f), -1f);
		if (!slot.IsWeapon())
		{
			return;
		}
		BulletStateScript[] componentsInChildren = NGUIBackPackUIScript.mInstance.gameObject.GetComponentsInChildren<BulletStateScript>();
		BulletStateScript[] array = componentsInChildren;
		foreach (BulletStateScript bulletStateScript in array)
		{
			if (bulletStateScript.BulletType == (WeaponType)slot.GetItem().baseItem.ItemClass)
			{
				UIFrameManager.GetInstance().CreateFrame(bulletStateScript.gameObject, new Vector2(0f, 0f), -0.5f);
				break;
			}
		}
	}

	protected void SetDescriptionPosition()
	{
		if (base.transform.localPosition.x < 150f)
		{
			Description.transform.localPosition = NGUIBackPackUIScript.mInstance.DescriptionRightPoint.transform.localPosition;
			int childCount = NGUIBackPackUIScript.mInstance.mBackPack.transform.GetChildCount();
			for (int i = 0; i < childCount; i++)
			{
				Transform child = NGUIBackPackUIScript.mInstance.mBackPack.transform.GetChild(i);
				for (int j = 0; j < child.GetChildCount(); j++)
				{
					if (child.GetChild(j).tag == TagName.QUALITY_EFFECT)
					{
						if (child.localPosition.x > 150f)
						{
							child.GetChild(j).gameObject.SetActive(false);
						}
						else
						{
							child.GetChild(j).gameObject.SetActive(true);
						}
					}
				}
			}
			return;
		}
		Description.transform.localPosition = NGUIBackPackUIScript.mInstance.DescriptionLeftPoint.transform.localPosition;
		int childCount2 = NGUIBackPackUIScript.mInstance.mBackPack.transform.GetChildCount();
		for (int k = 0; k < childCount2; k++)
		{
			Transform child2 = NGUIBackPackUIScript.mInstance.mBackPack.transform.GetChild(k);
			for (int l = 0; l < child2.GetChildCount(); l++)
			{
				if (child2.GetChild(l).tag == TagName.QUALITY_EFFECT)
				{
					if (child2.localPosition.x > 150f)
					{
						child2.GetChild(l).gameObject.SetActive(true);
					}
					else
					{
						child2.GetChild(l).gameObject.SetActive(false);
					}
				}
			}
		}
	}
}
