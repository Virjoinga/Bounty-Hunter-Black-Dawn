using UnityEngine;

public class TweenPanelClip : UITweener
{
	public UIPanel panel;

	public Vector4 from;

	public Vector4 to;

	private Transform mTrans;

	protected override void OnUpdate(float factor, bool isFinished)
	{
		panel.clipRange = from * (1f - factor) + to * factor;
	}
}
