using UnityEngine;

public abstract class HUDTemplate : MonoBehaviour
{
	private bool bRun;

	private void Start()
	{
		OnInit();
	}

	private void OnEnable()
	{
		Show();
	}

	private void Update()
	{
		if (bRun)
		{
			OnUpdate();
		}
	}

	private void OnDisable()
	{
		Hide();
	}

	private void OnDestroy()
	{
		OnClose();
	}

	public void Show()
	{
		if (!bRun)
		{
			bRun = true;
			OnShow();
		}
	}

	public void Hide()
	{
		if (bRun)
		{
			bRun = false;
			OnHide();
		}
	}

	protected abstract void OnShow();

	protected abstract void OnHide();

	protected abstract void OnInit();

	protected abstract void OnUpdate();

	protected abstract void OnClose();
}
