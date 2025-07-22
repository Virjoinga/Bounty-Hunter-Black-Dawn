using UnityEngine;

public class UIMap : UIGameMenuNormal
{
	public GameObject m_MapCamera;

	public GameObject m_MapWindow;

	public GameObject m_Info;

	public GameObject m_Control;

	public GameObject m_Warning;

	protected override void Awake()
	{
		base.Awake();
		if (Arena.GetInstance().IsCurrentSceneArena() || GameApp.GetInstance().GetGameWorld().IsVSScene())
		{
			m_MapCamera.SetActive(false);
			m_MapWindow.SetActive(false);
			m_Info.SetActive(false);
			m_Control.SetActive(false);
			m_Warning.SetActive(true);
		}
		else
		{
			m_MapCamera.SetActive(true);
			m_MapWindow.SetActive(true);
			m_Info.SetActive(true);
			m_Control.SetActive(true);
			m_Warning.SetActive(false);
		}
	}
}
