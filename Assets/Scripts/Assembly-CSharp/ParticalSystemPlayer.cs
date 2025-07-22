using UnityEngine;

public class ParticalSystemPlayer : IgnoreTimeScale
{
	public ParticleSystem[] m_ParticleSystems;

	public float frequency = 1f;

	private void Update()
	{
		float num = UpdateRealTimeDelta();
		if (num != 0f)
		{
			ParticleSystem[] particleSystems = m_ParticleSystems;
			foreach (ParticleSystem particleSystem in particleSystems)
			{
				particleSystem.Simulate(num * frequency, true, false);
			}
		}
	}
}
