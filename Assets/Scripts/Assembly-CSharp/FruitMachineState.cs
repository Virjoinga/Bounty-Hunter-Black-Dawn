using UnityEngine;

public abstract class FruitMachineState
{
	public void Click(GameObject go)
	{
		OnClick(go);
	}

	public void Drag(GameObject go, Vector2 delta)
	{
		OnDrag(go, delta);
	}

	public void Press(GameObject go, bool isPressed)
	{
		OnPress(go, isPressed);
	}

	public void Start(FruitMachineBundle bundle)
	{
		OnStart(bundle);
	}

	public void Exit()
	{
		OnExit();
	}

	public void Update()
	{
		OnUpdate();
	}

	protected abstract void OnStart(FruitMachineBundle bundle);

	protected virtual void OnUpdate()
	{
	}

	protected virtual void OnExit()
	{
	}

	protected virtual void OnClick(GameObject go)
	{
	}

	protected virtual void OnDrag(GameObject go, Vector2 delta)
	{
	}

	protected virtual void OnPress(GameObject go, bool isPressed)
	{
	}
}
