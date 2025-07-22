using UnityEngine;

public class PatrolGroupScript : MonoBehaviour
{
	public bool LinkVisible;

	public GameObject StartPoint;

	private void OnDrawGizmos()
	{
		if (LinkVisible && null != StartPoint)
		{
			PatrolPointScript component = StartPoint.GetComponent<PatrolPointScript>();
			if (null != component && null != component.NextPoint)
			{
				Gizmos.DrawLine(StartPoint.transform.position + Vector3.up * 0.1f, component.NextPoint.transform.position + Vector3.up * 0.1f);
			}
		}
	}
}
