using UnityEngine;

public class UIEquipmentSelectionSlot : MonoBehaviour
{
	public UISprite icon;

	public UISprite backgroundIcon;

	public NGUIBaseItem.EquipmentSlot EquipmentSlot;

	public int SlotNumber;

	protected GameObject QualityEffectObject;

	public NGUIGameItem EquipItem { get; set; }

	private void Start()
	{
	}

	private void Update()
	{
		if (!(icon != null))
		{
			return;
		}
		NGUIBaseItem nGUIBaseItem = ((EquipItem == null) ? null : EquipItem.baseItem);
		if (nGUIBaseItem == null || nGUIBaseItem.iconAtlas == null)
		{
			icon.gameObject.SetActive(false);
			backgroundIcon.gameObject.SetActive(false);
		}
		else
		{
			icon.atlas = nGUIBaseItem.iconAtlas;
			icon.spriteName = nGUIBaseItem.iconName;
			icon.gameObject.SetActive(true);
			backgroundIcon.gameObject.SetActive(true);
			backgroundIcon.atlas = nGUIBaseItem.iconAtlas;
			backgroundIcon.spriteName = nGUIBaseItem.GetBackGroundColorStringByQuality();
		}
		if ((nGUIBaseItem == null || (nGUIBaseItem.Quality != ItemQuality.Legendary && nGUIBaseItem.Quality != ItemQuality.Epic)) && QualityEffectObject != null)
		{
			Object.Destroy(QualityEffectObject);
			QualityEffectObject = null;
		}
		if (nGUIBaseItem != null && QualityEffectObject == null)
		{
			if (EquipItem.baseItem.Quality == ItemQuality.Legendary)
			{
				GameObject original = Resources.Load("RPG_effect/RPG_UI_Orange001") as GameObject;
				QualityEffectObject = Object.Instantiate(original) as GameObject;
				QualityEffectObject.transform.parent = base.transform;
				QualityEffectObject.transform.localPosition = base.transform.Find("Background").localPosition + Vector3.forward * -100f;
				QualityEffectObject.transform.localScale = base.transform.Find("Background").localScale + new Vector3(7f, 7f, 7f);
			}
			else if (EquipItem.baseItem.Quality == ItemQuality.Epic)
			{
				GameObject original2 = Resources.Load("RPG_effect/RPG_UI_Purple001") as GameObject;
				QualityEffectObject = Object.Instantiate(original2) as GameObject;
				QualityEffectObject.transform.parent = base.transform;
				QualityEffectObject.transform.localPosition = base.transform.Find("Background").localPosition + Vector3.forward * -100f;
				QualityEffectObject.transform.localScale = base.transform.Find("Background").localScale + new Vector3(5f, 5f, 5f);
			}
			if (QualityEffectObject != null && !base.GetComponent<Collider>().enabled)
			{
				QualityEffectObject.SetActive(false);
			}
		}
	}

	private void OnClick()
	{
		if (!UIEquipmentSelection.mInstance.mEquipmentUpgrade.UIEquipmentUpgrade.selfBlock.enabled && UIEquipmentSelection.mInstance != null)
		{
			UIFrameManager.GetInstance().DeleteFrame(UIEquipmentSelection.mInstance.gameObject);
			if (EquipItem != null)
			{
				UIFrameManager.GetInstance().CreateFrame(base.gameObject, new Vector2(0f, 0f), -0.5f);
			}
			UIEquipmentSelection.mInstance.SelectedEquip = EquipItem;
			UIEquipmentSelection.mInstance.mEquipmentUpgrade.mSlot = EquipmentSlot;
			UIEquipmentSelection.mInstance.mEquipmentUpgrade.mSlotNumber = SlotNumber;
		}
	}
}
