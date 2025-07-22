using UnityEngine;

public class PlayerView
{
	protected Player player;

	protected GameObject chuangsongObj;

	protected Transform entityTransform;

	public void Init(Player player)
	{
		this.player = player;
		entityTransform = player.GetTransform();
	}

	public void PlayTeleportAnimation()
	{
		chuangsongObj.GetComponent<Animation>().Stop();
		chuangsongObj.SetActiveRecursively(true);
		chuangsongObj.GetComponent<Animation>()["Take 001"].wrapMode = WrapMode.ClampForever;
		chuangsongObj.GetComponent<Animation>().Play("Take 001");
	}

	public bool TeleportAnimationEnds()
	{
		return false;
	}

	public void EnableTeleportEffect(bool bEnable)
	{
		if (!bEnable)
		{
			chuangsongObj.SetActiveRecursively(false);
		}
		else
		{
			chuangsongObj.SetActiveRecursively(true);
		}
	}
}
