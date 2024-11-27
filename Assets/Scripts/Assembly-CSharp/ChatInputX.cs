using UnityEngine;

public class ChatInputX : MonoBehaviour
{
	public UITextList textList;

	public bool fillWithDummyData;

	private UIInput mInput;

	private bool mIgnoreNextEnter;

	private bool isFirst;

	private void Start()
	{
		mInput = GetComponent<UIInput>();
	}

	private void OnEnable()
	{
		isFirst = true;
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Return))
		{
			if (!mIgnoreNextEnter && !mInput.selected)
			{
				mInput.selected = true;
			}
			mIgnoreNextEnter = false;
		}
	}

	private void OnSubmit()
	{
		if (textList != null)
		{
			string text = NGUITools.StripSymbols(mInput.text);
			int userID = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetUserID();
			string displayName = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetDisplayName();
			if (!string.IsNullOrEmpty(text))
			{
				ChatRequest request = new ChatRequest(userID, displayName, text);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				mInput.text = string.Empty;
				mInput.selected = false;
			}
		}
		mIgnoreNextEnter = true;
	}

	private void Add(string text)
	{
		if (isFirst)
		{
			isFirst = false;
		}
		else if (text != null && mInput != null)
		{
			mInput.text += text;
		}
	}
}
