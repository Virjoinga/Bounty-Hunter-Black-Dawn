using System.Collections.Generic;
using UnityEngine;

public class Npc : GameUnit
{
	public const float TotalRotateTime = 1.5f;

	public static NpcState IDLE_STATE = new NpcIdleState();

	public static NpcState TALK_STATE = new NpcTalkState();

	public static NpcState ASSIGNABLE_STATE = new NpcAssignableState();

	public static NpcState ASSIGNED_STATE = new NpcAssignableState();

	public float m_lastIdleTime;

	public float m_idleInterval;

	public Quaternion m_initRotation;

	public Vector3 m_initForward;

	public float m_turnRoundTime;

	protected QuestScript m_questScript;

	public NpcType m_type;

	public static Dictionary<string, int[]> IdelRate = new Dictionary<string, int[]> { 
	{
		"Green",
		new int[2] { 85, 100 }
	} };

	public NpcState State { get; set; }

	public virtual void Init()
	{
		base.GameUnitType = EGameUnitType.NPC;
		State = IDLE_STATE;
	}

	public override void SetObject(GameObject obj)
	{
		base.SetObject(obj);
		base.Name = obj.name;
		m_initRotation = obj.transform.localRotation;
		m_initForward = obj.transform.forward;
		animation = obj.GetComponent<Animation>();
		m_questScript = obj.GetComponent<QuestScript>();
		m_type = GameConfig.GetInstance().npcConfig[obj.name].m_type;
	}

	public QuestScript GetQuestScript()
	{
		return m_questScript;
	}

	public PromptType GetPromptType()
	{
		return m_questScript.GetPromptType();
	}

	public void UpdateFlag()
	{
		m_questScript.UpdateFlag();
	}

	public void SetIdleIntervalTime(float interval)
	{
		m_lastIdleTime = Time.time;
		m_idleInterval = interval;
	}

	public void SetIdleIntervalTime(float maxInterval, float minInterval)
	{
		m_lastIdleTime = Time.time;
		float idleInterval = Random.Range(minInterval, maxInterval);
		m_idleInterval = idleInterval;
	}

	public void StartTurnRoundTime()
	{
		m_turnRoundTime = Time.time;
	}

	public virtual void Loop(float deltaTime)
	{
		State.NextState(this, deltaTime);
	}

	public void TurnRound(Vector3 pos)
	{
		Vector3 normalized = (pos - GetPosition()).normalized;
		Quaternion to = Quaternion.LookRotation(normalized);
		GetTransform().rotation = Quaternion.Lerp(GetTransform().rotation, to, (Time.time - m_turnRoundTime) / 1.5f);
	}

	public void TurnRound(Quaternion m_initRotation)
	{
		GetTransform().rotation = Quaternion.Lerp(GetTransform().rotation, m_initRotation, (Time.time - m_turnRoundTime) / 1.5f);
	}
}
