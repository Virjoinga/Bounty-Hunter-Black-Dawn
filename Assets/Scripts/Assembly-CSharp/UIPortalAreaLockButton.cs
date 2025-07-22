using UnityEngine;

public class UIPortalAreaLockButton : MonoBehaviour
{
	public UILabel m_AreaId;

	public void SetNumber(int id)
	{
		m_AreaId.text = string.Empty + id;
	}
}
