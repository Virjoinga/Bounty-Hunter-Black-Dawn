using UnityEngine;

[RequireComponent(typeof(UIPanel))]
public class SpringPanelX : IgnoreTimeScale
{
	public Vector3 target = Vector3.zero;

	public float strength = 10f;

	private UIPanel mPanel;

	private Transform mTrans;

	private float mThreshold;

	private UIDraggablePanelAlign mDrag;

	private void Start()
	{
		mPanel = GetComponent<UIPanel>();
		mDrag = GetComponent<UIDraggablePanelAlign>();
		mTrans = base.transform;
	}

	private void Update()
	{
		float deltaTime = UpdateRealTimeDelta();
		if (mThreshold == 0f)
		{
			mThreshold = (target - mTrans.localPosition).magnitude * 0.005f;
		}
		Vector3 localPosition = mTrans.localPosition;
		mTrans.localPosition = NGUIMath.SpringLerp(mTrans.localPosition, target, strength, deltaTime);
		Vector3 vector = mTrans.localPosition - localPosition;
		Vector4 clipRange = mPanel.clipRange;
		clipRange.x -= vector.x;
		clipRange.y -= vector.y;
		mPanel.clipRange = clipRange;
		if (mDrag != null)
		{
			mDrag.UpdateScrollbars(false);
		}
		if (mThreshold >= (target - mTrans.localPosition).magnitude)
		{
			base.enabled = false;
		}
	}

	public static SpringPanelX Begin(GameObject go, Vector3 pos, float strength)
	{
		SpringPanelX springPanelX = go.GetComponent<SpringPanelX>();
		if (springPanelX == null)
		{
			springPanelX = go.AddComponent<SpringPanelX>();
		}
		springPanelX.target = pos;
		springPanelX.strength = strength;
		if (!springPanelX.enabled)
		{
			springPanelX.mThreshold = 0f;
			springPanelX.enabled = true;
		}
		return springPanelX;
	}
}
