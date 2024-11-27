using System;
using UnityEngine;

public abstract class UIDelegateMenu : MonoBehaviour
{
	protected void AddDelegate(GameObject _obj, out string name)
	{
		name = _obj.name;
		AddDelegate(_obj);
	}

	protected void AddDelegate(GameObject _obj)
	{
		UIEventListener uIEventListener = UIEventListener.Get(_obj);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickThumb));
		uIEventListener.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onDoubleClick, new UIEventListener.VoidDelegate(OnDoubleClickThumb));
		uIEventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onHover, new UIEventListener.BoolDelegate(OnHoverThumb));
		uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragThumb));
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressThumb));
	}

	protected void RemoveDelegate(GameObject _obj)
	{
		UIEventListener uIEventListener = UIEventListener.Get(_obj);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickThumb));
		uIEventListener.onDoubleClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onDoubleClick, new UIEventListener.VoidDelegate(OnDoubleClickThumb));
		uIEventListener.onHover = (UIEventListener.BoolDelegate)Delegate.Remove(uIEventListener.onHover, new UIEventListener.BoolDelegate(OnHoverThumb));
		uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Remove(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragThumb));
		uIEventListener.onPress = (UIEventListener.BoolDelegate)Delegate.Remove(uIEventListener.onPress, new UIEventListener.BoolDelegate(OnPressThumb));
	}

	protected bool IsThisObject(GameObject go, string name)
	{
		return go.name.Equals(name);
	}

	protected virtual void OnClickThumb(GameObject go)
	{
	}

	protected virtual void OnDoubleClickThumb(GameObject go)
	{
	}

	protected virtual void OnHoverThumb(GameObject go, bool isOver)
	{
	}

	protected virtual void OnDragThumb(GameObject go, Vector2 delta)
	{
	}

	protected virtual void OnPressThumb(GameObject go, bool isPressed)
	{
	}
}
