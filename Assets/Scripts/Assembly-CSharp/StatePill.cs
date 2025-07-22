using UnityEngine;

public class StatePill : MonoBehaviour
{
	public UILabel m_PillNum;

	private float mLastUpdateTime;

	private void Update()
	{
		if (Time.time - mLastUpdateTime > 0.2f)
		{
			mLastUpdateTime = Time.time;
			m_PillNum.text = string.Empty + UserStateHUD.GetInstance().GetPillCountInBag();
		}
	}
}
