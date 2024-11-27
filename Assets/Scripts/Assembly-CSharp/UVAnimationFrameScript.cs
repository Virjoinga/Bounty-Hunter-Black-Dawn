using UnityEngine;

public class UVAnimationFrameScript : MonoBehaviour
{
	public float speed = 1f;

	public string texturePropertyName = "_MainTex";

	public int maxRow;

	public Vector2 frameWH;

	public int maxFrame;

	protected int frameIdx;

	protected float animationTime;

	private void Start()
	{
		frameIdx = 0;
		animationTime = 0f;
	}

	private void Update()
	{
		animationTime += Time.deltaTime;
		frameIdx = (int)(animationTime * speed);
		frameIdx %= maxFrame;
		int num = (int)(1f / frameWH.x);
		int num2 = frameIdx % num;
		int num3 = frameIdx / maxRow;
		Vector2 offset = new Vector2((float)num2 * frameWH.x, (float)num3 * frameWH.y);
		base.GetComponent<Renderer>().material.SetTextureOffset(texturePropertyName, offset);
	}
}
