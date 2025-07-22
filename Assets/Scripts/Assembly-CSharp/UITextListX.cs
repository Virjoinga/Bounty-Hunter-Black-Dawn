using UnityEngine;

public class UITextListX : UITextList
{
	public enum ShowCondition
	{
		Always = 0,
		OnlyIfNeeded = 1,
		WhenDragging = 2
	}

	public GameObject receiver;

	public string functionName;

	public bool supportPageDown;

	public bool supportDrag;

	public float dragSensitivity = 2f;

	private bool isAtTheBottom;

	public UIScrollBar verticalScrollBar;

	public ShowCondition showScrollBars = ShowCondition.OnlyIfNeeded;

	private int mTouches;

	public bool shouldMoveVertically
	{
		get
		{
			int num = ((!(maxHeight > 0f)) ? 100000 : Mathf.FloorToInt(maxHeight / textLabel.cachedTransform.localScale.y));
			return mTotalLines > num;
		}
	}

	private void Start()
	{
		if (verticalScrollBar != null)
		{
			verticalScrollBar.alpha = ((showScrollBars != 0) ? 0f : 1f);
		}
	}

	private void OnPress(bool pressed)
	{
		mTouches += (pressed ? 1 : (-1));
	}

	private void OnClick()
	{
		if (supportPageDown)
		{
			int num = ((!(maxHeight > 0f)) ? 100000 : Mathf.FloorToInt(maxHeight / textLabel.cachedTransform.localScale.y));
			mScroll = Mathf.Max(0f, mScroll + (float)num);
			UpdateVisibleText();
			if (verticalScrollBar != null)
			{
				UpdateScrollbars();
			}
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (supportDrag)
		{
			mScroll = Mathf.Max(0f, mScroll + delta.y / dragSensitivity);
			UpdateVisibleText();
			if (verticalScrollBar != null)
			{
				UpdateScrollbars();
			}
		}
	}

	protected new void UpdateVisibleText()
	{
		base.UpdateVisibleText();
		if (supportPageDown)
		{
			int num = ((!(maxHeight > 0f)) ? 100000 : Mathf.FloorToInt(maxHeight / textLabel.cachedTransform.localScale.y));
			bool flag = isAtTheBottom;
			if (mScroll + (float)num >= (float)mTotalLines)
			{
				isAtTheBottom = true;
			}
			else
			{
				isAtTheBottom = false;
			}
		}
		if (verticalScrollBar != null)
		{
			UpdateScrollbars();
		}
	}

	public void UpdateScrollbars()
	{
		int num = ((!(maxHeight > 0f)) ? 100000 : Mathf.FloorToInt(maxHeight / textLabel.cachedTransform.localScale.y));
		float num2 = mTotalLines - num;
		verticalScrollBar.scrollValue = ((!(num2 > 0.001f)) ? 0f : (mScroll / num2));
	}

	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		if (showScrollBars == ShowCondition.Always || !shouldMoveVertically)
		{
			return;
		}
		bool flag = false;
		if (showScrollBars != ShowCondition.WhenDragging || mTouches > 0)
		{
			flag = true;
		}
		if ((bool)verticalScrollBar)
		{
			float alpha = verticalScrollBar.alpha;
			alpha += ((!flag) ? ((0f - deltaTime) * 3f) : (deltaTime * 6f));
			alpha = Mathf.Clamp01(alpha);
			if (verticalScrollBar.alpha != alpha)
			{
				verticalScrollBar.alpha = alpha;
			}
		}
	}
}
