using UnityEngine;

public class UserMithril : MonoBehaviour
{
	public UILabel m_Mithril;

	private void LateUpdate()
	{
		m_Mithril.text = string.Empty + GameApp.GetInstance().GetGlobalState().GetMithril();
	}
}
