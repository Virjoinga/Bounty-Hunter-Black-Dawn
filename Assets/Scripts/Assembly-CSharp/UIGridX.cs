using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGridX : MonoBehaviour
{
	public enum Arrangement
	{
		Horizontal = 0,
		Vertical = 1
	}

	private float OFFSET;

	public Arrangement arrangement;

	public int maxPerLine;

	public float cellWidth = 200f;

	public float cellHeight = 200f;

	public bool repositionNow;

	public bool sorted;

	public bool hideInactive = true;

	public bool invert;

	public bool circle;

	public UIPanel m_UIPanel;

	public UIDraggablePanelAlign m_UIDraggablePanelAlign;

	private bool circleEnable = true;

	private ArrayList mGridArray;

	private Vector4 uiGridRange;

	private void Awake()
	{
		initGridArray();
	}

	private void Start()
	{
		Reposition();
	}

	private void initGridArray()
	{
		mGridArray = new ArrayList();
		Component[] componentsInChildren = GetComponentsInChildren<Component>();
		if (componentsInChildren != null)
		{
			Component[] array = componentsInChildren;
			foreach (Component component in array)
			{
				if (component is UIDragPanelContentsAlign)
				{
					mGridArray.Add(component);
				}
			}
		}
		if (mGridArray.Count < 2)
		{
			circleEnable = false;
		}
	}

	public int GetGridCount()
	{
		return mGridArray.Count;
	}

	private void LateUpdate()
	{
		if (repositionNow)
		{
			repositionNow = false;
			initGridArray();
			Reposition();
		}
		if (circle && circleEnable)
		{
			bool flag = false;
			while (!flag)
			{
				flag = Reposition(arrangement == Arrangement.Horizontal);
			}
		}
	}

	private bool Reposition(bool horizontal)
	{
		bool flag = false;
		Component component = mGridArray[0] as Component;
		Component component2 = mGridArray[0] as Component;
		Vector4 clipRange = m_UIPanel.clipRange;
		if (horizontal)
		{
			for (int i = 1; i < mGridArray.Count; i++)
			{
				Component component3 = mGridArray[i] as Component;
				if (component3.transform.localPosition.x < component.transform.localPosition.x)
				{
					component = component3;
				}
				else if (component3.transform.localPosition.x > component2.transform.localPosition.x)
				{
					component2 = component3;
				}
			}
			if (component.transform.localPosition.x - cellWidth / 2f > clipRange.x - clipRange.z / 2f - OFFSET)
			{
				component2.transform.localPosition = component.transform.localPosition - new Vector3(cellWidth, 0f, 0f);
			}
			else if (component2.transform.localPosition.x + cellWidth / 2f < clipRange.x + clipRange.z / 2f + OFFSET)
			{
				component.transform.localPosition = component2.transform.localPosition + new Vector3(cellWidth, 0f, 0f);
			}
			return true;
		}
		for (int j = 1; j < mGridArray.Count; j++)
		{
			Component component3 = mGridArray[j] as Component;
			if (component3.transform.localPosition.y < component.transform.localPosition.y)
			{
				component = component3;
			}
			else if (component3.transform.localPosition.y > component2.transform.localPosition.y)
			{
				component2 = component3;
			}
		}
		if (component.transform.localPosition.y - cellHeight / 2f > clipRange.y - clipRange.w / 2f - OFFSET)
		{
			component2.transform.localPosition = component.transform.localPosition - new Vector3(0f, cellHeight, 0f);
		}
		else if (component2.transform.localPosition.y + cellHeight / 2f < clipRange.y + clipRange.w / 2f + OFFSET)
		{
			component.transform.localPosition = component2.transform.localPosition + new Vector3(0f, cellHeight, 0f);
		}
		return true;
	}

	public static int SortByName(Transform a, Transform b)
	{
		return string.Compare(a.name, b.name);
	}

	public void Reposition()
	{
		Transform transform = base.transform;
		int num = 0;
		int num2 = 0;
		int num3 = ((!invert) ? 1 : (-1));
		if (sorted)
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < transform.childCount; i++)
			{
				list.Add(transform.GetChild(i));
			}
			list.Sort(SortByName);
			int j = 0;
			for (int count = list.Count; j < count; j++)
			{
				Transform transform2 = list[j];
				if (transform2.gameObject.activeSelf || !hideInactive)
				{
					transform2.localPosition = ((arrangement != 0) ? new Vector3(cellWidth * (float)num2 * (float)num3, (0f - cellHeight) * (float)num, 0f) : new Vector3(cellWidth * (float)num * (float)num3, (0f - cellHeight) * (float)num2, 0f));
					if (++num >= maxPerLine && maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < transform.childCount; k++)
			{
				Transform child = transform.GetChild(k);
				if (child.gameObject.activeSelf || !hideInactive)
				{
					child.localPosition = ((arrangement != 0) ? new Vector3(cellWidth * (float)num2 * (float)num3, (0f - cellHeight) * (float)num, 0f) : new Vector3(cellWidth * (float)num * (float)num3, (0f - cellHeight) * (float)num2, 0f));
					if (++num >= maxPerLine && maxPerLine > 0)
					{
						num = 0;
						num2++;
					}
				}
			}
		}
		UIDraggablePanel uIDraggablePanel = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
		if (uIDraggablePanel != null)
		{
			uIDraggablePanel.UpdateScrollbars(true);
		}
		if (m_UIPanel == null || m_UIDraggablePanelAlign == null)
		{
			return;
		}
		if (arrangement == Arrangement.Horizontal)
		{
			uiGridRange = new Vector4(m_UIDraggablePanelAlign.bounds.center.x, m_UIDraggablePanelAlign.bounds.center.y, cellWidth * (float)mGridArray.Count, cellHeight);
			if (uiGridRange.z <= m_UIPanel.clipRange.z + 10f)
			{
				circleEnable = false;
			}
		}
		else
		{
			uiGridRange = new Vector4(m_UIDraggablePanelAlign.bounds.center.x, m_UIDraggablePanelAlign.bounds.center.y, cellWidth, cellHeight * (float)mGridArray.Count);
			if (uiGridRange.w <= m_UIPanel.clipRange.w + 10f)
			{
				circleEnable = false;
			}
		}
	}
}
