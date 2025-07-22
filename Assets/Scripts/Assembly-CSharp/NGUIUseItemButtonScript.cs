public class NGUIUseItemButtonScript : NGUIBackPackObject
{
	private void Update()
	{
		if (NGUIItemSlot.mSelectedSlot != null && NGUIItemSlot.mSelectedSlot.IsUsableItem())
		{
			if (!base.GetComponent<UnityEngine.Collider>().enabled)
			{
				base.GetComponent<UnityEngine.Collider>().enabled = true;
				UIImageButton[] components = base.gameObject.GetComponents<UIImageButton>();
				UIImageButton[] array = components;
				foreach (UIImageButton uIImageButton in array)
				{
					NGUITools.SetActive(uIImageButton.target.gameObject, true);
				}
			}
		}
		else if (base.GetComponent<UnityEngine.Collider>().enabled)
		{
			base.GetComponent<UnityEngine.Collider>().enabled = false;
			UIImageButton[] components2 = base.gameObject.GetComponents<UIImageButton>();
			UIImageButton[] array2 = components2;
			foreach (UIImageButton uIImageButton2 in array2)
			{
				NGUITools.SetActive(uIImageButton2.target.gameObject, false);
			}
		}
	}

	private void OnClick()
	{
		if (NGUIBackPackUIScript.mInstance != null && !NGUIBackPackUIScript.mInstance.IsBagLocked && NGUIItemSlot.mSelectedSlot != null)
		{
			NGUIItemSlot.mSelectedSlot.UseItem();
		}
	}
}
