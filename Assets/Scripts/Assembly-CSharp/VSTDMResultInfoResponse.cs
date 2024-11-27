using UnityEngine;

internal class VSTDMResultInfoResponse : Response
{
	protected byte mWinner;

	protected short mMoney;

	public override void ReadData(byte[] data)
	{
		Debug.Log("111111111111");
		BytesBuffer bytesBuffer = new BytesBuffer(data);
		Debug.Log("2222222222222");
		mWinner = bytesBuffer.ReadByte();
		Debug.Log("333333333333");
		mMoney = bytesBuffer.ReadShort();
		Debug.Log("44444444444444");
	}

	public override void ProcessLogic()
	{
		InGameMenuManager.GetInstance().Close();
		UIVSBattleShop.Close();
		UIVSBattleResult.Close();
		Debug.Log("Winner-----" + mWinner);
		GameApp.GetInstance().GetUserState().AddCash(mMoney);
		UserStateHUD.VSBattleField vSBattleFieldState = UserStateHUD.GetInstance().GetVSBattleFieldState();
		vSBattleFieldState.RedTeam.SetScoreVisible(true);
		vSBattleFieldState.BlueTeam.SetScoreVisible(true);
		if (mWinner == 2)
		{
			vSBattleFieldState.BlueTeam.State = UserStateHUD.VSUserTeam.VSTeamState.Win;
			vSBattleFieldState.RedTeam.State = UserStateHUD.VSUserTeam.VSTeamState.Lost;
		}
		else
		{
			vSBattleFieldState.RedTeam.State = UserStateHUD.VSUserTeam.VSTeamState.Win;
			vSBattleFieldState.BlueTeam.State = UserStateHUD.VSUserTeam.VSTeamState.Lost;
		}
		UIVSBattleResult.ShowResult(vSBattleFieldState.RedTeam, vSBattleFieldState.BlueTeam, UIVSBattleResult.Condition.VSEnd);
	}
}
