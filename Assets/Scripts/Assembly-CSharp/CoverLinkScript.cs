using UnityEngine;

public class CoverLinkScript : MonoBehaviour
{
	public bool LinkVisible;

	public GameObject Expose;

	public GameObject Hide;

	public CoverType Type;

	public CoverDirection Direction;

	public bool Occupied;

	public int Id;

	public Vector3 CrouchPosition;

	private void OnDrawGizmos()
	{
		if (LinkVisible && null != Expose && null != Hide)
		{
			Gizmos.DrawLine(Expose.transform.position + Vector3.up * 0.1f, Hide.transform.position + Vector3.up * 0.1f);
		}
	}
}
