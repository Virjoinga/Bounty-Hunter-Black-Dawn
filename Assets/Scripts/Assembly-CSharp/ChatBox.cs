using System.Collections.Generic;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
	public class BlinkButton
	{
		private float DURATION_DURING_BLINKS = 0.5f;

		private int TIMES_BLINK = 4;

		private UIImageButton button;

		private bool isBlink;

		private float startTime;

		private float time;

		public BlinkButton(UIImageButton button)
		{
			this.button = button;
			isBlink = false;
		}

		public void StartBlink()
		{
			startTime = Time.time;
			isBlink = true;
			time = 0f;
		}

		public void UpdateBlink()
		{
			if (isBlink)
			{
				time = Time.time - startTime;
				int num = (int)(time / DURATION_DURING_BLINKS);
				if (num > TIMES_BLINK)
				{
					isBlink = false;
				}
				else
				{
					button.target.spriteName = ((num % 2 != 0) ? button.normalSprite : button.hoverSprite);
				}
			}
		}

		public bool IsBlink()
		{
			return isBlink;
		}
	}

	public UITextListX m_UITextList;

	public GameObject containerBlinkButton;

	public UITweenX m_Tween;

	public GameObject m_Arrow;

	public ChatQuickPhraseContainer m_QuickPhraseContainer;

	private UIImageButton[] m_Buttons;

	private List<BlinkButton> blinkButtonList = new List<BlinkButton>();

	private void Awake()
	{
		m_Buttons = containerBlinkButton.GetComponents<UIImageButton>();
		int num = 1;
		if (GameApp.GetInstance().GetGameMode().IsCoopMode())
		{
			num = 5;
			for (int i = 0; i < num; i++)
			{
				m_QuickPhraseContainer.AddPhrase(LocalizationManager.GetInstance().GetString("CHAT_PHRASE_" + i));
			}
		}
		else
		{
			num = 6;
			for (int j = 0; j < num; j++)
			{
				m_QuickPhraseContainer.AddPhrase(LocalizationManager.GetInstance().GetString("CHAT_PVP_PHRASE_" + j));
			}
		}
		List<string> chatList = UserStateHUD.GetInstance().ChatBox.ChatList;
		foreach (string item in chatList)
		{
			m_UITextList.Add(item);
		}
	}

	private void Update()
	{
		if (!UserStateHUD.GetInstance().ChatBox.HasRead)
		{
			m_UITextList.Clear();
			List<string> chatList = UserStateHUD.GetInstance().ChatBox.ChatList;
			foreach (string item in chatList)
			{
				m_UITextList.Add(item);
			}
			UIImageButton[] buttons = m_Buttons;
			foreach (UIImageButton button in buttons)
			{
				BlinkButton blinkButton = new BlinkButton(button);
				blinkButton.StartBlink();
				blinkButtonList.Add(blinkButton);
			}
		}
		if (blinkButtonList.Count <= 0)
		{
			return;
		}
		foreach (BlinkButton blinkButton2 in blinkButtonList)
		{
			blinkButton2.UpdateBlink();
		}
		if (!blinkButtonList[0].IsBlink())
		{
			blinkButtonList.Clear();
		}
	}

	public void Play()
	{
		m_Tween.Play();
		m_Arrow.transform.localEulerAngles += new Vector3(0f, 0f, 180f);
	}
}
