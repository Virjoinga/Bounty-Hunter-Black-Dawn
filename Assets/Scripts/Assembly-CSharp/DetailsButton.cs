using UnityEngine;

public class DetailsButton : UIDelegateMenu
{
	public UILabel m_ButtonName;

	public UILabel m_Details;

	public GameObject m_Arrow;

	public GameObject m_DetailsButton;

	public GameObject m_Text;

	private void Awake()
	{
		AddDelegate(m_DetailsButton);
		m_Text.SetActive(false);
	}

	public void SetName(string name)
	{
		m_ButtonName.text = name;
	}

	public void SetDetails(string details)
	{
		m_Details.text = details;
	}

	protected override void OnClickThumb(GameObject go)
	{
		m_Arrow.transform.localEulerAngles += new Vector3(0f, 0f, 180f);
	}
}
