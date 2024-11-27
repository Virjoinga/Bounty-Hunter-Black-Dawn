using UnityEngine;

public class QualityEffectScript : IgnoreTimeScale
{
	public GameObject Effect;

	public string AnimationName;

	public GameObject ParticleObject;

	private AnimationState state;

	private void Awake()
	{
		state = Effect.GetComponent<Animation>()[AnimationName];
	}

	private void Update()
	{
		if (Time.timeScale != 0f)
		{
			return;
		}
		float num = UpdateRealTimeDelta();
		if (num == 0f)
		{
			return;
		}
		if (Effect != null)
		{
			float num2 = state.speed * num;
			state.time += num2;
			if (state.time > state.length)
			{
				state.time = 0f;
			}
		}
		if (ParticleObject != null)
		{
			ParticleObject.GetComponent<ParticleSystem>().Simulate(num, true, false);
		}
	}
}
