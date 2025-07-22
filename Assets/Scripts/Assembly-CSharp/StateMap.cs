using System.Collections.Generic;
using UnityEngine;

public class StateMap : MonoBehaviour
{
	private Dictionary<string, StateIcon> m_EnemyList = new Dictionary<string, StateIcon>();

	private Dictionary<string, StateIcon> m_QuestList = new Dictionary<string, StateIcon>();

	private Dictionary<string, StateIcon> m_RemotePlayerList = new Dictionary<string, StateIcon>();

	private Dictionary<string, StateIcon> m_RemotePlayerDyingList = new Dictionary<string, StateIcon>();

	private Dictionary<string, StateIcon> m_CapturePointList1 = new Dictionary<string, StateIcon>();

	private Dictionary<string, StateIcon> m_CapturePointList2 = new Dictionary<string, StateIcon>();

	private float mLastUpdateTime;

	public GameObject m_CapturePointIconContainer1;

	public GameObject m_CapturePointIconContainer2;

	public GameObject m_EnemyIconContainer;

	public GameObject m_QuestIconContainer;

	public GameObject m_RemotePlayerIconContainer;

	public GameObject m_RemotePlayerDyingIconContainer;

	public float intensity;

	private StateIcon[] m_CapturePointIcons1;

	private StateIcon[] m_CapturePointIcons2;

	private StateIcon[] m_EnemyIcons;

	private StateIcon[] m_QuestIcons;

	private StateIcon[] m_RemotePlayerIcons;

	private StateIcon[] m_RemotePlayerDyingIcons;

	private Vector3 initPos;

	private Color redPointColor = new Color(1f, 0f, 0f);

	private Color bluePointColor = new Color(0f, 0.99215686f, 1f);

	private Color whitePointColor = Color.white;

	private void Awake()
	{
		m_CapturePointIcons1 = m_CapturePointIconContainer1.GetComponentsInChildren<StateIcon>();
		m_CapturePointIcons2 = m_CapturePointIconContainer2.GetComponentsInChildren<StateIcon>();
		m_EnemyIcons = m_EnemyIconContainer.GetComponentsInChildren<StateIcon>();
		m_QuestIcons = m_QuestIconContainer.GetComponentsInChildren<StateIcon>();
		m_RemotePlayerIcons = m_RemotePlayerIconContainer.GetComponentsInChildren<StateIcon>();
		m_RemotePlayerDyingIcons = m_RemotePlayerDyingIconContainer.GetComponentsInChildren<StateIcon>();
		initPos = base.gameObject.transform.localPosition;
	}

	private void OnEnable()
	{
		Hide(m_CapturePointIcons1);
		Hide(m_CapturePointIcons2);
		Hide(m_EnemyIcons);
		Hide(m_QuestIcons);
		Hide(m_RemotePlayerIcons);
		Hide(m_RemotePlayerDyingIcons);
		Show(m_CapturePointList1);
		Show(m_CapturePointList2);
		Show(m_EnemyList);
		Show(m_QuestList);
		Show(m_RemotePlayerList);
		Show(m_RemotePlayerDyingList);
	}

	private void Show(Dictionary<string, StateIcon> list)
	{
		foreach (KeyValuePair<string, StateIcon> item in list)
		{
			NGUITools.SetActive(item.Value.gameObject, true);
		}
	}

	private void Hide(StateIcon[] icons)
	{
		foreach (StateIcon stateIcon in icons)
		{
			NGUITools.SetActive(stateIcon.gameObject, false);
		}
	}

	private void Update()
	{
		if (Time.time - mLastUpdateTime > 0.05f)
		{
			mLastUpdateTime = Time.time;
			Dictionary<string, UserStateHUD.GameUnitHUD> enemyList = UserStateHUD.GetInstance().GetEnemyList();
			Dictionary<string, QuestPoint> questList = UserStateHUD.GetInstance().GetQuestList();
			Dictionary<string, UserStateHUD.GameUnitHUD> remotePlayerList = UserStateHUD.GetInstance().GetRemotePlayerList();
			Dictionary<string, TDMCapturePointScript> capturePointList = UserStateHUD.GetInstance().GetCapturePointList();
			Update(m_EnemyList, enemyList, m_EnemyIcons, false);
			if (!Arena.GetInstance().IsCurrentSceneArena())
			{
				Update(m_QuestList, questList, m_QuestIcons);
			}
			Update(m_RemotePlayerList, remotePlayerList, m_RemotePlayerIcons, false);
			Update(m_RemotePlayerDyingList, remotePlayerList, m_RemotePlayerDyingIcons, true);
			if (GameApp.GetInstance().GetGameWorld().IsVS1Scene())
			{
				Update(m_CapturePointList1, capturePointList, m_CapturePointIcons1);
			}
			else
			{
				Update(m_CapturePointList2, capturePointList, m_CapturePointIcons2);
			}
		}
	}

	private void Translate()
	{
		Vector3 vector = new Vector3(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InputController.CameraRotation.x, GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.InputController.CameraRotation.y, 0f);
		base.gameObject.transform.localPosition = Vector3.MoveTowards(base.gameObject.transform.localPosition, initPos + new Vector3(vector.x * 4f, 0f, 0f), 1f);
	}

	private void Update(Dictionary<string, StateIcon> iconList, Dictionary<string, UserStateHUD.GameUnitHUD> unitList, StateIcon[] icons, bool reverseAlive)
	{
		foreach (KeyValuePair<string, UserStateHUD.GameUnitHUD> unit in unitList)
		{
			if (!unit.Value.IsActive || iconList.ContainsKey(unit.Key) || ((!unit.Value.IsAlive || reverseAlive) && (unit.Value.IsAlive || !reverseAlive)))
			{
				continue;
			}
			if (unit.Value.IconIndex > -1 && unit.Value.IconIndex < icons.Length)
			{
				StateIcon stateIcon = icons[unit.Value.IconIndex];
				if (!stateIcon.gameObject.activeSelf)
				{
					NGUITools.SetActive(stateIcon.gameObject, true);
					stateIcon.unit = unit.Value;
					iconList.Add(unit.Key, stateIcon);
					break;
				}
				continue;
			}
			foreach (StateIcon stateIcon2 in icons)
			{
				if (!stateIcon2.gameObject.activeSelf)
				{
					NGUITools.SetActive(stateIcon2.gameObject, true);
					stateIcon2.unit = unit.Value;
					iconList.Add(unit.Key, stateIcon2);
					break;
				}
			}
		}
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, StateIcon> icon in iconList)
		{
			if (!unitList.ContainsKey(icon.Key) || !unitList[icon.Key].IsActive || (!unitList[icon.Key].IsAlive && !reverseAlive) || (unitList[icon.Key].IsAlive && reverseAlive))
			{
				list.Add(icon.Key);
				continue;
			}
			float num = MathUtil.GetAngleBetweenUserHorizontal(unitList[icon.Key].Position);
			if (num <= -180f)
			{
				num += 360f;
			}
			if (num >= 180f)
			{
				num -= 360f;
			}
			num = Mathf.Clamp(num, -50f, 50f);
			icon.Value.UpdatePosition(num);
		}
		foreach (string item in list)
		{
			NGUITools.SetActive(iconList[item].gameObject, false);
			iconList.Remove(item);
		}
	}

	private void Update(Dictionary<string, StateIcon> iconList, Dictionary<string, QuestPoint> unitList, StateIcon[] icons)
	{
		if (unitList == null)
		{
			if (iconList.Count <= 0)
			{
				return;
			}
			foreach (KeyValuePair<string, StateIcon> icon in iconList)
			{
				NGUITools.SetActive(icon.Value.gameObject, false);
			}
			iconList.Clear();
			return;
		}
		foreach (KeyValuePair<string, QuestPoint> unit in unitList)
		{
			if (iconList.ContainsKey(unit.Key))
			{
				continue;
			}
			foreach (StateIcon stateIcon in icons)
			{
				if (!stateIcon.gameObject.activeSelf)
				{
					NGUITools.SetActive(stateIcon.gameObject, true);
					iconList.Add(unit.Key, stateIcon);
					break;
				}
			}
		}
		List<string> list = new List<string>();
		GameObject gameObject = null;
		float num = 100000000f;
		List<GameObject> portal = GameApp.GetInstance().GetGameScene().GetPortal();
		foreach (GameObject item in portal)
		{
			float num2 = UserStateHUD.GetInstance().GetUserTransform().position.x - item.transform.position.x;
			float num3 = UserStateHUD.GetInstance().GetUserTransform().position.z - item.transform.position.z;
			float num4 = num2 * num2 + num3 * num3;
			if (num4 < num)
			{
				num = num4;
				gameObject = item;
			}
		}
		Vector3 targetPos = Vector3.zero;
		float num5 = 100000000f;
		foreach (KeyValuePair<string, StateIcon> icon2 in iconList)
		{
			if (!unitList.ContainsKey(icon2.Key))
			{
				list.Add(icon2.Key);
				continue;
			}
			List<QuestPoint.QuestPosition> pos = unitList[icon2.Key].GetPos();
			int num6 = 0;
			for (num6 = 0; num6 < pos.Count; num6++)
			{
				float num7 = 0f;
				Vector3 vector = Vector3.zero;
				if (unitList[icon2.Key].m_siteId == GameApp.GetInstance().GetGameWorld().CurrentSceneID)
				{
					if (pos[num6].m_state != 0)
					{
						continue;
					}
					vector = new Vector3(pos[num6].m_pos.x, 0f, pos[num6].m_pos.y);
				}
				else if (gameObject != null)
				{
					vector = new Vector3(gameObject.transform.position.x, 0f, gameObject.transform.position.z);
				}
				float num8 = UserStateHUD.GetInstance().GetUserTransform().position.x - vector.x;
				float num9 = UserStateHUD.GetInstance().GetUserTransform().position.z - vector.z;
				num7 = num8 * num8 + num9 * num9;
				if (num7 < num5)
				{
					num5 = num7;
					targetPos = vector;
				}
			}
		}
		float num10 = MathUtil.GetAngleBetweenUserHorizontal(targetPos);
		if (num10 <= -180f)
		{
			num10 += 360f;
		}
		if (num10 >= 180f)
		{
			num10 -= 360f;
		}
		num10 = Mathf.Clamp(num10, -50f, 50f);
		foreach (KeyValuePair<string, StateIcon> icon3 in iconList)
		{
			icon3.Value.UpdatePosition(num10);
		}
		foreach (string item2 in list)
		{
			NGUITools.SetActive(iconList[item2].gameObject, false);
			iconList.Remove(item2);
		}
	}

	private void Update(Dictionary<string, StateIcon> iconList, Dictionary<string, TDMCapturePointScript> unitList, StateIcon[] icons)
	{
		List<string> list = new List<string>();
		List<UserStateHUD.VSBattleFieldPoint> pointInfo = UserStateHUD.GetInstance().GetVSBattleFieldState().PointInfo;
		foreach (KeyValuePair<string, TDMCapturePointScript> unit in unitList)
		{
			if (unit.Value.PointID >= pointInfo.Count)
			{
				continue;
			}
			if (!iconList.ContainsKey(unit.Key))
			{
				if (pointInfo[unit.Value.PointID].IsCapturing)
				{
					StateIcon stateIcon = icons[unit.Value.PointID];
					if (!stateIcon.gameObject.activeSelf)
					{
						NGUITools.SetActive(stateIcon.gameObject, true);
						iconList.Add(unit.Key, stateIcon);
						break;
					}
				}
			}
			else if (!pointInfo[unit.Value.PointID].IsCapturing)
			{
				list.Add(unit.Key);
			}
		}
		foreach (KeyValuePair<string, StateIcon> icon in iconList)
		{
			if (!unitList.ContainsKey(icon.Key))
			{
				list.Add(icon.Key);
				continue;
			}
			TDMCapturePointScript tDMCapturePointScript = unitList[icon.Key];
			UserStateHUD.VSBattleFieldPoint vSBattleFieldPoint = pointInfo[tDMCapturePointScript.PointID];
			TeamName userTeamName = UserStateHUD.GetInstance().GetUserTeamName(vSBattleFieldPoint.Owner);
			if (userTeamName != UserStateHUD.GetInstance().GetUserTeamName())
			{
				icon.Value.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
			}
			else
			{
				icon.Value.transform.localScale = Vector3.one;
			}
			switch (userTeamName)
			{
			case TeamName.Blue:
				icon.Value.SetColor(bluePointColor);
				break;
			case TeamName.Red:
				icon.Value.SetColor(redPointColor);
				break;
			default:
				icon.Value.SetColor(whitePointColor);
				break;
			}
			float num = MathUtil.GetAngleBetweenUserHorizontal(unitList[icon.Key].transform.position);
			if (num <= -180f)
			{
				num += 360f;
			}
			if (num >= 180f)
			{
				num -= 360f;
			}
			num = Mathf.Clamp(num, -50f, 50f);
			icon.Value.UpdatePosition(num);
		}
		foreach (string item in list)
		{
			NGUITools.SetActive(iconList[item].gameObject, false);
			iconList.Remove(item);
		}
	}
}
