using System;
using UnityEngine;

public class StateIcon : MonoBehaviour
{
	public float radius = 50f;

	public float widthFactor = 1f;

	public bool reverseHorizontal;

	public bool reverseVetical = true;

	private Vector2 iniPos;

	private float initAngle;

	private UISprite sprite;

	public UserStateHUD.GameUnitHUD unit { get; set; }

	private void Awake()
	{
		sprite = base.gameObject.GetComponentInChildren<UISprite>();
		iniPos = new Vector2(base.transform.localPosition.x, base.transform.localPosition.y);
		initAngle = base.transform.eulerAngles.z;
	}

	public void UpdatePosition(float angle)
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, angle * (float)((!reverseHorizontal) ? 1 : (-1)) * (float)(reverseVetical ? 1 : (-1)) + initAngle);
		angle = angle * (float)Math.PI / 180f;
		base.transform.localPosition = new Vector3(radius * Mathf.Sin(angle) * widthFactor * (float)((!reverseHorizontal) ? 1 : (-1)) + iniPos.x, radius * Mathf.Cos(angle) * (float)((!reverseVetical) ? 1 : (-1)) + iniPos.y, base.transform.localPosition.z);
	}

	public void SetColor(Color color)
	{
		sprite.color = color;
	}
}
