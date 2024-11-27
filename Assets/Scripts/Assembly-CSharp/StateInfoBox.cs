using UnityEngine;

public class StateInfoBox : MonoBehaviour
{
	public UIInfoBox m_UIInfoBox;

	public UINumberMessage m_UINumberMessage;

	private void Update()
	{
		UserStateHUD.InfoBoxHUD.InfoHUD infoHUD = UserStateHUD.GetInstance().InfoBox.PopInfo();
		if (infoHUD != null)
		{
			if (infoHUD.GetType == UserStateHUD.InfoBoxHUD.InfoHUD.InfoType.PopInfo)
			{
				m_UIInfoBox.AddInfo(infoHUD.StrInfo);
			}
			else if (infoHUD.GetType == UserStateHUD.InfoBoxHUD.InfoHUD.InfoType.NumberInfo)
			{
				m_UINumberMessage.SetMessage(infoHUD.StrInfo, infoHUD.NumberInfo);
			}
		}
	}
}
