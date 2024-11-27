using UnityEngine;

public class CloseButton : UIDelegateMenu
{
	public static CloseButton instance;

	public GameObject m_CloseButton;

	private void Awake()
	{
		instance = this;
		AddDelegate(m_CloseButton);
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !InGameMenuManager.GetInstance().Lock)
		{
			InGameMenuManager.GetInstance().Close();
		}
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (!InGameMenuManager.GetInstance().Lock)
		{
			InGameMenuManager.GetInstance().Close();
		}
	}
}
