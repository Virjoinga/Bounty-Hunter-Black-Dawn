using UnityEngine;

public class UIFrame : MonoBehaviour
{
	public UISlicedSprite mSprite;

	public void SetSize(Vector2 size)
	{
		mSprite.transform.localScale = new Vector3(size.x, size.y, mSprite.transform.localPosition.z);
	}
}
