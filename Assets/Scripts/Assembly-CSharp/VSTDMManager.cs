using System.Collections.Generic;

public class VSTDMManager : VSManager
{
	public Dictionary<int, TargetPointInfo> pointInfo = new Dictionary<int, TargetPointInfo>();

	public VSTeamManager RedTeam = new VSTeamManager(TeamName.Red);

	public VSTeamManager BlueTeam = new VSTeamManager(TeamName.Blue);

	public bool PointCanCapture(int pointID)
	{
		if (GameApp.GetInstance().GetGameWorld().Is1V1VSScene() && pointID != 2)
		{
			return false;
		}
		switch (pointID)
		{
		case 0:
			if (pointInfo.ContainsKey(1) && pointInfo[1].GetOwner() == 2)
			{
				return true;
			}
			break;
		case 1:
			if (pointInfo.ContainsKey(2) && pointInfo[2].GetOwner() == 2 && pointInfo.ContainsKey(0) && pointInfo[0].GetOwner() == 1)
			{
				return true;
			}
			break;
		case 2:
			if (pointInfo.ContainsKey(3) && pointInfo[3].GetOwner() == 2 && pointInfo.ContainsKey(1) && pointInfo[1].GetOwner() == 1)
			{
				return true;
			}
			break;
		case 3:
			if (pointInfo.ContainsKey(4) && pointInfo[4].GetOwner() == 2 && pointInfo.ContainsKey(2) && pointInfo[2].GetOwner() == 1)
			{
				return true;
			}
			break;
		case 4:
			if (pointInfo.ContainsKey(3) && pointInfo[3].GetOwner() == 1)
			{
				return true;
			}
			break;
		}
		return false;
	}
}
