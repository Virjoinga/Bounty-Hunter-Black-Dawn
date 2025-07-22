public class ProtectedRadio : ProtectedItem
{
	public ProtectedRadio()
	{
		base.ProtectedType = EProtectedType.RADIO;
	}

	protected override string GetResourcePath()
	{
		return "Controllable/Protected/radio";
	}

	protected override void StartInit()
	{
		SetState(ControllableUnit.INIT_STATE);
	}

	public override void DoInit()
	{
		EndInit();
		StartIdle();
	}

	protected override void EndInit()
	{
	}

	protected override void StartIdle()
	{
		SetState(ControllableUnit.IDLE_STATE);
	}

	public override void DoIdle()
	{
	}

	protected override void EndIdle()
	{
	}

	public override void StartDead()
	{
		SetState(ControllableUnit.DEAD_STATE);
	}

	public override void DoDead()
	{
	}

	protected override void EndDead()
	{
	}

	public override void StartDisappear()
	{
		SetState(ControllableUnit.DISAPPEAR_STATE);
	}

	public override void DoDisappear()
	{
	}

	protected override void EndDisappear()
	{
	}
}
