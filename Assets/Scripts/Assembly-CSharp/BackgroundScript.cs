using UnityEngine;

public class BackgroundScript : IgnoreTimeScale
{
	public GameObject m_Effect;

	private AnimationState state;

	private void Awake()
	{
		GameObject gameObject = m_Effect.transform.Find("Plane01").gameObject;
		state = gameObject.GetComponent<Animation>()["RPG_anim_037"];
	}

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (num != 0f && m_Effect != null)
		{
			float num2 = state.speed * num;
			state.time += num2;
			if (state.time > state.length)
			{
				state.time = 0f;
			}
		}
	}
}
