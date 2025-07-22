using UnityEngine;

public class EffectTimeDelay : MonoBehaviour
{
	public enum EffectTimeDelayMode
	{
		None = 0,
		GameObjectOn = 1,
		GameObjectOff = 2,
		EmitterOn = 3,
		EmitterOff = 4,
		AnimationStart = 5,
		AnimationStop = 6
	}

	public EffectTimeDelayMode ModeSet;

	public float timeDelay;

	public Transform trans;

	private void Start()
	{
		if (!(trans == null) && ModeSet != 0)
		{
			Invoke("OnEffectEventFire", timeDelay);
		}
	}

	private void OnEffectEventFire()
	{
		switch (ModeSet)
		{
		case EffectTimeDelayMode.GameObjectOn:
			trans.gameObject.SetActiveRecursively(true);
			break;
		case EffectTimeDelayMode.GameObjectOff:
			trans.gameObject.SetActiveRecursively(false);
			break;
		case EffectTimeDelayMode.EmitterOn:
			if (trans.GetComponent<ParticleEmitter>() != null)
			{
				trans.GetComponent<ParticleEmitter>().emit = true;
			}
			break;
		case EffectTimeDelayMode.EmitterOff:
			if (trans.GetComponent<ParticleEmitter>() != null)
			{
				trans.GetComponent<ParticleEmitter>().emit = false;
			}
			break;
		case EffectTimeDelayMode.AnimationStart:
			if (trans.GetComponent<Animation>() != null)
			{
				trans.GetComponent<Animation>().Play();
			}
			break;
		case EffectTimeDelayMode.AnimationStop:
			if (trans.GetComponent<Animation>() != null)
			{
				trans.GetComponent<Animation>().Stop();
			}
			break;
		}
	}
}
