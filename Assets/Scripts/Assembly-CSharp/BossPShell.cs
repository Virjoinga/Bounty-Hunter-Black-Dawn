public class BossPShell : Shell_ProtoType
{
	public override void CreateNavMeshAgent()
	{
		MyCreateNavMeshAgent();
	}

	public override bool IsBoss()
	{
		return false;
	}

	public override bool IsBossRush()
	{
		return true;
	}

	public override bool NeedEliteImg()
	{
		return false;
	}
}
