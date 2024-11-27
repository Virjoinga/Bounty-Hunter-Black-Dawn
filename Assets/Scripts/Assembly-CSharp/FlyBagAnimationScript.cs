using UnityEngine;

public class FlyBagAnimationScript : MonoBehaviour
{
	public Player player;

	protected bool enabledFire;

	protected Timer enableFireTimer = new Timer();

	protected Timer disableFireTimer = new Timer();

	private void Start()
	{
		base.GetComponent<Animation>()["front"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["back"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["left"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["right"].wrapMode = WrapMode.Loop;
		base.GetComponent<Animation>()["idle"].wrapMode = WrapMode.Loop;
		EnableFire();
		enableFireTimer.SetTimer(1f, false);
		disableFireTimer.SetTimer(30f, false);
	}

	private void Update()
	{
		if (player != null && player.inputController != null)
		{
			InputInfo inputInfo = player.inputController.inputInfo;
			if (inputInfo.IsMoving())
			{
				switch (inputInfo.dir)
				{
				case MoveDirection.forward:
					base.GetComponent<Animation>().CrossFade("front", 1f);
					break;
				case MoveDirection.back:
					base.GetComponent<Animation>().CrossFade("back", 1f);
					break;
				case MoveDirection.left:
					base.GetComponent<Animation>().CrossFade("left", 1f);
					break;
				case MoveDirection.right:
					base.GetComponent<Animation>().CrossFade("right", 1f);
					break;
				}
			}
			else
			{
				base.GetComponent<Animation>().CrossFade("idle", 1f);
			}
		}
		if (!enabledFire)
		{
			if (disableFireTimer.Ready())
			{
				EnableFire();
				enableFireTimer.Do();
			}
		}
		else if (enableFireTimer.Ready())
		{
			DisableFire();
			disableFireTimer.Do();
		}
	}

	public void EnableFire()
	{
		base.transform.GetChild(0).GetChild(0).GetComponent<ParticleEmitter>().enabled = true;
		base.transform.GetChild(1).GetChild(0).GetComponent<ParticleEmitter>().enabled = true;
		base.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = true;
		base.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().enabled = true;
		enabledFire = true;
	}

	public void DisableFire()
	{
		base.transform.GetChild(0).GetChild(0).GetComponent<ParticleEmitter>().enabled = false;
		base.transform.GetChild(1).GetChild(0).GetComponent<ParticleEmitter>().enabled = false;
		base.transform.GetChild(0).GetChild(0).GetComponent<Renderer>().enabled = false;
		base.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().enabled = false;
		enabledFire = false;
	}
}
