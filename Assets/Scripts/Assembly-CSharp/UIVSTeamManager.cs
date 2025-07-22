using UnityEngine;

public class UIVSTeamManager : MonoBehaviour
{
	public GameObject resourceMode;

	public UILabel resourceLabel;

	public UILabel strongholdLabel;

	public GameObject winEffect;

	public GameObject lostEffect;

	public UIVSUserInfoManager uiVSUserInfoManager;

	private UserStateHUD.VSUserTeam vsUserTeam;

	private bool bFirstTimeBeVisible;

	private void OnEnable()
	{
		bFirstTimeBeVisible = true;
		winEffect.SetActive(false);
		lostEffect.SetActive(false);
	}

	private void Update()
	{
		if (vsUserTeam == null)
		{
			return;
		}
		if (vsUserTeam.State == UserStateHUD.VSUserTeam.VSTeamState.Win && winEffect != null)
		{
			if (!winEffect.activeSelf)
			{
				winEffect.SetActive(true);
			}
			if (lostEffect.activeSelf)
			{
				lostEffect.SetActive(false);
			}
			if (bFirstTimeBeVisible)
			{
				bFirstTimeBeVisible = false;
				AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_match_win");
			}
		}
		if (vsUserTeam.State == UserStateHUD.VSUserTeam.VSTeamState.Lost && lostEffect != null)
		{
			if (!lostEffect.activeSelf)
			{
				lostEffect.SetActive(true);
			}
			if (winEffect.activeSelf)
			{
				winEffect.SetActive(false);
			}
			if (bFirstTimeBeVisible)
			{
				bFirstTimeBeVisible = false;
				AudioManager.GetInstance().PlaySoundSingle("RPG_Audio/VS/PVP_match_lose");
			}
		}
		if (vsUserTeam.GameMode == UIVS.Mode.CaptureHold_4v4 || vsUserTeam.GameMode == UIVS.Mode.CaptureHold_1v1)
		{
			resourceMode.SetActive(true);
			if (resourceLabel != null)
			{
				resourceLabel.text = string.Empty + vsUserTeam.Resource;
			}
			if (strongholdLabel != null)
			{
				strongholdLabel.text = string.Empty + vsUserTeam.Point;
			}
		}
		else
		{
			resourceMode.SetActive(false);
		}
		if (uiVSUserInfoManager != null)
		{
			uiVSUserInfoManager.UpdatePlayer(vsUserTeam.VSUserStateList);
		}
	}

	public void UpdateTeam(UserStateHUD.VSUserTeam vsUserTeam)
	{
		if (vsUserTeam != null)
		{
			this.vsUserTeam = vsUserTeam;
		}
	}
}
