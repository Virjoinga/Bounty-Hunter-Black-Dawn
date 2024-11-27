using UnityEngine;

public class UIMsg : MonoBehaviour
{
	public enum UIMsgButton
	{
		Ok = 0,
		Cancel = 1,
		DialogOver = 2
	}

	private class EmptyListener : UIMsgListener
	{
		public void OnButtonClick(UIMsg whichMsg, UIMsgButton buttonId)
		{
		}
	}

	public UILabel m_Information;

	private UIMsgListener listener;

	private BoxCollider m_block;

	private static UIMsgListener mEmpty;

	public static UIMsgListener EMPTY
	{
		get
		{
			return mEmpty;
		}
	}

	public byte EventId { get; set; }

	public bool IgnoreOthers { get; set; }

	public byte Type { get; set; }

	private void Awake()
	{
		mEmpty = new EmptyListener();
		if (listener == null)
		{
			listener = mEmpty;
		}
	}

	private void Start()
	{
		m_block = base.gameObject.GetComponent<Collider>() as BoxCollider;
		m_block.size = new Vector3(Screen.width, Screen.height, 0f);
	}

	public void SetListener(UIMsgListener listener)
	{
		if (listener != null)
		{
			this.listener = null;
			this.listener = listener;
		}
	}

	public void HandleEvent(UIMsgButton eventID)
	{
		listener.OnButtonClick(this, eventID);
	}
}
