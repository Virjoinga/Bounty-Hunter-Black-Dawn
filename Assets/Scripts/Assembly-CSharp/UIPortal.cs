using System.Collections.Generic;
using UnityEngine;

public class UIPortal : UIGameMenu
{
	public class SceneData
	{
		private List<Vector3> mQuestPosList;

		private List<InstancePortalConfig> mPortalList;

		private SceneConfig mSceneConfig;

		private bool bQuestMain;

		private bool mLock;

		public byte FatherSceneID
		{
			get
			{
				return mSceneConfig.FatherSceneID;
			}
		}

		public byte SceneID
		{
			get
			{
				return mSceneConfig.SceneID;
			}
		}

		public bool IsArena
		{
			get
			{
				return Arena.GetInstance().IsArena(mSceneConfig.SceneID);
			}
		}

		public string ButtonName
		{
			get
			{
				return mSceneConfig.SceneName;
			}
		}

		public string SceneIntroImgName
		{
			get
			{
				return mSceneConfig.SceneFileName;
			}
		}

		public string SceneIntroTxt
		{
			get
			{
				return mSceneConfig.SceneIntro;
			}
		}

		public string SceneName
		{
			get
			{
				return mSceneConfig.SceneFileName;
			}
		}

		public bool IsSubScene
		{
			get
			{
				return mSceneConfig.FatherSceneID != mSceneConfig.SceneID;
			}
		}

		public int AreaID
		{
			get
			{
				return mSceneConfig.AreaID;
			}
		}

		public int CurrentPortalIndex
		{
			get
			{
				if (CurrentIndexInList > mPortalList.Count - 1)
				{
					return -1;
				}
				return mPortalList[CurrentIndexInList].Index;
			}
		}

		public bool IsMainQuest
		{
			get
			{
				return bQuestMain;
			}
		}

		public bool IsLock
		{
			get
			{
				return mLock;
			}
		}

		public bool Hide
		{
			get
			{
				return mSceneConfig.Hide;
			}
		}

		public int CurrentIndexInList { get; set; }

		public SceneData(SceneConfig sc, bool isLock)
		{
			if (sc == null)
			{
				mSceneConfig = new SceneConfig();
			}
			else
			{
				mSceneConfig = sc;
			}
			if (GameConfig.GetInstance().instacePortalConfig.ContainsKey(mSceneConfig.SceneID))
			{
				mPortalList = GameConfig.GetInstance().instacePortalConfig[mSceneConfig.SceneID];
			}
			else
			{
				mPortalList = new List<InstancePortalConfig>();
			}
			mQuestPosList = new List<Vector3>();
			Dictionary<string, QuestPoint> dictionary = null;
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				dictionary = GameApp.GetInstance().GetUserState().m_questStateContainer.GetMarkQuestPoint();
				bQuestMain = GameApp.GetInstance().GetUserState().m_questStateContainer.GetMarkQuestAttr() == QuestAttr.MAIN_QUEST;
			}
			else
			{
				dictionary = GameApp.GetInstance().GetUserState().m_questStateContainer.GetCurrentQuestPoint();
				bQuestMain = GameApp.GetInstance().GetUserState().m_questStateContainer.GetCurrentQuestAttr() == QuestAttr.MAIN_QUEST;
			}
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, QuestPoint> item in dictionary)
				{
					if (mSceneConfig.SceneID != item.Value.m_siteId)
					{
						continue;
					}
					foreach (QuestPoint.QuestPosition po in item.Value.GetPos())
					{
						if (po.m_state == QuestPointState.enable)
						{
							mQuestPosList.Add(new Vector3(po.m_pos.x, 0f, po.m_pos.y));
						}
					}
				}
				float num = 100000f;
				for (int i = 0; i < mPortalList.Count; i++)
				{
					foreach (Vector3 mQuestPos in mQuestPosList)
					{
						float num2 = Vector3.Distance(mQuestPos, mPortalList[i].Pos);
						if (num2 < num)
						{
							num = num2;
							CurrentIndexInList = i;
						}
					}
				}
			}
			mLock = isLock;
		}

		public List<Vector2> GetQuestPosListInUI(Vector2 uiSize, Vector2 uiBottomLeft)
		{
			List<Vector2> list = new List<Vector2>();
			foreach (Vector3 mQuestPos in mQuestPosList)
			{
				list.Add(transformPos(mQuestPos, uiSize, uiBottomLeft));
			}
			return list;
		}

		public List<Vector2> GetPortalListInUI(Vector2 uiSize, Vector2 uiBottomLeft)
		{
			List<Vector2> list = new List<Vector2>();
			foreach (InstancePortalConfig mPortal in mPortalList)
			{
				list.Add(transformPos(mPortal.Pos, uiSize, uiBottomLeft));
			}
			return list;
		}

		public Vector2 transformPos(Vector3 targetPos, Vector2 uiSize, Vector2 uiBottomLeft)
		{
			float num = mSceneConfig.MapBottomLeft.x - mSceneConfig.MapTopRight.x;
			float num2 = mSceneConfig.MapBottomLeft.z - mSceneConfig.MapTopRight.z;
			float x = (mSceneConfig.MapBottomLeft.x - targetPos.x) * uiSize.x / num;
			float y = (mSceneConfig.MapBottomLeft.z - targetPos.z) * uiSize.y / num2;
			Vector2 vector = new Vector2(x, y);
			return uiBottomLeft + vector;
		}

		public Vector2 transformPos(Vector3 targetPos, Vector2 uiSize)
		{
			return transformPos(targetPos, uiSize, Vector2.zero);
		}
	}

	private class SceneInfo
	{
		public GameObject m_ButtonObject;

		public SceneData m_SceneData;
	}

	public class AreaData
	{
		private AreaConfig mAreaConfig;

		public string Name
		{
			get
			{
				return mAreaConfig.Name;
			}
		}

		public int Id
		{
			get
			{
				return mAreaConfig.Id;
			}
		}

		public AreaData(AreaConfig ac)
		{
			if (ac == null)
			{
				mAreaConfig = new AreaConfig();
			}
			else
			{
				mAreaConfig = ac;
			}
		}

		public bool Unlock(List<SceneData> list)
		{
			bool result = false;
			foreach (SceneData item in list)
			{
				if (item.AreaID == Id && !item.IsLock)
				{
					result = true;
				}
			}
			return result;
		}
	}

	private class AreaInfo
	{
		public GameObject m_ButtonObject;

		public AreaData m_AreaData;
	}

	private const float MAX_DRAGPANEL_HEIGHT = 450f;

	public GameObject m_QuestMark;

	public GameObject m_AreaButton;

	public GameObject m_LockAreaButton;

	public GameObject m_CityButton;

	public GameObject m_InstanceButton;

	public GameObject m_LockInstanceButton;

	public UITable m_Table;

	public DetailsButton m_DetailsButton;

	public UILabel m_LabelUnlockCon;

	public GameObject m_Map;

	public GameObject m_EnterButton;

	public GameObject m_CloseButton;

	public GameObject m_BossRushButton;

	private GameObject m_MapImage;

	private List<AreaInfo> mAreaInfoList;

	private List<AreaData> mAreaDataList;

	private List<SceneInfo> mSceneInfoList;

	private List<SceneData> mSceneDatalist;

	public GameObject m_CityPortalMark;

	public GameObject m_InstancePortalMark;

	public GameObject m_PortalMarkContainer;

	private List<GameObject> m_PortalMarkList;

	public GameObject m_MainQuestPosMark;

	public GameObject m_SubQuestPosMark;

	public GameObject m_QuestPosMarkContainer;

	private List<GameObject> m_QuestPosMarkList;

	private int curIndex;

	private int questIndex;

	private static int lastIndex;

	private static bool isShow;

	protected override void Awake()
	{
		base.Awake();
		SetMenuCloseOnDestroy(true);
		m_QuestMark.SetActive(false);
		m_AreaButton.SetActive(false);
		m_LockAreaButton.SetActive(false);
		m_CityButton.SetActive(false);
		m_InstanceButton.SetActive(false);
		m_CityPortalMark.SetActive(false);
		m_InstancePortalMark.SetActive(false);
		m_MainQuestPosMark.SetActive(false);
		m_SubQuestPosMark.SetActive(false);
		curIndex = -1;
		questIndex = -1;
		lastIndex = GameApp.GetInstance().GetUserState().GetLastPortalIndex();
		AddDelegate(m_EnterButton);
		AddDelegate(m_CloseButton);
		AddDelegate(m_BossRushButton);
	}

	private void Start()
	{
		SetAllData();
		InitAreaInfoList();
		InitSceneInfoList();
	}

	protected override byte InitMask()
	{
		return 2;
	}

	private List<AreaInfo> GetAreaInfoList(List<AreaData> adl, List<SceneData> sdl)
	{
		List<AreaInfo> list = new List<AreaInfo>();
		for (int i = 0; i < adl.Count; i++)
		{
			GameObject gameObject = null;
			if (adl[i].Unlock(sdl))
			{
				gameObject = Object.Instantiate(m_AreaButton) as GameObject;
				UIPortalAreaButton component = gameObject.GetComponent<UIPortalAreaButton>();
				component.SetName(adl[i].Name);
				component.SetNumber(adl[i].Id);
			}
			else
			{
				gameObject = Object.Instantiate(m_LockAreaButton) as GameObject;
				UIPortalAreaLockButton component2 = gameObject.GetComponent<UIPortalAreaLockButton>();
				component2.SetNumber(adl[i].Id);
			}
			AreaInfo areaInfo = new AreaInfo();
			areaInfo.m_ButtonObject = gameObject;
			areaInfo.m_AreaData = adl[i];
			list.Add(areaInfo);
		}
		return list;
	}

	private void InitAreaInfoList()
	{
		mAreaInfoList = GetAreaInfoList(mAreaDataList, mSceneDatalist);
		for (int i = 0; i < mAreaInfoList.Count; i++)
		{
			mAreaInfoList[i].m_ButtonObject.name = i + "-" + mAreaInfoList[i].m_ButtonObject.name;
			mAreaInfoList[i].m_ButtonObject.transform.parent = m_Table.transform;
			mAreaInfoList[i].m_ButtonObject.transform.localPosition = Vector3.zero;
			mAreaInfoList[i].m_ButtonObject.transform.localEulerAngles = Vector3.zero;
			mAreaInfoList[i].m_ButtonObject.transform.localScale = Vector3.one;
			mAreaInfoList[i].m_ButtonObject.SetActive(true);
		}
		m_Table.repositionNow = true;
	}

	private List<SceneInfo> GetSceneInfoList(List<SceneData> sdl, List<AreaInfo> arl)
	{
		List<SceneInfo> list = new List<SceneInfo>();
		for (int i = 0; i < sdl.Count; i++)
		{
			if (arl[sdl[i].AreaID].m_ButtonObject.GetComponent<UIPortalAreaButton>() != null)
			{
				GameObject original = ((!sdl[i].IsSubScene) ? m_CityButton : ((!sdl[i].IsLock) ? m_InstanceButton : m_LockInstanceButton));
				GameObject gameObject = Object.Instantiate(original) as GameObject;
				DestinationButton component = gameObject.GetComponent<DestinationButton>();
				component.SetName(sdl[i].ButtonName);
				if (IsQuestScene(sdl[i]))
				{
					GameObject mark = Object.Instantiate(m_QuestMark) as GameObject;
					component.AddQuestMark(mark);
					questIndex = i;
				}
				SceneInfo sceneInfo = new SceneInfo();
				sceneInfo.m_ButtonObject = gameObject;
				sceneInfo.m_SceneData = sdl[i];
				list.Add(sceneInfo);
			}
		}
		return list;
	}

	private void InitSceneInfoList()
	{
		mSceneInfoList = GetSceneInfoList(mSceneDatalist, mAreaInfoList);
		int num = ((questIndex <= -1) ? lastIndex : questIndex);
		for (int i = 0; i < mSceneInfoList.Count; i++)
		{
			UIPortalAreaButton component = mAreaInfoList[mSceneInfoList[i].m_SceneData.AreaID].m_ButtonObject.GetComponent<UIPortalAreaButton>();
			component.AddDestinationButton(mSceneInfoList[i].m_ButtonObject);
			AddDelegate(mSceneInfoList[i].m_ButtonObject);
			if (i == num)
			{
				component.Select();
				ShowAreaInfo(i);
			}
		}
	}

	private void SetAllData()
	{
		SetData(GetAreaDataList(), GetSceneDataList());
	}

	private List<SceneData> GetSceneDataList()
	{
		Dictionary<string, SceneConfig> sceneConfig = GameConfig.GetInstance().sceneConfig;
		List<SceneData> list = new List<SceneData>();
		foreach (SceneConfig value in sceneConfig.Values)
		{
			if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetUserState()
				.GetStageInstanceState(value.SceneID - 32) == 1)
			{
				list.Add(new SceneData(value, false));
			}
			else if (!value.Hide)
			{
				list.Add(new SceneData(value, true));
			}
		}
		return list;
	}

	private List<AreaData> GetAreaDataList()
	{
		Dictionary<int, AreaConfig> areaConfig = GameConfig.GetInstance().areaConfig;
		List<AreaData> list = new List<AreaData>();
		foreach (AreaConfig value in areaConfig.Values)
		{
			list.Add(new AreaData(value));
		}
		return list;
	}

	private void SetTestData()
	{
		SetData(GetTestAreaDataList(), GetTestSceneDataList());
	}

	private List<SceneData> GetTestSceneDataList()
	{
		List<SceneData> list = new List<SceneData>();
		list.Add(new SceneData(null, false));
		list.Add(new SceneData(null, false));
		list.Add(new SceneData(null, false));
		return list;
	}

	private List<AreaData> GetTestAreaDataList()
	{
		List<AreaData> list = new List<AreaData>();
		list.Add(new AreaData(null));
		list.Add(new AreaData(null));
		list.Add(new AreaData(null));
		return list;
	}

	private void SetData(List<AreaData> alist, List<SceneData> slist)
	{
		if (alist != null && slist != null)
		{
			mAreaDataList = alist;
			mSceneDatalist = slist;
		}
	}

	public override void OnCloseButtonClick()
	{
		base.OnCloseButtonClick();
		Close();
	}

	protected override void OnClickThumb(GameObject go)
	{
		SceneData sceneData = mSceneInfoList[curIndex].m_SceneData;
		if (go.Equals(m_CloseButton))
		{
			Close();
			return;
		}
		if (go.Equals(m_BossRushButton))
		{
			SetMenuCloseOnDestroy(false);
			UIBossRush.Show();
			return;
		}
		if (go.Equals(m_EnterButton))
		{
			GoToThisScene(sceneData);
			return;
		}
		for (int i = 0; i < mSceneInfoList.Count; i++)
		{
			if (go.Equals(mSceneInfoList[i].m_ButtonObject))
			{
				if (curIndex != i)
				{
					ShowAreaInfo(i);
				}
				else if (!sceneData.IsLock)
				{
					GoToThisScene(sceneData);
				}
			}
		}
		for (int j = 0; j < m_PortalMarkList.Count; j++)
		{
			if (go.Equals(m_PortalMarkList[j]))
			{
				if (j != sceneData.CurrentIndexInList)
				{
					sceneData.CurrentIndexInList = j;
				}
				else
				{
					GoToThisScene(sceneData);
				}
			}
		}
	}

	private void GoToThisScene(SceneData sd)
	{
		if (sd.IsArena)
		{
			SetMenuCloseOnDestroy(false);
			MemoryManager.FreeNGUI(m_MapImage);
			Close();
			UIArena.Show(sd.FatherSceneID);
			return;
		}
		GameApp.GetInstance().GetUserState().SetLastPortalIndex((byte)curIndex);
		if (!IsCurrentScene(sd))
		{
			PortalEnterTrigger.instance.SetDest(sd.SceneName, sd.CurrentPortalIndex);
			MemoryManager.FreeNGUI(m_MapImage);
			Close();
		}
	}

	private void ShowAreaInfo(int index)
	{
		if (index >= 0 && index < mSceneInfoList.Count)
		{
			DeleteButtonLight(curIndex);
			curIndex = index;
			AddButtonLight(curIndex);
			SceneData sceneData = mSceneInfoList[curIndex].m_SceneData;
			m_DetailsButton.SetName(sceneData.ButtonName);
			m_DetailsButton.SetDetails(sceneData.SceneIntroTxt);
			LoadPreview(sceneData);
			LoadPortals(sceneData);
			LoadQuestPos(sceneData);
			CheckEnterButton(sceneData);
		}
	}

	private void AddButtonLight(int index)
	{
		UIFrameManager.GetInstance().CreateFrame(mSceneInfoList[index].m_ButtonObject, 2);
	}

	private void DeleteButtonLight(int index)
	{
		if (index > -1)
		{
			UIFrameManager.GetInstance().DeleteFrame(mSceneInfoList[index].m_ButtonObject);
		}
	}

	private void CheckEnterButton(SceneData sd)
	{
		if (IsCurrentScene(sd) || sd.IsLock)
		{
			m_EnterButton.SetActive(false);
		}
		else
		{
			m_EnterButton.SetActive(true);
		}
	}

	private void LoadPreview(SceneData sd)
	{
		if (m_MapImage != null)
		{
			MemoryManager.FreeNGUI(m_MapImage);
		}
		if (sd.IsLock)
		{
			m_LabelUnlockCon.gameObject.SetActive(true);
			m_LabelUnlockCon.text = LocalizationManager.GetInstance().GetString("MENU_PORTAL_UNLOCK_CONDITION_SCENE" + sd.SceneID);
			return;
		}
		m_LabelUnlockCon.gameObject.SetActive(false);
		GameObject original = Resources.Load("MiniMapX/Res/P_" + sd.SceneIntroImgName) as GameObject;
		m_MapImage = Object.Instantiate(original) as GameObject;
		m_MapImage.SetActive(true);
		m_MapImage.transform.parent = m_Map.transform;
		m_MapImage.transform.localPosition = Vector3.zero;
		m_MapImage.transform.localRotation = Quaternion.identity;
		m_MapImage.transform.localScale = Vector3.one;
	}

	private void LoadPortals(SceneData sd)
	{
		if (m_PortalMarkList != null)
		{
			foreach (GameObject portalMark in m_PortalMarkList)
			{
				Object.Destroy(portalMark);
			}
			m_PortalMarkList.Clear();
		}
		if (sd.IsLock)
		{
			return;
		}
		m_PortalMarkList = new List<GameObject>();
		List<Vector2> portalListInUI = sd.GetPortalListInUI(new Vector2(m_Map.transform.localScale.x, m_Map.transform.localScale.y), new Vector2((0f - m_Map.transform.localScale.x) / 2f, (0f - m_Map.transform.localScale.y) / 2f));
		for (int i = 0; i < portalListInUI.Count; i++)
		{
			GameObject gameObject = Object.Instantiate(m_InstancePortalMark) as GameObject;
			gameObject.SetActive(true);
			gameObject.transform.parent = m_PortalMarkContainer.transform;
			gameObject.transform.localPosition = portalListInUI[i];
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			m_PortalMarkList.Add(gameObject);
			AddDelegate(gameObject);
			if (i == sd.CurrentIndexInList)
			{
				UICheckbox component = gameObject.GetComponent<UICheckbox>();
				component.startsChecked = true;
			}
		}
	}

	private void LoadQuestPos(SceneData sd)
	{
		if (m_QuestPosMarkList != null)
		{
			foreach (GameObject questPosMark in m_QuestPosMarkList)
			{
				Object.Destroy(questPosMark);
			}
			m_QuestPosMarkList.Clear();
		}
		if (!sd.IsLock)
		{
			m_QuestPosMarkList = new List<GameObject>();
			List<Vector2> questPosListInUI = sd.GetQuestPosListInUI(new Vector2(m_Map.transform.localScale.x, m_Map.transform.localScale.y), new Vector2((0f - m_Map.transform.localScale.x) / 2f, (0f - m_Map.transform.localScale.y) / 2f));
			for (int i = 0; i < questPosListInUI.Count; i++)
			{
				GameObject gameObject = null;
				gameObject = ((!sd.IsMainQuest) ? (Object.Instantiate(m_SubQuestPosMark) as GameObject) : (Object.Instantiate(m_MainQuestPosMark) as GameObject));
				gameObject.SetActive(true);
				gameObject.transform.parent = m_QuestPosMarkContainer.transform;
				gameObject.transform.localPosition = questPosListInUI[i];
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				m_QuestPosMarkList.Add(gameObject);
			}
		}
	}

	private bool IsCurrentScene(SceneData sd)
	{
		SceneConfig sceneConfig = GameConfig.GetInstance().sceneConfig[sd.SceneName];
		if (GameApp.GetInstance().GetGameWorld().CurrentSceneID == sceneConfig.SceneID && GameApp.GetInstance().GetUserState().GetCurrentCityID() == sceneConfig.AreaID)
		{
			return true;
		}
		return false;
	}

	private bool IsQuestScene(SceneData sd)
	{
		SceneConfig sceneConfig = GameConfig.GetInstance().sceneConfig[sd.SceneName];
		Dictionary<string, QuestPoint> dictionary = null;
		dictionary = ((!GameApp.GetInstance().GetGameMode().IsMultiPlayer()) ? GameApp.GetInstance().GetUserState().m_questStateContainer.GetCurrentQuestPointForPortal() : GameApp.GetInstance().GetUserState().m_questStateContainer.GetMarkQuestPointForPortal());
		if (dictionary == null)
		{
			return false;
		}
		foreach (KeyValuePair<string, QuestPoint> item in dictionary)
		{
			if (item.Value.m_siteId == sceneConfig.SceneID)
			{
				return true;
			}
		}
		return false;
	}

	public static void Show()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(27, false, true, false);
		isShow = true;
	}

	public static void Close()
	{
		GameApp.GetInstance().GetUIStateManager().FrGoToPhase(GameApp.GetInstance().GetUIStateManager().FrGetPreviousPhase(), false, false, false);
		isShow = false;
	}

	public static bool IsShow()
	{
		return isShow;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Close();
		}
	}
}
