using UnityEngine;

public class StateCircleAim : MonoBehaviour
{
	public GameObject inner;

	public GameObject outer;

	private Vector3 initScale;

	private void Awake()
	{
		initScale = outer.transform.localScale;
	}

	private void OnEnable()
	{
		outer.transform.localScale = initScale / 2f;
	}

	private void Update()
	{
		outer.transform.localScale = Vector3.MoveTowards(outer.transform.localScale, initScale, 0.1f);
	}
}
