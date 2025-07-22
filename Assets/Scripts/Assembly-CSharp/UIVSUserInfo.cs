using UnityEngine;

public class UIVSUserInfo : MonoBehaviour
{
	public UILabel rankLabel;

	public UILabel nameLabel;

	public UISprite iconSprite;

	public UILabel winLabel;

	public UILabel lostLabel;

	public UILabel offlineLabel;

	public UILabel scoreLabel;

	public UILabel killLabel;

	public UILabel deadLabel;

	public UILabel assistLabel;

	public UILabel winningPercentageLabel;

	public GameObject localEffect;

	private UserStateHUD.VSUserState mVSUserState;

	public void UpdateInfo(UserStateHUD.GameUnitHUD unit)
	{
		mVSUserState = CreateInfo(unit);
	}

	public void UpdateInfo(UserStateHUD.VSUserState vsUserState)
	{
		mVSUserState = vsUserState;
	}

	private void Update()
	{
		if (mVSUserState == null)
		{
			return;
		}
		if (rankLabel != null)
		{
			rankLabel.text = string.Empty + mVSUserState.Rank;
		}
		if (nameLabel != null)
		{
			nameLabel.text = mVSUserState.Name;
		}
		if (iconSprite != null)
		{
			iconSprite.spriteName = mVSUserState.ClassSpriteName;
		}
		if (winLabel != null)
		{
			winLabel.text = string.Empty + mVSUserState.Win;
		}
		if (lostLabel != null)
		{
			lostLabel.text = string.Empty + mVSUserState.Lost;
		}
		if (offlineLabel != null)
		{
			offlineLabel.text = string.Empty + mVSUserState.Offline;
		}
		if (scoreLabel != null)
		{
			scoreLabel.text = ((!mVSUserState.ScoreVisible) ? string.Empty : (string.Empty + mVSUserState.Bonus));
		}
		if (killLabel != null)
		{
			killLabel.text = string.Empty + mVSUserState.LastKill;
		}
		if (deadLabel != null)
		{
			deadLabel.text = string.Empty + mVSUserState.LastDead;
		}
		if (assistLabel != null)
		{
			assistLabel.text = string.Empty + mVSUserState.LastAssist;
		}
		if (winningPercentageLabel != null)
		{
			winningPercentageLabel.text = ((mVSUserState.Win + mVSUserState.Lost + mVSUserState.Offline != 0) ? (string.Empty + mVSUserState.Win * 100 / (mVSUserState.Win + mVSUserState.Lost + mVSUserState.Offline) + "%") : "0%");
		}
		SetColor(mVSUserState, rankLabel);
		SetColor(mVSUserState, nameLabel);
		SetColor(mVSUserState, winLabel);
		SetColor(mVSUserState, lostLabel);
		SetColor(mVSUserState, offlineLabel);
		SetColor(mVSUserState, scoreLabel);
		SetColor(mVSUserState, killLabel);
		SetColor(mVSUserState, deadLabel);
		SetColor(mVSUserState, assistLabel);
		if (localEffect != null)
		{
			if (mVSUserState.IsLocal)
			{
				localEffect.SetActive(true);
			}
			else
			{
				localEffect.SetActive(false);
			}
		}
		mVSUserState = null;
	}

	private void SetColor(UserStateHUD.VSUserState vsUserState, UILabel label)
	{
		if (label != null)
		{
			label.color = ((!vsUserState.IsLocal) ? new Color(0f, 1f, 78f / 85f) : new Color(1f, 76f / 85f, 0f));
		}
	}

	public static UserStateHUD.VSUserState CreateInfo(UserStateHUD.GameUnitHUD unit)
	{
		VSTDMRank vSTDMRank = (VSTDMRank)unit.GetStatistics()[0];
		UserStateHUD.VSUserState vSUserState = new UserStateHUD.VSUserState();
		vSUserState.IsLocal = unit.type == UserStateHUD.GameUnitHUD.Type.LocalPlayer;
		vSUserState.Win = vSTDMRank.Win;
		vSUserState.Lost = vSTDMRank.Lost;
		vSUserState.Offline = vSTDMRank.Offline;
		vSUserState.Bonus = vSTDMRank.Score;
		vSUserState.Name = unit.Name;
		vSUserState.ClassSpriteName = unit.ClassIconSpriteName;
		return vSUserState;
	}
}
