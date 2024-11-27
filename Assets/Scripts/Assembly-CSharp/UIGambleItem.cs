using UnityEngine;

public class UIGambleItem : MonoBehaviour
{
	public ItemPopMenuWindow m_WeaponWindow;

	public ItemPopMenuWindow m_ShieldWindow;

	public ItemPopMenuWindow m_GrenadeWindow;

	public ItemPopMenuWindow m_SlotWindow;

	public GameObject m_Collider;

	private bool isColliderOn;

	private void OnEnable()
	{
		m_WeaponWindow.gameObject.SetActiveRecursively(false);
		m_ShieldWindow.gameObject.SetActiveRecursively(false);
		m_GrenadeWindow.gameObject.SetActiveRecursively(false);
		m_SlotWindow.gameObject.SetActiveRecursively(false);
		m_Collider.SetActiveRecursively(false);
	}

	public void Show(NGUIBaseItem nguiBaseItem, ItemPopMenuEventListener itemPopMenuEventListener)
	{
		ItemPopMenuWindow itemPopMenuWindow = ((nguiBaseItem.ItemClass == ItemClasses.U_Shield) ? m_ShieldWindow : ((nguiBaseItem.ItemClass == ItemClasses.Grenade) ? m_GrenadeWindow : ((nguiBaseItem.ItemClass != ItemClasses.V_Slot) ? m_WeaponWindow : m_SlotWindow)));
		itemPopMenuWindow.gameObject.SetActiveRecursively(true);
		itemPopMenuWindow.SetNGUIBaseItem(nguiBaseItem);
		itemPopMenuWindow.SetEventListener(itemPopMenuEventListener);
		m_Collider.SetActiveRecursively(true);
	}
}
