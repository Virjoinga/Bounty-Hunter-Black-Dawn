using System;
using System.Collections.Generic;
using UnityEngine;

public class MapController : UIDelegateMenu
{
	public const string TAG_GAMBLE = "Gamble";

	public const string TAG_BAR = "Bar";

	public const string TAG_UPGRADE_MACHINE = "UpgradeMachine";

	public GameObject m_ZoomIn;

	public GameObject m_ZoomOut;

	public GameObject m_OffOn;

	public GameObject m_Info;

	public UISlider m_Slider;

	public Transform m_Background;

	public GameObject m_PlayerContainer;

	public GameObject m_RemotePlayerContainer;

	public GameObject m_MiniMap;

	public GameObject m_Icon;

	public GameObject m_Portal;

	public GameObject m_MainQuest;

	public GameObject m_SubQuest;

	public GameObject m_Multiplay;

	public GameObject m_Store;

	public GameObject m_Gameble;

	public GameObject m_UpgradeMachine;

	private UISprite[] m_Player;

	private UISprite[] m_RemotePlayer;

	public float minSize = 0.5f;

	public float maxSize = 2f;

	private float screenWidth;

	private float screenHeight;

	private float mapWidth;

	private float mapHeight;

	private float xFactor;

	private float yFactor;

	private float xConst;

	private float yConst;

	private Vector3 topRight;

	private Vector3 topLeft;

	private float mLastUpdateTime;

	private void Awake()
	{
		m_Portal.SetActive(false);
		m_MainQuest.SetActive(false);
		m_SubQuest.SetActive(false);
		m_Multiplay.SetActive(false);
		m_Store.SetActive(false);
		m_Gameble.SetActive(false);
		m_UpgradeMachine.SetActive(false);
		m_Player = m_PlayerContainer.GetComponentsInChildren<UISprite>();
		m_RemotePlayer = m_RemotePlayerContainer.GetComponentsInChildren<UISprite>();
		UISprite[] player = m_Player;
		foreach (UISprite uISprite in player)
		{
			uISprite.gameObject.SetActive(false);
		}
		UISprite[] remotePlayer = m_RemotePlayer;
		foreach (UISprite uISprite2 in remotePlayer)
		{
			uISprite2.gameObject.SetActive(false);
		}
		UISlider slider = m_Slider;
		slider.onValueChange = (UISlider.OnValueChange)Delegate.Combine(slider.onValueChange, new UISlider.OnValueChange(OnScrollBarChange));
		AddDelegate(m_ZoomIn);
		AddDelegate(m_ZoomOut);
		AddDelegate(m_OffOn);
		initMap();
		initSetting();
	}

	private void initMap()
	{
		SceneConfig currentSceneConfig = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetCurrentSceneConfig();
		if (m_MiniMap.transform.GetChildCount() > 0)
		{
			UnityEngine.Object.Destroy(m_MiniMap.transform.GetChild(0).gameObject);
		}
		m_MiniMap.transform.localScale = currentSceneConfig.MiniMapSize;
		GameObject original = Resources.Load("MiniMapX/Res/P_" + currentSceneConfig.SceneFileName) as GameObject;
		GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = m_MiniMap.transform;
		gameObject.transform.position = Vector3.zero;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.transform.localScale = Vector3.one;
		xFactor = currentSceneConfig.MiniMapSize.x / (currentSceneConfig.MapTopRight.x - currentSceneConfig.MapTopLeft.x);
		yFactor = currentSceneConfig.MiniMapSize.y / (currentSceneConfig.MapTopRight.z - currentSceneConfig.MapBottomRight.z);
		xConst = currentSceneConfig.MiniMapSize.x / 2f - currentSceneConfig.MapTopRight.x * currentSceneConfig.MiniMapSize.x / (currentSceneConfig.MapTopRight.x - currentSceneConfig.MapTopLeft.x);
		yConst = currentSceneConfig.MiniMapSize.y / 2f - currentSceneConfig.MapTopRight.z * currentSceneConfig.MiniMapSize.y / (currentSceneConfig.MapTopRight.z - currentSceneConfig.MapBottomRight.z);
		topRight = currentSceneConfig.MapTopRight;
		topLeft = currentSceneConfig.MapTopLeft;
	}

	private void initSetting()
	{
		screenHeight = NGUITools.FindInParents<UIRoot>(base.gameObject).activeHeight;
		screenWidth = (float)Screen.width * screenHeight / (float)Screen.height;
		float num = m_Background.localScale.x - 220f;
		float num2 = m_Background.localScale.y - 20f;
		float num3 = num / screenWidth;
		float height = num2 / screenHeight;
		float num4 = num2 / m_Background.localScale.y * (float)UIRatio.BASE_SCREEN_HEIGHT / screenHeight;
		base.GetComponent<Camera>().rect = new Rect((1f - num3) / 2f, (1f - num4) / 2f, num3, height);
		MiniMap component = m_MiniMap.GetComponent<MiniMap>();
		component.onFingerDrag = OnFingerDrag;
		component.onFingerStretch = OnFingerStretch;
		float num5 = Screen.width / 2;
		float num6 = UIRatio.BASE_SCREEN_HEIGHT / 2;
		Transform parent = m_Background.transform;
		while (parent != null && parent.gameObject.GetComponent<UIRoot>() == null)
		{
			num6 += parent.localPosition.y;
			parent = parent.parent;
		}
		num6 = num6 * (float)Screen.height / screenHeight;
		float num7 = num * (float)Screen.height / screenHeight;
		float num8 = num2 * (float)Screen.height / screenHeight;
		component.Range = new Vector4(num5 - num7 / 2f + 100f, num6 - num8 / 2f, num7 - 100f, num8);
		mapWidth = m_MiniMap.transform.localScale.x;
		mapHeight = m_MiniMap.transform.localScale.y;
		maxSize = Mathf.Min(maxSize, mapWidth / (screenWidth * base.GetComponent<Camera>().rect.width / base.GetComponent<Camera>().rect.height), mapHeight / screenHeight);
		base.GetComponent<Camera>().orthographicSize = maxSize;
	}

	private void Start()
	{
		LocatePlayer();
		GameObject gameObject = m_Player[GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetSeatID()].gameObject;
		base.transform.localPosition = gameObject.transform.localPosition;
		Reposition();
		LocateStore();
		LocateMultiplay();
		LocatePortal();
		LocateQuest();
		LocateGamble();
		LocateUpgradeMachine();
		LocateAccQuest();
	}

	private void Update()
	{
		if (Time.time - mLastUpdateTime > 0.05f)
		{
			mLastUpdateTime = Time.time;
			LocateRemotePlayer();
		}
	}

	private void Reposition()
	{
		base.GetComponent<Camera>().orthographicSize = Mathf.Clamp(base.GetComponent<Camera>().orthographicSize, minSize, maxSize);
		m_Slider.sliderValue = (base.GetComponent<Camera>().orthographicSize - minSize) / (maxSize - minSize);
		float num = Mathf.Abs((screenWidth * base.GetComponent<Camera>().rect.width / base.GetComponent<Camera>().rect.height * base.GetComponent<Camera>().orthographicSize - mapWidth) * 0.5f);
		float num2 = Mathf.Abs((screenHeight * base.GetComponent<Camera>().orthographicSize - mapHeight) * 0.5f);
		float x = Mathf.Clamp(base.transform.localPosition.x, 0f - num, num);
		float y = Mathf.Clamp(base.transform.localPosition.y, 0f - num2, num2);
		base.transform.localPosition = new Vector3(x, y, base.transform.localPosition.z);
	}

	private GameObject DuplicateGameObject(GameObject go)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(go) as GameObject;
		gameObject.SetActive(true);
		gameObject.transform.parent = m_Icon.transform;
		gameObject.transform.localScale = go.transform.localScale;
		gameObject.transform.rotation = go.transform.rotation;
		gameObject.transform.position = go.transform.position;
		return gameObject;
	}

	private void LocateRemotePlayer()
	{
		Dictionary<string, UserStateHUD.GameUnitHUD> remotePlayerList = UserStateHUD.GetInstance().GetRemotePlayerList();
		bool[] array = new bool[m_RemotePlayer.Length];
		foreach (KeyValuePair<string, UserStateHUD.GameUnitHUD> item in remotePlayerList)
		{
			GameObject gameObject = m_RemotePlayer[item.Value.IconIndex].gameObject;
			array[item.Value.IconIndex] = true;
			LocateObject(item.Value.Transform, gameObject.transform, true);
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] && !m_RemotePlayer[i].gameObject.activeSelf)
			{
				NGUITools.SetActive(m_RemotePlayer[i].gameObject, true);
			}
			else if (!array[i] && m_RemotePlayer[i].gameObject.activeSelf)
			{
				NGUITools.SetActive(m_RemotePlayer[i].gameObject, false);
			}
		}
	}

	private void LocatePortal()
	{
		List<GameObject> portal = GameApp.GetInstance().GetGameScene().GetPortal();
		if (portal == null)
		{
			return;
		}
		foreach (GameObject item in portal)
		{
			LocateObject(item.transform, DuplicateGameObject(m_Portal).transform, false);
		}
	}

	private void LocateStore()
	{
		GameObject store = GameApp.GetInstance().GetGameScene().GetStore();
		if (store != null)
		{
			LocateObject(store.transform, DuplicateGameObject(m_Store).transform, false);
		}
	}

	private void LocateMultiplay()
	{
		GameObject[] multiplay = GameApp.GetInstance().GetGameScene().GetMultiplay();
		if (multiplay != null)
		{
			GameObject[] array = multiplay;
			foreach (GameObject gameObject in array)
			{
				LocateObject(gameObject.transform, DuplicateGameObject(m_Multiplay).transform, false);
			}
		}
	}

	private void LocateGamble()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Gamble");
		if (array != null)
		{
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				LocateObject(gameObject.transform, DuplicateGameObject(m_Gameble).transform, false);
			}
		}
	}

	private void LocateUpgradeMachine()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("UpgradeMachine");
		if (array != null)
		{
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				LocateObject(gameObject.transform, DuplicateGameObject(m_UpgradeMachine).transform, false);
			}
		}
	}

	private void LocateAccQuest()
	{
		if (!GameApp.GetInstance().GetGameWorld().IsCityScene())
		{
			return;
		}
		Dictionary<short, Npc> npc = GameApp.GetInstance().GetGameScene().GetNpc();
		foreach (KeyValuePair<short, Npc> item in npc)
		{
			QuestScript questScript = item.Value.GetQuestScript();
			if (questScript != null && questScript.GetPromptType() == PromptType.Assignable)
			{
				LocateObject(item.Value.GetTransform(), DuplicateGameObject(m_MainQuest).transform, false);
			}
		}
	}

	private void LocateQuest()
	{
		Dictionary<string, QuestPoint> dictionary = null;
		GameObject gameObject = null;
		if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
		{
			dictionary = GameApp.GetInstance().GetUserState().m_questStateContainer.GetMarkQuestPoint();
			gameObject = ((GameApp.GetInstance().GetUserState().m_questStateContainer.GetMarkQuestAttr() != QuestAttr.MAIN_QUEST) ? m_SubQuest : m_MainQuest);
		}
		else
		{
			dictionary = GameApp.GetInstance().GetUserState().m_questStateContainer.GetCurrentQuestPoint();
			gameObject = ((GameApp.GetInstance().GetUserState().m_questStateContainer.GetCurrentQuestAttr() != QuestAttr.MAIN_QUEST) ? m_SubQuest : m_MainQuest);
		}
		if (dictionary == null)
		{
			return;
		}
		bool flag = false;
		foreach (KeyValuePair<string, QuestPoint> item in dictionary)
		{
			if (GameApp.GetInstance().GetGameWorld().CurrentSceneID == item.Value.m_siteId)
			{
				foreach (QuestPoint.QuestPosition po in item.Value.GetPos())
				{
					if (po.m_state == QuestPointState.enable)
					{
						LocateObject(po.m_pos, DuplicateGameObject(gameObject).transform);
					}
				}
			}
			else
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return;
		}
		List<GameObject> portal = GameApp.GetInstance().GetGameScene().GetPortal();
		if (portal.Count <= 0)
		{
			return;
		}
		float num = 1000000f;
		GameObject gameObject2 = null;
		foreach (GameObject item2 in portal)
		{
			float num2 = Vector3.Distance(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetPosition(), item2.transform.position);
			if (num2 < num)
			{
				num = num2;
				gameObject2 = item2;
			}
		}
		LocateObject(gameObject2.transform, DuplicateGameObject(gameObject).transform, false);
	}

	private void LocatePlayer()
	{
		GameObject gameObject = m_Player[GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetSeatID()].gameObject;
		gameObject.SetActive(true);
		LocateObject(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetTransform(), gameObject.transform, true);
	}

	private void LocateObject(Transform trans, Transform target, bool rotate)
	{
		if (rotate)
		{
			Vector3 forward = trans.forward;
			Vector3 normalized = ((topRight + topLeft) / 2f - trans.position).normalized;
			float angleBetweenHorizontal = MathUtil.GetAngleBetweenHorizontal(forward, normalized);
			target.eulerAngles = new Vector3(0f, 0f, angleBetweenHorizontal);
		}
		LocateObject(new Vector2(trans.position.x, trans.position.z), target);
	}

	private void LocateObject(Vector2 pos, Transform target)
	{
		float x = xConst + xFactor * pos.x;
		float y = yConst + yFactor * pos.y;
		target.localPosition = new Vector3(x, y, 0f);
	}

	private void OnFingerDrag(Vector2 delta)
	{
		base.transform.localPosition -= new Vector3(delta.x, delta.y, 0f);
		Reposition();
	}

	private void OnFingerStretch(float delta)
	{
		base.GetComponent<Camera>().orthographicSize -= delta / 8f;
		Reposition();
	}

	protected override void OnClickThumb(GameObject go)
	{
		if (go.Equals(m_ZoomIn))
		{
			base.GetComponent<Camera>().orthographicSize -= 0.1f;
			Reposition();
		}
		else if (go.Equals(m_ZoomOut))
		{
			base.GetComponent<Camera>().orthographicSize += 0.1f;
			Reposition();
		}
		else if (go.Equals(m_OffOn))
		{
			NGUITools.SetActive(m_Info, !m_Info.activeSelf);
			UITweener[] componentsInChildren = m_Info.GetComponentsInChildren<UITweener>();
			UITweener[] array = componentsInChildren;
			foreach (UITweener uITweener in array)
			{
				uITweener.Reset();
			}
		}
	}

	private void OnScrollBarChange(float value)
	{
		Debug.Log("value : " + value);
		base.GetComponent<Camera>().orthographicSize = minSize + (maxSize - minSize) * value;
		Reposition();
	}
}
