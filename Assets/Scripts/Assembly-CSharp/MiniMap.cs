using UnityEngine;

public class MiniMap : MonoBehaviour
{
	public delegate void VoidDelegate(Vector2 delta);

	public delegate void FloatDelegate(float delta);

	public VoidDelegate onFingerDrag;

	public FloatDelegate onFingerStretch;

	private FingerTrick mFingerTrick;

	public Vector4 Range { get; set; }

	private void Start()
	{
		mFingerTrick = new FingerTrick(base.gameObject, Range);
	}

	private void Update()
	{
		mFingerTrick.Update();
	}

	private void OnFingerDrag(Vector2 delta)
	{
		if (onFingerDrag != null)
		{
			onFingerDrag(delta);
		}
	}

	private void OnFingerStretch(float delta)
	{
		if (onFingerStretch != null)
		{
			onFingerStretch(delta);
		}
	}
}
