using UnityEngine;

public class UIPortalAreaButton : UIDelegateMenu
{
	private static float HEIGHT = 54f;

	public GameObject m_Upper;

	public GameObject m_Under;

	public UITweenX m_Tween1;

	public UITweenX m_Tween2;

	public GameObject m_DestinationContainer;

	public GameObject m_OtherContainer;

	public UILabel m_AreaName;

	public UILabel m_AreaId;

	private int destNum;

	private bool isPlayingAnimation;

	private bool isForward = true;

	private void Awake()
	{
		AddDelegate(m_Upper);
		AddDelegate(m_Under);
	}

	private void Update()
	{
		m_Under.transform.localPosition = new Vector3(0f, (float)(-destNum) * HEIGHT * m_DestinationContainer.transform.localScale.y, 0f);
	}

	public void SetName(string name)
	{
		m_AreaName.text = name;
	}

	public void SetNumber(int id)
	{
		m_AreaId.text = string.Empty + id;
	}

	public void AddDestinationButton(GameObject button)
	{
		button.transform.parent = m_DestinationContainer.transform;
		button.transform.localPosition = new Vector3(0f, (float)(-destNum) * HEIGHT, 0f);
		button.transform.localEulerAngles = Vector3.zero;
		button.transform.localScale = Vector3.one;
		button.SetActive(true);
		destNum++;
	}

	protected override void OnClickThumb(GameObject go)
	{
		Select();
	}

	public void Select()
	{
		if (!isPlayingAnimation)
		{
			isPlayingAnimation = true;
			if (isForward)
			{
				m_Tween1.PlayForward(PlayForwardTween2);
			}
			else
			{
				m_Tween2.PlayReverse(PlayReverseTween1);
			}
		}
	}

	private void PlayForwardTween2()
	{
		m_OtherContainer.SetActive(false);
		m_Tween2.PlayForward(PlayForwardOver);
	}

	private void PlayForwardOver()
	{
		isForward = false;
		isPlayingAnimation = false;
	}

	private void PlayReverseTween1()
	{
		m_OtherContainer.SetActive(true);
		m_Tween1.PlayReverse(PlayReverseOver);
	}

	private void PlayReverseOver()
	{
		isForward = true;
		isPlayingAnimation = false;
	}
}
