using UnityEngine;

public class SightHeadScript : MonoBehaviour
{
	public GameObject CrossSightHead;

	public GameObject TiltedSightHead;

	public GameObject CenterPoint;

	private void OnEnable()
	{
		if (CenterPoint != null)
		{
			NGUITools.SetActive(CenterPoint, true);
		}
		if (CrossSightHead != null)
		{
			NGUITools.SetActive(CrossSightHead, true);
		}
		if (TiltedSightHead != null)
		{
			NGUITools.SetActive(TiltedSightHead, false);
		}
	}

	public void SetColor(Color _color)
	{
		if (CrossSightHead != null && CrossSightHead.activeSelf)
		{
			CrossSightHead.transform.Find("Up").GetComponent<UISprite>().color = _color;
			CrossSightHead.transform.Find("Down").GetComponent<UISprite>().color = _color;
			CrossSightHead.transform.Find("Left").GetComponent<UISprite>().color = _color;
			CrossSightHead.transform.Find("Right").GetComponent<UISprite>().color = _color;
		}
		else if (TiltedSightHead != null && TiltedSightHead.activeSelf)
		{
			CrossSightHead.transform.Find("LU").GetComponent<UISprite>().color = _color;
			CrossSightHead.transform.Find("LD").GetComponent<UISprite>().color = _color;
			CrossSightHead.transform.Find("RU").GetComponent<UISprite>().color = _color;
			CrossSightHead.transform.Find("RD").GetComponent<UISprite>().color = _color;
		}
		if (CenterPoint != null)
		{
			CenterPoint.GetComponent<UISprite>().color = _color;
		}
	}
}
