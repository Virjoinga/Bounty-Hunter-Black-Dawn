using UnityEngine;

public class NGUIBagArrowScript : MonoBehaviour
{
	public UIDraggablePanel uiDraggablePanel;

	public GameObject leftArrow;

	public GameObject rightArrow;

	public float error = 10f;

	private UIPanel uiPanel;

	private float mLeft;

	private float mRight;

	private void Awake()
	{
		uiPanel = uiDraggablePanel.gameObject.GetComponent<UIPanel>();
		mLeft = (0f - uiPanel.clipRange.z) / 2f;
		mRight = uiPanel.clipRange.z / 2f;
	}

	private void Update()
	{
		if (uiPanel.transform.localPosition.x > mLeft - error)
		{
			if (leftArrow.gameObject.activeSelf)
			{
				leftArrow.gameObject.SetActive(false);
			}
		}
		else if (!leftArrow.gameObject.activeSelf)
		{
			leftArrow.gameObject.SetActive(true);
		}
		if (uiPanel.transform.localPosition.x + uiDraggablePanel.bounds.size.x < mRight + error)
		{
			if (rightArrow.gameObject.activeSelf)
			{
				rightArrow.gameObject.SetActive(false);
			}
		}
		else if (!rightArrow.gameObject.activeSelf)
		{
			rightArrow.gameObject.SetActive(true);
		}
	}
}
