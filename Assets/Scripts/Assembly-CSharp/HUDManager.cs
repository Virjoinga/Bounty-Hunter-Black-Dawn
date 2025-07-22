using UnityEngine;

public class HUDManager : MonoBehaviour
{
	public enum Type
	{
		None = 0,
		Battle = 1,
		CoopBattle = 2,
		VSTDMBattle = 3
	}

	private enum HUDState
	{
		None = 0,
		Show = 1,
		Hide = 2
	}

	public static HUDManager instance;

	public GameObject m_StaticPanel;

	public JoystickManager m_Joystick;

	public HotKeyManager m_HotKeyManager;

	public InfoManager m_InfoManager;

	public ChatManager m_ChatManager;

	public OtherManager m_OtherManager;

	public DyingManager m_DyingManager;

	public VSHUDManager m_VSHUDManager;

	private HUDTemplate m_HUDTemplate;

	private Type mType;

	public UILabel m_fps;

	protected float frames;

	protected float updateInterval = 2f;

	protected float timeLeft;

	protected string fpsStr;

	protected float accum;

	public UILabel m_netWorkTime;

	public UILabel TMD_SCORE;

	private float mLastUpdateAimTime;

	private HUDState hudState;

	private bool bTest;

	private void Awake()
	{
		instance = this;
		CloseAll();
		mType = Type.None;
		UserStateHUD.GetInstance().Clear();
		if (!bTest)
		{
			m_fps.gameObject.SetActive(false);
			m_netWorkTime.gameObject.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		instance = null;
		Debug.Log("HUDManager destroy");
	}

	public void Invalidate()
	{
		CloseAll();
		mType = Type.None;
		m_HUDTemplate = null;
	}

	private void Update()
	{
		if (bTest)
		{
			timeLeft -= Time.deltaTime;
			accum += Time.timeScale / Time.deltaTime;
			frames += 1f;
			if (timeLeft <= 0f)
			{
				fpsStr = "FPS:" + accum / frames;
				frames = 0f;
				accum = 0f;
				timeLeft = updateInterval;
			}
			if (Time.time - mLastUpdateAimTime > 0.2f)
			{
				mLastUpdateAimTime = Time.time;
				m_fps.text = fpsStr;
				if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
				{
					m_netWorkTime.text = "NetWorkTime : " + TimeManager.GetInstance().NetworkTime;
				}
				if (TMD_SCORE != null && GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
				{
					VSTDMManager vSTDMManager = (VSTDMManager)GameApp.GetInstance().GetVSManager();
					if (vSTDMManager != null)
					{
						TMD_SCORE.text = "[FF0000]" + vSTDMManager.RedTeam.GetScore() + "[-] VS [0000FF]" + vSTDMManager.BlueTeam.GetScore() + "[-]";
					}
				}
			}
		}
		switch (hudState)
		{
		case HUDState.Show:
			hudState = HUDState.None;
			if (m_HUDTemplate != null)
			{
				m_HUDTemplate.Show();
			}
			break;
		case HUDState.Hide:
			hudState = HUDState.None;
			if (m_HUDTemplate != null)
			{
				m_HUDTemplate.Hide();
			}
			break;
		}
	}

	private void CloseAll()
	{
		m_Joystick.SetAllActiveRecursively(false);
		m_HotKeyManager.SetAllActiveRecursively(false);
		m_InfoManager.SetAllActiveRecursively(false);
		m_ChatManager.SetAllActiveRecursively(false);
		m_OtherManager.SetAllActiveRecursively(false);
		m_DyingManager.SetAllActiveRecursively(false);
		if (m_VSHUDManager != null)
		{
			m_VSHUDManager.SetAllActiveRecursively(false);
		}
	}

	public void LoadHUD(Type type)
	{
		CloseAll();
		if (m_HUDTemplate != null)
		{
			Object.Destroy(m_HUDTemplate);
		}
		m_HUDTemplate = null;
		m_VSHUDManager = null;
		switch (type)
		{
		case Type.Battle:
		{
			HUDBattle hUDBattle = base.gameObject.AddComponent<HUDBattle>() as HUDBattle;
			m_HotKeyManager.setListener(hUDBattle);
			m_HUDTemplate = hUDBattle;
			if (m_VSHUDManager != null)
			{
				Object.Destroy(m_VSHUDManager.gameObject);
			}
			break;
		}
		case Type.CoopBattle:
		{
			HUDCoopBattle hUDCoopBattle = base.gameObject.AddComponent<HUDCoopBattle>() as HUDCoopBattle;
			m_HotKeyManager.setListener(hUDCoopBattle);
			m_HUDTemplate = hUDCoopBattle;
			if (m_VSHUDManager != null)
			{
				Object.Destroy(m_VSHUDManager.gameObject);
			}
			break;
		}
		case Type.VSTDMBattle:
		{
			HUDVSTDMBattle hUDVSTDMBattle = base.gameObject.AddComponent<HUDVSTDMBattle>() as HUDVSTDMBattle;
			m_HotKeyManager.setListener(hUDVSTDMBattle);
			m_HUDTemplate = hUDVSTDMBattle;
			GameObject original = ResourceLoad.GetInstance().LoadUI("HUD", "HUDVSPlugin");
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.name = "VS";
			m_VSHUDManager = gameObject.GetComponent<VSHUDManager>();
			gameObject.transform.parent = m_StaticPanel.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localEulerAngles = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			break;
		}
		}
		mType = type;
	}

	public void Show()
	{
		hudState = HUDState.Show;
	}

	public void Close()
	{
		hudState = HUDState.Hide;
	}

	public Type GetRunningHUDType()
	{
		return mType;
	}

	private void OnSelectionChange(string name)
	{
		switch (name)
		{
		case "Low":
			Global.Priority = ThreadPriority.Low;
			break;
		case "BelowNormal":
			Global.Priority = ThreadPriority.BelowNormal;
			break;
		case "Normal":
			Global.Priority = ThreadPriority.Normal;
			break;
		case "High":
			Global.Priority = ThreadPriority.High;
			break;
		default:
			Global.Priority = ThreadPriority.Normal;
			break;
		}
	}
}
