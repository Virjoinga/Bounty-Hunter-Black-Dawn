using UnityEngine;

public class UIFrameManager
{
	private static UIFrameManager instance;

	private UIFrameManager()
	{
	}

	public static UIFrameManager GetInstance()
	{
		if (instance == null)
		{
			instance = new UIFrameManager();
		}
		return instance;
	}

	public GameObject CreateFrame(GameObject icon, int id)
	{
		return CreateFrame(icon, Vector2.zero, -1f, id);
	}

	public GameObject CreateFrame(GameObject icon, Vector2 inc, int id)
	{
		return CreateFrame(icon, inc, -1f, id);
	}

	public GameObject CreateFrame(GameObject icon, float depth, int id)
	{
		return CreateFrame(icon, Vector2.zero, depth, id);
	}

	public GameObject CreateFrame(GameObject icon)
	{
		return CreateFrame(icon, Vector2.zero, -1f, 1);
	}

	public GameObject CreateFrame(GameObject icon, Vector2 inc)
	{
		return CreateFrame(icon, inc, -1f, 1);
	}

	public GameObject CreateFrame(GameObject icon, float depth)
	{
		return CreateFrame(icon, Vector2.zero, depth, 1);
	}

	public GameObject CreateFrame(GameObject icon, Vector2 inc, float depth)
	{
		return CreateFrame(icon, inc, depth, 1);
	}

	public GameObject CreateFrame(GameObject icon, Vector2 inc, float depth, int id)
	{
		if (IsIdCorrect(id))
		{
			GameObject original = ResourceLoad.GetInstance().LoadUI("Frame", "Frame" + id);
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			UIFrame component = gameObject.GetComponent<UIFrame>();
			UIWidget widget = GetWidget(icon);
			if (widget != null)
			{
				component.mSprite.pivot = widget.pivot;
				float num = 0f;
				float num2 = 0f;
				float x = inc.x;
				float y = inc.y;
				if (component.mSprite.pivot == UIWidget.Pivot.Top || component.mSprite.pivot == UIWidget.Pivot.TopLeft || component.mSprite.pivot == UIWidget.Pivot.TopRight)
				{
					num2 = inc.y / 2f;
					y = inc.y;
				}
				if (component.mSprite.pivot == UIWidget.Pivot.Bottom || component.mSprite.pivot == UIWidget.Pivot.BottomLeft || component.mSprite.pivot == UIWidget.Pivot.BottomRight)
				{
					num2 = inc.y / 2f;
					y = inc.y;
				}
				if (component.mSprite.pivot == UIWidget.Pivot.Left || component.mSprite.pivot == UIWidget.Pivot.TopLeft || component.mSprite.pivot == UIWidget.Pivot.BottomLeft)
				{
					num = inc.x / 2f;
					x = inc.x;
				}
				if (component.mSprite.pivot == UIWidget.Pivot.Right || component.mSprite.pivot == UIWidget.Pivot.TopRight || component.mSprite.pivot == UIWidget.Pivot.BottomRight)
				{
					num = inc.x / 2f;
					x = inc.x;
				}
				component.mSprite.transform.localPosition = new Vector3(widget.transform.localPosition.x + num, widget.transform.localPosition.y + num2, widget.transform.localPosition.z + depth);
				component.mSprite.transform.localEulerAngles = widget.transform.localEulerAngles;
				component.mSprite.transform.localScale = new Vector3(widget.transform.localScale.x + x, widget.transform.localScale.y + y, widget.transform.localScale.z);
			}
			gameObject.transform.parent = icon.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			UIPanel component2 = gameObject.GetComponent<UIPanel>();
			if (component2 != null)
			{
				Object.Destroy(component2);
			}
			return gameObject;
		}
		Debug.Log("The Frame" + id + " doesn't exist.");
		return null;
	}

	private bool IsIdCorrect(int id)
	{
		return id >= 1 && id <= 3;
	}

	public void DeleteFrame(GameObject icon)
	{
		UIFrame[] componentsInChildren = icon.GetComponentsInChildren<UIFrame>();
		if (componentsInChildren != null)
		{
			UIFrame[] array = componentsInChildren;
			foreach (UIFrame uIFrame in array)
			{
				Object.Destroy(uIFrame.gameObject);
			}
		}
	}

	private UIWidget GetWidget(GameObject icon)
	{
		UIWidget result = null;
		Transform[] componentsInChildren = icon.GetComponentsInChildren<Transform>();
		float num = 0f;
		if (componentsInChildren != null)
		{
			Transform[] array = componentsInChildren;
			foreach (Transform transform in array)
			{
				if (transform.localScale.x * transform.localScale.y > num)
				{
					UIWidget component = transform.gameObject.GetComponent<UIWidget>();
					if (component != null && component.transform.parent != null && component.transform.parent.gameObject.GetComponent<UIFrame>() == null)
					{
						num = transform.localScale.x * transform.localScale.y;
						result = component;
					}
				}
			}
		}
		return result;
	}
}
