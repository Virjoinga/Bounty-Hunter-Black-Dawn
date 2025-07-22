using UnityEngine;

public class StateUser : MonoBehaviour
{
	[SerializeField]
	private UILabel m_LabelLevel;

	[SerializeField]
	private UIFilledSprite m_HP;

	[SerializeField]
	private UIFilledSprite m_EXP;

	[SerializeField]
	private UISprite m_Class;

	[SerializeField]
	private UISprite m_RoomMaster;

	[SerializeField]
	private UISprite m_SPTL;

	[SerializeField]
	private UISprite m_SPTR;

	[SerializeField]
	private UISprite m_SPBL;

	[SerializeField]
	private UISprite m_SPBR;

	private int lastLevel = -1;

	private int lastHp = -1;

	private int lastMaxHp = -1;

	private int lastUserExp = -1;

	private int lastNextLvRequiredExp = -1;

	private Interval tlInterval;

	private Interval trInterval;

	private Interval brInterval;

	private Interval blInterval;

	private void Start()
	{
		m_Class.spriteName = string.Empty;
		tlInterval = new Interval(7500, 833, 833, 835);
		trInterval = new Interval(5000, 833, 833, 3335);
		brInterval = new Interval(2500, 833, 833, 5835);
		blInterval = new Interval(1, 833, 833, 8334);
	}

	private void Update()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		if (UserStateHUD.GetInstance().IsUserRoomMaster())
		{
			if (!m_RoomMaster.gameObject.activeSelf)
			{
				m_RoomMaster.gameObject.SetActive(true);
			}
		}
		else if (m_RoomMaster.gameObject.activeSelf)
		{
			m_RoomMaster.gameObject.SetActive(false);
		}
		if (m_Class.spriteName.EndsWith(string.Empty))
		{
			m_Class.spriteName = UserStateHUD.GetInstance().GetUserClassIconName();
		}
		if (lastLevel != UserStateHUD.GetInstance().GetUserLevel())
		{
			lastLevel = UserStateHUD.GetInstance().GetUserLevel();
			m_LabelLevel.text = UserStateHUD.GetInstance().GetUserLevelStr();
		}
		if (lastHp != UserStateHUD.GetInstance().GetUserHp() || lastMaxHp != UserStateHUD.GetInstance().GetUserMaxHp())
		{
			lastHp = UserStateHUD.GetInstance().GetUserHp();
			lastMaxHp = UserStateHUD.GetInstance().GetUserMaxHp();
			m_HP.fillAmount = (float)lastHp / (float)lastMaxHp;
		}
		if (UserStateHUD.GetInstance().GetUserMaxShield() == 0)
		{
			CheckSPTLState(0);
			CheckSPTRState(0);
			CheckSPBRState(0);
			CheckSPBLState(0);
		}
		else
		{
			int value = UserStateHUD.GetInstance().GetUserShield() * 10000 / UserStateHUD.GetInstance().GetUserMaxShield();
			CheckSPTLState(tlInterval.GetIndex(value));
			CheckSPTRState(trInterval.GetIndex(value));
			CheckSPBRState(brInterval.GetIndex(value));
			CheckSPBLState(blInterval.GetIndex(value));
		}
		if (lastUserExp != UserStateHUD.GetInstance().GetUserExp() || lastNextLvRequiredExp != UserStateHUD.GetInstance().GetNextLvRequiredExp())
		{
			lastUserExp = UserStateHUD.GetInstance().GetUserExp() - Exp.RequiredLevelUpExp[GameApp.GetInstance().GetUserState().GetCharLevel() - 1];
			lastNextLvRequiredExp = Exp.RequiredLevelUpExp[GameApp.GetInstance().GetUserState().GetCharLevel()] - Exp.RequiredLevelUpExp[GameApp.GetInstance().GetUserState().GetCharLevel() - 1];
			m_EXP.fillAmount = (float)lastUserExp / (float)lastNextLvRequiredExp;
		}
	}

	private void CheckSPTLState(int state)
	{
		CheckSPState(state, m_SPTL, "head_bkup");
	}

	private void CheckSPTRState(int state)
	{
		CheckSPState(state, m_SPTR, "head_bkup0");
	}

	private void CheckSPBLState(int state)
	{
		CheckSPState(state, m_SPBL, "head_bkdn");
	}

	private void CheckSPBRState(int state)
	{
		CheckSPState(state, m_SPBR, "head_bkdn0");
	}

	private void CheckSPState(int state, UISprite sp, string spriteName)
	{
		if (state == 0)
		{
			if (sp.gameObject.activeSelf)
			{
				sp.gameObject.SetActive(false);
			}
		}
		else if (state < 4)
		{
			if (!sp.gameObject.activeSelf)
			{
				sp.gameObject.SetActive(true);
			}
			if (!sp.spriteName.Equals(spriteName + (3 - state)))
			{
				sp.spriteName = spriteName + (3 - state);
				sp.MakePixelPerfect();
			}
		}
	}
}
