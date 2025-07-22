using UnityEngine;

public class UIInfoBoxContent : MonoBehaviour
{
	public UIInfoBox mUIInfoBox;

	public UILabel mUILabel;

	private void Awake()
	{
		if (mUILabel == null)
		{
			mUILabel = GetComponent<UILabel>();
		}
		mUIInfoBox = NGUITools.FindInParents<UIInfoBox>(base.gameObject);
		if (mUIInfoBox == null)
		{
			Object.Destroy(this);
		}
	}

	public void SetInfo(string str)
	{
		if (mUILabel == null)
		{
			Debug.Log("No Label!");
		}
		else
		{
			mUILabel.text = str;
		}
	}
}
