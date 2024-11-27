using UnityEngine;

public class ChatPopMenuButton : MonoBehaviour
{
	public UITweenX m_PopMenuTween;

	public UISprite m_ButtonSprite;

	private bool isPopMenuClosed = true;

	private void OnClick()
	{
		if (isPopMenuClosed)
		{
			OpenPopMenu();
		}
		else
		{
			ClosePopMenu();
		}
	}

	public void OpenPopMenu()
	{
		if (isPopMenuClosed)
		{
			m_PopMenuTween.PlayForward();
			m_ButtonSprite.gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
			isPopMenuClosed = false;
		}
	}

	public void ClosePopMenu()
	{
		if (!isPopMenuClosed)
		{
			m_PopMenuTween.PlayReverse();
			m_ButtonSprite.gameObject.transform.localEulerAngles = Vector3.zero;
			isPopMenuClosed = true;
		}
	}
}
