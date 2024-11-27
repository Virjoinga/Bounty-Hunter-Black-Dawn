using UnityEngine;

public class VSTDMPoint : MonoBehaviour
{
	public bool disable;

	public GameObject point;

	public TweenAlpha redLight;

	public TweenAlpha blueLight;

	public GameObject pointLock;

	public GameObject pointRedLight;

	public GameObject pointBlueLight;

	public UIFilledSprite redPoint;

	public UIFilledSprite bluePoint;

	public GameObject barContainer;

	public UIFilledSprite redBar;

	public UIFilledSprite blueBar;

	private byte lastTeam;

	private float lastCapturingTime;

	private Timer timer;

	private void Awake()
	{
		timer = new Timer();
		timer.SetTimer(2f, false);
		redLight.enabled = false;
		blueLight.enabled = false;
		pointLock.SetActive(false);
		pointRedLight.SetActive(false);
		pointBlueLight.SetActive(false);
		redPoint.fillAmount = 0f;
		bluePoint.fillAmount = 0f;
		lastCapturingTime = 0f;
		lastTeam = 99;
		barContainer.SetActive(false);
	}

	private void Start()
	{
		if (disable)
		{
			base.gameObject.SetActive(false);
		}
	}

	public void SetPoint(UserStateHUD.VSBattleFieldPoint pointStatus, bool newBattle)
	{
		if (disable)
		{
			return;
		}
		if (pointStatus.Owner == 0)
		{
			pointLock.SetActive(false);
			pointRedLight.SetActive(false);
			pointBlueLight.SetActive(false);
			float num = pointStatus.CapturingTime / pointStatus.TotalCaptureTime;
			float num2 = (0f - pointStatus.CapturingTime) / pointStatus.TotalCaptureTime;
			redPoint.fillAmount = Mathf.Clamp(num, 0f, 1f);
			bluePoint.fillAmount = Mathf.Clamp(num2, 0f, 1f);
			BarControl(num, num2, pointStatus);
			if (UserStateHUD.GetInstance().GetUserInBattleFieldPoint() > -1)
			{
				if (!barContainer.activeSelf)
				{
					barContainer.SetActive(true);
				}
				redBar.fillAmount = Mathf.Clamp(pointStatus.CapturingTime / pointStatus.TotalCaptureTime, 0f, 1f);
				blueBar.fillAmount = Mathf.Clamp((0f - pointStatus.CapturingTime) / pointStatus.TotalCaptureTime, 0f, 1f);
			}
			else if (barContainer.activeSelf)
			{
				barContainer.SetActive(false);
			}
		}
		else if (!pointStatus.IsCapturing)
		{
			pointLock.SetActive(true);
			pointRedLight.SetActive(false);
			pointBlueLight.SetActive(false);
			if (pointStatus.Owner == 1)
			{
				redPoint.fillAmount = 1f;
				bluePoint.fillAmount = 0f;
			}
			else
			{
				redPoint.fillAmount = 0f;
				bluePoint.fillAmount = 1f;
			}
		}
		else
		{
			pointLock.SetActive(false);
			if (pointStatus.Owner == 1)
			{
				pointRedLight.SetActive(true);
				pointBlueLight.SetActive(false);
				float num3 = Mathf.Clamp((0f - pointStatus.CapturingTime) / pointStatus.TotalCaptureTime, 0f, 1f);
				redPoint.fillAmount = 1f - num3;
				bluePoint.fillAmount = num3;
				if (lastCapturingTime != pointStatus.CapturingTime && !newBattle)
				{
					lastCapturingTime = pointStatus.CapturingTime;
					redLight.enabled = true;
					blueLight.enabled = false;
					timer.Do();
				}
				else if (timer.Ready())
				{
					redLight.Reset();
					redLight.enabled = false;
				}
				BarControl(1f - num3, num3, pointStatus);
			}
			else
			{
				pointRedLight.SetActive(false);
				pointBlueLight.SetActive(true);
				float num4 = Mathf.Clamp(pointStatus.CapturingTime / pointStatus.TotalCaptureTime, 0f, 1f);
				redPoint.fillAmount = num4;
				bluePoint.fillAmount = 1f - num4;
				if (lastCapturingTime != pointStatus.CapturingTime && !newBattle)
				{
					lastCapturingTime = pointStatus.CapturingTime;
					blueLight.enabled = true;
					redLight.enabled = false;
					timer.Do();
				}
				else if (timer.Ready())
				{
					blueLight.Reset();
					blueLight.enabled = false;
				}
				BarControl(num4, 1f - num4, pointStatus);
			}
		}
		if (lastTeam == 99 || newBattle)
		{
			lastTeam = pointStatus.Owner;
		}
		else
		{
			if (lastTeam == pointStatus.Owner)
			{
				return;
			}
			lastTeam = pointStatus.Owner;
			GameObject gameObject = Object.Instantiate(point) as GameObject;
			gameObject.transform.parent = base.gameObject.transform;
			gameObject.transform.localPosition = point.transform.localPosition;
			gameObject.transform.localEulerAngles = point.transform.localEulerAngles;
			gameObject.transform.localScale = point.transform.localScale;
			gameObject.GetComponent<TweenAlphaX>().enabled = true;
			gameObject.GetComponent<TweenScale>().enabled = true;
			gameObject.GetComponent<AutoDestroyScript>().enabled = true;
			if (pointStatus.Owner + 1 == (int)GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.Team)
			{
				AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_point_captured");
			}
			else
			{
				AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_point_lost");
			}
			if (UserStateHUD.GetInstance().GetUserInBattleFieldPoint() > -1)
			{
				if (lastTeam == 1)
				{
					UserStateHUD.GetInstance().SetVSTDMCaptureSignVisible(TeamName.Red);
				}
				if (lastTeam == 2)
				{
					UserStateHUD.GetInstance().SetVSTDMCaptureSignVisible(TeamName.Blue);
				}
				UserStateHUD.GetInstance().SetUserOutBattleFieldPoint();
			}
		}
	}

	private void BarControl(float red, float blue, UserStateHUD.VSBattleFieldPoint pointStatus)
	{
		if (UserStateHUD.GetInstance().GetUserInBattleFieldPoint() < 0)
		{
			if (barContainer.activeSelf)
			{
				barContainer.SetActive(false);
			}
		}
		else
		{
			if (UserStateHUD.GetInstance().GetUserInBattleFieldPoint() != pointStatus.PointID)
			{
				return;
			}
			if ((UserStateHUD.GetInstance().GetUserTeamName() == TeamName.Red && red == 1f) || (UserStateHUD.GetInstance().GetUserTeamName() == TeamName.Blue && blue == 1f))
			{
				if (barContainer.activeSelf)
				{
					barContainer.SetActive(false);
				}
				return;
			}
			if (!barContainer.activeSelf)
			{
				barContainer.SetActive(true);
			}
			redBar.fillAmount = Mathf.Clamp(red, 0f, 1f);
			blueBar.fillAmount = Mathf.Clamp(blue, 0f, 1f);
		}
	}
}
