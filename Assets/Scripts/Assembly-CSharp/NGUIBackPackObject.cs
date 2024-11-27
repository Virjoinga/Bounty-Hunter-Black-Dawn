using UnityEngine;

public class NGUIBackPackObject : MonoBehaviour
{
	private void OnDrop(GameObject go)
	{
		if (NGUIItemSlot.mDragingSlot != null)
		{
			NGUIItemSlot.mDragingSlot.OnDropFail();
		}
	}

	private void OnClick()
	{
		if (ItemDescriptionScript.mInstance != null && ItemDescriptionScript.mInstance.gameObject.activeSelf)
		{
			ItemDescriptionScript.mInstance.SetObserveItem(null);
		}
	}
}
