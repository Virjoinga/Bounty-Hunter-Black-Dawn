using UnityEngine;

public class UIGameMenu : UIDelegateMenu, InGameMenuListener
{
	public const byte GAME_MENU_TYPE_NONE = 0;

	public const byte GAME_MENU_TYPE_STATE = 1;

	public const byte GAME_MENU_TYPE_CLOSE = 2;

	public const byte GAME_MENU_TYPE_MENU_BUTTON = 4;

	public const byte GAME_MENU_TYPE_SHOP_BUTTON = 8;

	private bool isCloseClick;

	private float mTimeStart;

	private float mTimeDelta;

	private float mActual;

	private bool mTimeStarted;

	private bool isCloseOnDestory;

	protected byte mMask = 3;

	public float realTimeDelta
	{
		get
		{
			return mTimeDelta;
		}
	}

	protected virtual void Awake()
	{
		InGameMenuManager.GetInstance().RemoveListener();
		InGameMenuManager.GetInstance().SetListener(this);
		Show(InitMask());
		UIMemoryManager.IncreaseMemoryClearCounter(1);
	}

	public void Show(byte mask)
	{
		InGameMenuManager.GetInstance().HideHUD(PauseGame());
		if (((uint)mask & (true ? 1u : 0u)) != 0)
		{
			InGameMenuManager.GetInstance().ShowStateBar();
		}
		if ((mask & 2u) != 0)
		{
			InGameMenuManager.GetInstance().ShowCloseButton();
		}
		if ((mask & 4u) != 0)
		{
			InGameMenuManager.GetInstance().ShowGameMenuButton();
		}
	}

	protected virtual byte InitMask()
	{
		return mMask;
	}

	protected virtual bool PauseGame()
	{
		return true;
	}

	protected virtual void OnEnable()
	{
		mTimeStarted = true;
		mTimeDelta = 0f;
		mTimeStart = Time.realtimeSinceStartup;
	}

	protected virtual void OnDestroy()
	{
		if (isCloseClick || isCloseOnDestory)
		{
			CloseGameMenu();
		}
	}

	private void CloseGameMenu()
	{
		InGameMenuManager.GetInstance().CloseStarBar();
		InGameMenuManager.GetInstance().CloseMenuButton();
		InGameMenuManager.GetInstance().CloseCloseButton();
		InGameMenuManager.GetInstance().ShowHUD();
		InGameMenuManager.GetInstance().RemoveListener(this);
		UIMemoryManager.CheckMemoryClear();
	}

	public virtual void OnCloseButtonClick()
	{
		isCloseClick = true;
	}

	protected float UpdateRealTimeDelta()
	{
		if (mTimeStarted)
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float b = realtimeSinceStartup - mTimeStart;
			mActual += Mathf.Max(0f, b);
			mTimeDelta = 0.001f * Mathf.Round(mActual * 1000f);
			mActual -= mTimeDelta;
			mTimeStart = realtimeSinceStartup;
		}
		else
		{
			mTimeStarted = true;
			mTimeStart = Time.realtimeSinceStartup;
			mTimeDelta = 0f;
		}
		return mTimeDelta;
	}

	protected void SetMenuCloseOnDestroy(bool result)
	{
		isCloseOnDestory = result;
	}
}
