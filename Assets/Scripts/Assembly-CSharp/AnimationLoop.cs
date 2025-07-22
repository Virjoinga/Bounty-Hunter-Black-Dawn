using UnityEngine;

public class AnimationLoop : MonoBehaviour
{
	private void OnEnable()
	{
		Animation[] componentsInChildren = GetComponentsInChildren<Animation>();
		Animation[] array = componentsInChildren;
		foreach (Animation animation in array)
		{
			animation.wrapMode = WrapMode.Loop;
		}
	}
}
