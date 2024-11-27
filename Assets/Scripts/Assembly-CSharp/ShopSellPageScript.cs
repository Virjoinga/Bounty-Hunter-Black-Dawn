using UnityEngine;

public class ShopSellPageScript : UIGameMenuShop
{
	public static ShopSellPageScript mInstance;

	public ShopItemSlotScript m_Weapon1Slot;

	public ShopItemSlotScript m_Weapon2Slot;

	public ShopItemSlotScript m_Weapon3Slot;

	public ShopItemSlotScript m_Weapon4Slot;

	public ShopItemSlotScript m_GrenadeSlot;

	public ShopItemSlotScript m_ShieldSlot;

	public ShopItemSlotScript m_Slot1Slot;

	public ShopItemSlotScript m_Slot2Slot;

	public ShopItemSlotScript m_Slot3Slot;

	public ShopItemSlotScript m_Slot4Slot;

	public ShopItemStorageScript m_BackPack;

	public GameObject Avatar;

	private float mLastUpdateAimItemTime;

	public GameObject m_Character;

	private new void Awake()
	{
		BoxCollider boxCollider = base.gameObject.GetComponent<Collider>() as BoxCollider;
		boxCollider.size = new Vector3(Screen.width, Screen.height, 0f);
	}

	private new void OnEnable()
	{
		mInstance = this;
		RefreshItems();
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

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (m_Character != null)
		{
			Object.Destroy(m_Character);
			m_Character = null;
		}
		mInstance = null;
	}

	public void CreateAvatar()
	{
		if (m_Character != null)
		{
			Object.Destroy(m_Character);
		}
		m_Character = AvatarBuilder.GetInstance().CreateUIAvatarWithWeapon(GameApp.GetInstance().GetUserState().GetRoleState(), Avatar, "UI", false);
	}

	public void RefreshItems()
	{
		CreateAvatar();
		ItemInfo itemInfoData = GameApp.GetInstance().GetUserState().ItemInfoData;
		if (!itemInfoData.IsWeapon3Enable)
		{
			m_Weapon3Slot.gameObject.SetActive(false);
		}
		if (!itemInfoData.IsWeapon4Enable)
		{
			m_Weapon4Slot.gameObject.SetActive(false);
		}
		if (!itemInfoData.IsSlot2Enable)
		{
			m_Slot2Slot.gameObject.SetActive(false);
		}
		if (!itemInfoData.IsSlot3Enable)
		{
			m_Slot3Slot.gameObject.SetActive(false);
		}
		if (!itemInfoData.IsSlot4Enable)
		{
			NGUITools.SetActive(m_Slot4Slot.gameObject, false);
		}
		m_BackPack.maxItemCount = itemInfoData.BackpackSlotCount;
	}

	private void OnClick()
	{
		if (ItemDescriptionScript.mInstance != null && ItemDescriptionScript.mInstance.gameObject.activeSelf)
		{
			ItemDescriptionScript.mInstance.SetObserveItem(null);
		}
	}
}
