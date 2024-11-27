using System.Collections.Generic;
using UnityEngine;

public class BossAreaTrigger : MonoBehaviour
{
	public abstract class TriggerState
	{
		private bool isEnd;

		private TriggerState nextState;

		public void Init()
		{
			isEnd = false;
			nextState = null;
			OnInit();
		}

		public void Update()
		{
			OnUpdate();
		}

		public bool IsEnd()
		{
			return isEnd;
		}

		public bool IsHasNextState()
		{
			return nextState != null;
		}

		public TriggerState GetNextState()
		{
			return nextState;
		}

		protected void End()
		{
			isEnd = true;
		}

		protected void NextState(TriggerState state)
		{
			nextState = state;
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnUpdate()
		{
		}
	}

	public class StateEmpty : TriggerState
	{
	}

	public class StateLockControl : TriggerState
	{
		private BossAreaTrigger mTrigger;

		public StateLockControl(BossAreaTrigger trigger)
		{
			mTrigger = trigger;
		}

		protected override void OnInit()
		{
			mTrigger.mIsCutSceneFinish = false;
		}

		protected override void OnUpdate()
		{
			if (HUDManager.instance != null)
			{
				HUDManager.instance.Close();
			}
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = true;
			NextState(STATE_CAMERA_BOSS);
		}
	}

	public class StateCameraBoss : TriggerState
	{
		private bool mIsBossLoaded;

		private BossAreaTrigger mTrigger;

		public StateCameraBoss(BossAreaTrigger trigger)
		{
			mTrigger = trigger;
		}

		protected override void OnInit()
		{
			mTrigger.m_CameraBoss.enabled = true;
			mTrigger.m_IsDoorOpen = false;
			mIsBossLoaded = false;
		}

		protected override void OnUpdate()
		{
			if (!mIsBossLoaded)
			{
				Dictionary<string, Enemy> enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
				bool flag = false;
				foreach (Enemy value in enemies.Values)
				{
					if (!value.IsBoss())
					{
						continue;
					}
					mIsBossLoaded = true;
					EnemyBoss enemyBoss = value as EnemyBoss;
					enemyBoss.StartSeeBoss();
					if (flag)
					{
						continue;
					}
					flag = true;
					if (mTrigger.mFsm != null)
					{
						PlayMakerFSM component = mTrigger.mFsm.GetComponent<PlayMakerFSM>();
						if (component != null)
						{
							component.SendEvent("StartCameraAnim");
						}
					}
					else
					{
						mTrigger.mIsCutSceneFinish = true;
					}
				}
			}
			if (mTrigger.mIsCutSceneFinish)
			{
				mTrigger.m_CameraBoss.enabled = false;
				Camera.main.enabled = true;
				NextState(STATE_UNLOCK_CONTROL);
			}
		}
	}

	public class StateUnlockControl : TriggerState
	{
		protected override void OnUpdate()
		{
			if (HUDManager.instance != null)
			{
				HUDManager.instance.Show();
			}
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InputController.Block = false;
			Dictionary<string, Enemy> enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
			string[] array = new string[enemies.Count];
			enemies.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				Enemy enemy = enemies[array[i]];
				if (enemy.IsBoss())
				{
					EnemyBoss enemyBoss = enemy as EnemyBoss;
					enemyBoss.StartBossBattle();
				}
			}
			NextState(STATE_CHECK_BOSS_DEAD);
		}
	}

	public class StateCheckBossDead : TriggerState
	{
		private EnemyBoss boss;

		private BossAreaTrigger mBossAreaTrigger;

		public StateCheckBossDead(BossAreaTrigger bossAreaTrigger)
		{
			mBossAreaTrigger = bossAreaTrigger;
		}

		protected override void OnInit()
		{
			Dictionary<string, Enemy> enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
			string[] array = new string[enemies.Count];
			enemies.Keys.CopyTo(array, 0);
			for (int i = 0; i < array.Length; i++)
			{
				Enemy enemy = enemies[array[i]];
				if (enemy.IsBoss())
				{
					boss = enemy as EnemyBoss;
					break;
				}
			}
		}

		protected override void OnUpdate()
		{
			if (boss.GetState() == Enemy.DEAD_STATE)
			{
				if (GameApp.GetInstance().GetGameMode().IsSingle())
				{
					GameApp.GetInstance().GetGameWorld().OnWinBossBattle();
				}
				NextState(STATE_EMPTY);
			}
			else if (GameApp.GetInstance().GetGameMode().SubModePlay == SubMode.Story)
			{
				NextState(STATE_EMPTY);
			}
		}
	}

	public static StateEmpty STATE_EMPTY;

	public static StateLockControl STATE_LOCK_CONTROL;

	public static StateCameraBoss STATE_CAMERA_BOSS;

	public static StateUnlockControl STATE_UNLOCK_CONTROL;

	public static StateCheckBossDead STATE_CHECK_BOSS_DEAD;

	public Camera m_CameraBoss;

	public GameObject mFsm;

	public bool m_IsDoorOpen = true;

	public bool mIsCutSceneFinish;

	private TriggerState state;

	private GameWorld mGameWorld;

	private void Awake()
	{
		STATE_EMPTY = new StateEmpty();
		STATE_LOCK_CONTROL = new StateLockControl(this);
		STATE_CAMERA_BOSS = new StateCameraBoss(this);
		STATE_UNLOCK_CONTROL = new StateUnlockControl();
		STATE_CHECK_BOSS_DEAD = new StateCheckBossDead(this);
		mGameWorld = GameApp.GetInstance().GetGameWorld();
		if (mGameWorld.BossState == EBossState.NORMAL)
		{
			m_IsDoorOpen = true;
		}
		else if (mGameWorld.BossState == EBossState.BATTLE)
		{
			m_IsDoorOpen = false;
		}
		else if (mGameWorld.BossState == EBossState.LOSE)
		{
			m_IsDoorOpen = true;
		}
		else if (mGameWorld.BossState == EBossState.WIN)
		{
			m_IsDoorOpen = true;
			GameObject[] array = GameObject.FindGameObjectsWithTag(TagName.BOSS_PORTAL);
			GameObject[] array2 = array;
			foreach (GameObject gameObject in array2)
			{
				MapEntranceScript component = gameObject.GetComponent<MapEntranceScript>();
				if (null != component)
				{
					component.enabled = true;
				}
			}
			GameObject[] array3 = GameObject.FindGameObjectsWithTag(TagName.BOSS_PORTAL_EFFECT);
			GameObject[] array4 = array3;
			foreach (GameObject gameObject2 in array4)
			{
				int childCount = gameObject2.transform.GetChildCount();
				for (int k = 0; k < childCount; k++)
				{
					gameObject2.transform.GetChild(k).gameObject.SetActive(true);
				}
			}
		}
		EnterState(STATE_EMPTY);
	}

	private void Update()
	{
		if (mGameWorld.BossState == EBossState.BATTLE && state != null)
		{
			state.Update();
			if (state.IsHasNextState())
			{
				EnterState(state.GetNextState());
			}
		}
	}

	private void OnTriggerEnter(Collider obj)
	{
		Debug.Log("OnTriggerEnter : " + obj.name);
		Debug.Log("OnTriggerEnter : " + GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.State);
		if (mGameWorld.BossState == EBossState.NORMAL || mGameWorld.BossState == EBossState.LOSE)
		{
			Debug.Log("Trigger" + obj.gameObject.name);
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsCoopMode())
			{
				InvitationRequest request = new InvitationRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetUserID(), InvitationRequest.Type.BossRoom, SubMode.Boss, GameApp.GetInstance().GetGameWorld().CurrentSceneID, 0);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
				return;
			}
			mGameWorld.BossState = EBossState.BATTLE;
			Debug.Log("Dodge_This --- Start");
			AchievementTrigger trigger = AchievementTrigger.Create(AchievementID.Dodge_This, AchievementTrigger.Type.Start);
			AchievementManager.GetInstance().Trigger(trigger);
			StartAnimation();
		}
	}

	public void StartAnimation()
	{
		EnterState(STATE_LOCK_CONTROL);
		GameApp.GetInstance().GetGameMode().SubModePlay = SubMode.Boss;
	}

	private void EnterState(TriggerState _state)
	{
		Debug.Log("EnterState : " + _state);
		state = _state;
		state.Init();
	}
}
