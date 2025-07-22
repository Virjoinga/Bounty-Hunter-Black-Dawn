using UnityEngine;

public class StateMissision : MonoBehaviour
{
	public StateMissionUpdate m_StateMissionUpdate;

	public IconFlash m_SwitchButtonBackground;

	public IconFlash m_SwitchButtonIcon;

	public GameObject m_SwitchButton;

	private float mLastUpdateAimTime;

	private int i;

	private void Update()
	{
		if (!(Time.time - mLastUpdateAimTime > 0.2f))
		{
			return;
		}
		mLastUpdateAimTime = Time.time;
		string missionDetail = UserStateHUD.GetInstance().GetMissionDetail();
		if (missionDetail == null || missionDetail.Equals(string.Empty))
		{
			if (m_StateMissionUpdate.gameObject.activeSelf)
			{
				m_StateMissionUpdate.ClearData();
				m_SwitchButtonBackground.Refresh();
				m_SwitchButtonIcon.Refresh();
				m_StateMissionUpdate.gameObject.SetActive(false);
			}
			return;
		}
		if (UserStateHUD.GetInstance().IsMissionAccomplished())
		{
			EffectPlayer.GetInstance().PlayMissionAccomplished();
			m_SwitchButtonBackground.Resume(4);
			m_SwitchButtonIcon.Resume(4);
		}
		if (m_StateMissionUpdate.IsHasNoData())
		{
			if (!m_StateMissionUpdate.gameObject.activeSelf)
			{
				m_StateMissionUpdate.gameObject.SetActive(true);
			}
			m_StateMissionUpdate.ClearData();
			m_StateMissionUpdate.SetMissionText(missionDetail);
		}
		else if (UserStateHUD.GetInstance().IsMissionUpdate())
		{
			m_StateMissionUpdate.PlayEffects(missionDetail);
		}
		else if (!m_StateMissionUpdate.IsSameText(missionDetail))
		{
			m_StateMissionUpdate.ClearData();
			m_StateMissionUpdate.SetMissionText(missionDetail);
		}
		if (UserStateHUD.GetInstance().GetMissionCount() > 1)
		{
			if (!m_SwitchButton.gameObject.activeSelf)
			{
				m_SwitchButton.gameObject.SetActive(true);
			}
		}
		else if (m_SwitchButton.gameObject.activeSelf)
		{
			m_SwitchButton.gameObject.SetActive(false);
		}
	}
}
