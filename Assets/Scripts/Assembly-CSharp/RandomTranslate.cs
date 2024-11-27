using UnityEngine;

public class RandomTranslate : MonoBehaviour
{
	public int range = 6;

	public float multiple = 0.01f;

	private float dx;

	private float dz;

	private void OnEnable()
	{
		dx = (float)Random.Range(-range, range) * multiple;
		dz = (float)Random.Range(-range, range) * multiple;
	}

	private void Update()
	{
		base.gameObject.transform.position += new Vector3(dx, 0f, dz);
	}
}
