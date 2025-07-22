using UnityEngine;

public class UVOffsetScript : MonoBehaviour
{
	public float scrollSpeed = 1f;

	protected float alpha;

	protected float startTime;

	public string texturePropertyName = "_tex2";

	public int materialIndex;

	public int cols = 3;

	public int rows = 3;

	public bool loop = true;

	public bool stop;

	protected float xStep;

	protected float yStep;

	private void Start()
	{
		startTime = Time.time;
		xStep = 1f / (float)cols;
		yStep = 1f / (float)rows;
	}

	private void Update()
	{
		if (!stop)
		{
			int num = (int)(Time.time * scrollSpeed) % (cols * rows);
			float x = xStep * (float)(num % cols);
			float num2 = 1f - yStep * (float)(num / cols + 1);
			if (num2 == 1f)
			{
				num2 = 0f;
			}
			if (!loop && num % cols == cols - 1 && num / cols == rows - 1)
			{
				stop = true;
			}
			base.GetComponent<Renderer>().materials[materialIndex].SetTextureOffset(texturePropertyName, new Vector2(x, num2));
		}
	}
}
