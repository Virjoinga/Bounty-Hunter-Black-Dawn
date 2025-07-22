using UnityEngine;

public class BossFather : MobSMG
{
	public override void Activate()
	{
		base.Activate();
		GetObject().transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
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
		return true;
	}
}
