using UnityEngine;

public class UIAdsPush : UIDelegateMenu
{
	[SerializeField]
	private GameObject buttonLink;

	[SerializeField]
	private GameObject buttonClose;

	private void Awake()
	{
		AddDelegate(buttonLink);
		AddDelegate(buttonClose);
	}

	protected override void OnClickThumb(GameObject go)
	{
		base.OnClickThumb(go);
		if (go.Equals(buttonLink))
		{
			MemoryManager.FreeNGUI(base.gameObject);
			string url = "https://itunes.apple.com/us/app/star-warfare-alien-invasion/id486314228?mt=8";
			Application.OpenURL(url);
		}
		else if (go.Equals(buttonClose))
		{
			MemoryManager.FreeNGUI(base.gameObject);
		}
	}
}
