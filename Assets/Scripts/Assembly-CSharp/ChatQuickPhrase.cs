using UnityEngine;

public class ChatQuickPhrase : MonoBehaviour
{
	public UILabel m_QuickPhraseLabel;

	public void SetPhrase(string text)
	{
		m_QuickPhraseLabel.text = text;
	}

	private void OnClick()
	{
		ChatRequest request = new ChatRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetUserID(), GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetDisplayName(), m_QuickPhraseLabel.text);
		GameApp.GetInstance().GetNetworkManager().SendRequest(request);
	}
}
