using UnityEngine;

public class UIPlayerName : MonoBehaviour
{
	public UILabel m_Label;

	protected Transform cameraTransform;

	private void Start()
	{
		if (Camera.main != null)
		{
			cameraTransform = Camera.main.transform;
		}
	}

	private void Update()
	{
		if (cameraTransform == null)
		{
			if (Camera.main != null)
			{
				cameraTransform = Camera.main.transform;
			}
		}
		else
		{
			base.transform.LookAt(cameraTransform);
			base.transform.RotateAround(base.transform.position, base.transform.up, 180f);
		}
	}

	public void SetName(string name, TeamName team)
	{
		m_Label.text = name;
		m_Label.color = UIConstant.COLOR_TEAM_PLAYER[(int)team];
	}

	public void SetName(string name, Color color)
	{
		m_Label.text = name;
		m_Label.color = color;
	}
}
