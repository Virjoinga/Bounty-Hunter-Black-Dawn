using UnityEngine;

public class UploadDataRequest : Request
{
	private BytesBuffer br;

	public UploadDataRequest()
	{
		requestID = 15;
		GlobalState globalState = GameApp.GetInstance().GetGlobalState();
		UserState userState = GameApp.GetInstance().GetUserState();
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		byte b = (byte)(45 + Global.DECORATION_PART_NUM + Global.TOTAL_STAGE);
		Debug.Log("BUFFTERLENGHT: " + b);
		br = new BytesBuffer(b);
		globalState.WriteMithril(br);
		globalState.WriteSaveNum(br);
		br.AddInt(localPlayer.Hp);
		br.AddInt(localPlayer.MaxHp);
		br.AddInt(localPlayer.Shield);
		br.AddInt(localPlayer.MaxShield);
		br.AddInt(localPlayer.ExtraShield);
		userState.WriteEnergy(br);
		userState.WriteCash(br);
		userState.WriteCharClass(br);
		userState.WriteSex(br);
		userState.WriteCharLevel(br);
		userState.WriteExp(br);
		userState.WriteAvatar(br);
		userState.WriteDecoration(br);
		userState.WriteStageState(br);
	}

	public UploadDataRequest(RobotUser robot)
	{
		requestID = 15;
		UserState userState = robot.GetUserState();
		byte b = (byte)(45 + Global.DECORATION_PART_NUM + Global.TOTAL_STAGE);
		Debug.Log("BUFFTERLENGHT: " + b);
		br = new BytesBuffer(b);
		br.AddInt(99);
		br.AddInt(0);
		br.AddInt(robot.Hp);
		br.AddInt(robot.MaxHp);
		br.AddInt(robot.Shield);
		br.AddInt(robot.MaxShield);
		br.AddInt(0);
		userState.WriteEnergy(br);
		userState.WriteCash(br);
		userState.WriteCharClass(br);
		userState.WriteSex(br);
		userState.WriteCharLevel(br);
		userState.WriteExp(br);
		userState.WriteAvatar(br);
		userState.WriteDecoration(br);
		userState.WriteStageState(br);
	}

	public override byte[] GetBody()
	{
		return br.GetBytes();
	}
}
