internal class OpenAimTask : Task
{
	public override void Do()
	{
		if (HUDManager.instance != null)
		{
			HUDManager.instance.m_InfoManager.m_Aim.SetActive(true);
		}
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.ActivatePlayer(false);
	}
}
