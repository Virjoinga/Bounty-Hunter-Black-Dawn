using UnityEngine;

public class StateSightHead : MonoBehaviour
{
	public GameObject m_Default;

	public GameObject m_Spin;

	public Camera m_Camera;

	private float mLastUpdateAimTime;

	public Transform Up;

	public Transform Down;

	public Transform Left;

	public Transform Right;

	public Transform LU;

	public Transform LD;

	public Transform RU;

	public Transform RD;

	public float AimOffset { get; set; }

	private void OnEnable()
	{
		NGUITools.SetActive(m_Default, false);
		mLastUpdateAimTime = Time.time;
		NGUITools.SetActive(LU.gameObject, false);
		NGUITools.SetActive(LD.gameObject, false);
		NGUITools.SetActive(RU.gameObject, false);
		NGUITools.SetActive(RD.gameObject, false);
	}

	private void Update()
	{
		UpdateWeaponRecoil();
		UpdateSightHeadColor();
	}

	private void UpdateSightHeadColor()
	{
		if (!(Time.time - mLastUpdateAimTime > 0.2f))
		{
			return;
		}
		mLastUpdateAimTime = Time.time;
		UserStateHUD.GameUnitHUD targetAimed = UserStateHUD.GetInstance().TargetAimed;
		switch (targetAimed.type)
		{
		case UserStateHUD.GameUnitHUD.Type.None:
			if (!m_Default.activeSelf)
			{
				NGUITools.SetActive(m_Default, true);
			}
			m_Default.GetComponent<SightHeadScript>().SetColor(Color.green);
			break;
		case UserStateHUD.GameUnitHUD.Type.Enemy:
			if (!m_Default.activeSelf)
			{
				NGUITools.SetActive(m_Default, true);
			}
			m_Default.GetComponent<SightHeadScript>().SetColor(Color.red);
			break;
		case UserStateHUD.GameUnitHUD.Type.RemotePlayer:
			if (targetAimed.GetTeamName() == UserStateHUD.GetInstance().GetUserTeamName())
			{
				if (!m_Default.activeSelf)
				{
					NGUITools.SetActive(m_Default, true);
				}
				m_Default.GetComponent<SightHeadScript>().SetColor(Color.green);
			}
			else
			{
				if (!m_Default.activeSelf)
				{
					NGUITools.SetActive(m_Default, true);
				}
				m_Default.GetComponent<SightHeadScript>().SetColor(Color.red);
			}
			break;
		}
	}

	private void UpdateSpin()
	{
		if (m_Spin.activeSelf)
		{
			NGUITools.SetActive(m_Spin, false);
		}
		if (UserStateHUD.GetInstance().Reload)
		{
			UserStateHUD.GetInstance().Reload = false;
			NGUITools.SetActive(m_Spin, true);
		}
	}

	private void UpdateWeaponRecoil()
	{
		Camera camera = m_Camera.GetComponent<Camera>();
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		int num = (int)(localPlayer.GetWeapon().CalculateMaxAccurancyFactor() * camera.pixelHeight * 0.5f);
		int num2 = (int)(localPlayer.GetWeapon().CalculateMinAccurancyFactor() * camera.pixelHeight * 0.5f);
		AimOffset = localPlayer.GetWeapon().AimOffset;
		if (AimOffset < (float)(num2 + 7))
		{
			AimOffset = num2 + 7;
		}
		if (AimOffset > (float)num)
		{
			AimOffset = num;
		}
		if (m_Default.activeSelf)
		{
			SightHeadScript component = m_Default.GetComponent<SightHeadScript>();
			if (component.CrossSightHead.activeSelf)
			{
				Up.localPosition = new Vector3(0f, AimOffset);
				Down.localPosition = new Vector3(0f, 0f - AimOffset);
				Left.localPosition = new Vector3(0f - AimOffset, 0f);
				Right.localPosition = new Vector3(AimOffset, 0f);
			}
			else if (component.TiltedSightHead.activeSelf)
			{
				LU.position = new Vector3(0f - AimOffset, AimOffset);
				LD.position = new Vector3(0f - AimOffset, 0f - AimOffset);
				RU.position = new Vector3(AimOffset, AimOffset);
				RD.position = new Vector3(AimOffset, 0f - AimOffset);
			}
		}
	}
}
