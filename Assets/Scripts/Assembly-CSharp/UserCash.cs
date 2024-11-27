using UnityEngine;

public class UserCash : MonoBehaviour
{
	public UILabel m_Cash;

	private void LateUpdate()
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		m_Cash.text = string.Empty + userState.GetCash();
	}
}
