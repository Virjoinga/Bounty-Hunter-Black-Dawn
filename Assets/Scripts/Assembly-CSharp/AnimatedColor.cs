using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UIWidget))]
public class AnimatedColor : MonoBehaviour
{
	public Color color = Color.white;

	private UILabel mLabel;

	private void Awake()
	{
		mLabel = GetComponent<UILabel>();
	}

	private void Update()
	{
		mLabel.color = color;
	}
}
