using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Panel Contents Align")]
[ExecuteInEditMode]
public class UIDragPanelContentsAlign : MonoBehaviour
{
	public UIDraggablePanelAlign draggablePanel;

	[HideInInspector]
	[SerializeField]
	private UIPanel panel;

	private void Start()
	{
		if (panel != null)
		{
			if (draggablePanel == null)
			{
				draggablePanel = panel.GetComponent<UIDraggablePanelAlign>();
				if (draggablePanel == null)
				{
					draggablePanel = panel.gameObject.AddComponent<UIDraggablePanelAlign>();
				}
			}
			panel = null;
		}
		else if (draggablePanel == null)
		{
			draggablePanel = NGUITools.FindInParents<UIDraggablePanelAlign>(base.gameObject);
		}
	}

	private void OnPress(bool pressed)
	{
		if (base.enabled && base.gameObject.active && draggablePanel != null)
		{
			draggablePanel.Press(pressed);
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (base.enabled && base.gameObject.active && draggablePanel != null)
		{
			draggablePanel.Drag(delta);
		}
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && base.gameObject.active && draggablePanel != null)
		{
			draggablePanel.Scroll(delta);
		}
	}
}
