using UnityEngine;

public class UVAnimationXScript : MonoBehaviour
{
	public bool u = true;

	public bool v = true;

	public float scrollSpeed = 1f;

	public float maxScroll;

	protected float alpha;

	protected float startTime;

	public string alphaPropertyName = "_Alpha";

	public string texturePropertyName = "_MainTex";

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		float num = Time.time * scrollSpeed % maxScroll;
		if (u && v)
		{
			base.GetComponent<Renderer>().material.SetTextureOffset(texturePropertyName, new Vector2(num, num));
		}
		else if (u)
		{
			base.GetComponent<Renderer>().material.SetTextureOffset(texturePropertyName, new Vector2(num, 0f));
		}
		else if (v)
		{
			base.GetComponent<Renderer>().material.SetTextureOffset(texturePropertyName, new Vector2(0f, num));
		}
	}
}
