using HutongGames.PlayMaker;
using UnityEngine;

[ActionCategory(ActionCategory.Debug)]
[Tooltip("Draw gizmos shape")]
public class DebugDrawShape : FsmStateAction
{
	public enum ShapeType
	{
		Sphere = 0,
		Cube = 1,
		WireSphere = 2,
		WireCube = 3
	}

	[RequiredField]
	public FsmOwnerDefault gameObject;

	public ShapeType shape;

	public FsmColor color;

	[Tooltip("Use this for sphere gizmos")]
	public FsmFloat radius;

	[Tooltip("Use this for cube gizmos")]
	public FsmVector3 size;

	public override void Reset()
	{
		gameObject = null;
		shape = ShapeType.Sphere;
		color = Color.grey;
		radius = 1f;
		size = new Vector3(1f, 1f, 1f);
	}

	public override void OnDrawGizmos()
	{
		Transform transform = base.Fsm.GetOwnerDefaultTarget(gameObject).transform;
		if (!(transform == null))
		{
			Gizmos.color = color.Value;
			switch (shape)
			{
			case ShapeType.Sphere:
				Gizmos.DrawSphere(transform.position, radius.Value);
				break;
			case ShapeType.WireSphere:
				Gizmos.DrawWireSphere(transform.position, radius.Value);
				break;
			case ShapeType.Cube:
				Gizmos.DrawCube(transform.position, size.Value);
				break;
			case ShapeType.WireCube:
				Gizmos.DrawWireCube(transform.position, size.Value);
				break;
			}
		}
	}
}
