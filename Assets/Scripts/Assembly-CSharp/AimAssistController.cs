using System.Collections.Generic;
using UnityEngine;

public class AimAssistController
{
	public abstract class AimProcess
	{
		private class EnemyState
		{
			public Enemy enemy;

			public bool isHead;

			public Vector3 deltaAngle;

			public Vector3 enemyPos;
		}

		private Vector3 sightHeadCenter = ((!(HUDManager.instance == null)) ? HUDManager.instance.m_InfoManager.m_SightHeadState.transform.localPosition : Vector3.zero);

		private float mRadius = 50f;

		private bool isNotUpdate;

		private Vector2 mRotation;

		private bool isStop;

		private bool isNotifyStop;

		private int property;

		private EnemyState state;

		private float mUpdateTime;

		public void Init()
		{
			mRotation = Vector2.zero;
			isStop = false;
			isNotifyStop = false;
			isNotUpdate = false;
			state = GetTarget(mRadius);
			mUpdateTime = Time.time;
			if (state == null)
			{
				OnUpdate(false, GetSightHeadCenter(), Vector3.zero);
				return;
			}
			state = PosProcess(state);
			OnStart(true, state.enemyPos, state.deltaAngle);
		}

		public void Update()
		{
			if (isStop || isNotUpdate)
			{
				return;
			}
			if (Time.time - mUpdateTime > 0.2f)
			{
				mUpdateTime = Time.time;
				if (state == null || state.enemy == null || !state.enemy.IsActive() || !state.enemy.CanAutoAim())
				{
					state = GetTarget(mRadius);
				}
				else
				{
					float num = Vector3.Distance(GetSightHeadCenter(), state.enemyPos);
					if (num > mRadius)
					{
						state = GetTarget(mRadius);
					}
				}
			}
			GameWorld gameWorld = GameApp.GetInstance().GetGameWorld();
			LocalPlayer localPlayer = ((gameWorld != null) ? gameWorld.GetLocalPlayer() : null);
			if (state == null || state.enemy == null || !state.enemy.IsActive() || !state.enemy.CanAutoAim() || gameWorld == null || localPlayer == null || localPlayer.GetWeapon().GetWeaponType() == WeaponType.RPG)
			{
				OnUpdate(false, GetSightHeadCenter(), Vector3.zero);
				return;
			}
			state = PosProcess(state);
			OnUpdate(true, state.enemyPos, state.deltaAngle);
		}

		private EnemyState PosProcess(EnemyState target)
		{
			if (Camera.main == null)
			{
				return target;
			}
			Vector3 vector = Camera.main.transform.eulerAngles;
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			if (target.isHead)
			{
				zero = Camera.main.WorldToScreenPoint(target.enemy.GetHeadPosition()) - new Vector3(Screen.width / 2, Screen.height / 2);
				Camera.main.transform.LookAt(target.enemy.GetHeadPosition());
				zero2 = Camera.main.transform.eulerAngles;
			}
			else
			{
				zero = Camera.main.WorldToScreenPoint(target.enemy.GetBodyPosition()) - new Vector3(Screen.width / 2, Screen.height / 2);
				Camera.main.transform.LookAt(target.enemy.GetBodyPosition());
				zero2 = Camera.main.transform.eulerAngles;
			}
			Camera.main.transform.eulerAngles = vector;
			if (Mathf.Abs(zero2.x - vector.x) > 180f)
			{
				if (vector.x > 180f)
				{
					vector = new Vector3(vector.x - 360f, vector.y, vector.z);
				}
				if (zero2.x > 180f)
				{
					zero2 = new Vector3(zero2.x - 360f, zero2.y, zero2.z);
				}
			}
			if (Mathf.Abs(zero2.y - vector.y) > 180f)
			{
				if (vector.y > 180f)
				{
					vector = new Vector3(vector.x, vector.y - 360f, vector.z);
				}
				if (zero2.y > 180f)
				{
					zero2 = new Vector3(zero2.x, zero2.y - 360f, zero2.z);
				}
			}
			target.enemyPos = zero;
			target.deltaAngle = zero2 - vector;
			return target;
		}

		protected virtual void OnStart(bool hasData, Vector3 enemyPos, Vector3 deltaAngle)
		{
		}

		protected virtual void OnUpdate(bool hasData, Vector3 enemyPos, Vector3 deltaAngle)
		{
		}

		protected void SetProperty(int property)
		{
			this.property = property;
		}

		public int GetProperty()
		{
			return property;
		}

		public Vector2 GetRotation()
		{
			return mRotation;
		}

		protected void SetRotation(Vector2 rotation)
		{
			mRotation = rotation;
		}

		public void TryToStop(AimProcess aimProcess)
		{
			if (aimProcess.GetProperty() > GetProperty())
			{
				Stop(aimProcess);
			}
			else
			{
				NotifyStop(aimProcess);
			}
		}

		private void NotifyStop(AimProcess aimProcess)
		{
			if (!isNotifyStop)
			{
				isNotifyStop = true;
				OnNotifyStop(aimProcess);
			}
		}

		protected virtual void OnNotifyStop(AimProcess aimProcess)
		{
		}

		protected bool IsNotifyStop()
		{
			return isNotifyStop;
		}

		protected void Stop()
		{
			Stop(null);
		}

		protected void Stop(AimProcess aimProcess)
		{
			if (!isStop)
			{
				isStop = true;
				OnStop(aimProcess);
			}
		}

		protected virtual void OnStop(AimProcess aimProcess)
		{
		}

		public bool IsStop()
		{
			return isStop;
		}

		protected Vector3 GetSightHeadCenter()
		{
			return sightHeadCenter;
		}

		protected void SetUpdate(bool state)
		{
			isNotUpdate = !state;
		}

		protected void SetRadius(float radius)
		{
			mRadius = radius;
		}

		protected float GetRadius()
		{
			return mRadius;
		}

		private EnemyState GetTarget(float radius)
		{
			Camera mainCamera = Camera.main;
			Dictionary<string, Enemy> enemies = GameApp.GetInstance().GetGameScene().GetEnemies();
			List<EnemyState> list = new List<EnemyState>();
			foreach (KeyValuePair<string, Enemy> item in enemies)
			{
				if (!item.Value.InPlayingState() || !item.Value.AimAssist())
				{
					continue;
				}
				Vector3 b = mainCamera.WorldToScreenPoint(item.Value.GetBodyPosition()) - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
				Vector3 b2 = mainCamera.WorldToScreenPoint(item.Value.GetHeadPosition()) - new Vector3(Screen.width / 2, Screen.height / 2, 0f);
				float angleBetweenUserHorizontal = MathUtil.GetAngleBetweenUserHorizontal(item.Value.GetTransform());
				float num = Vector3.Distance(GetSightHeadCenter(), b);
				float num2 = Vector3.Distance(GetSightHeadCenter(), b2);
				float num3 = Mathf.Min(num, num2);
				if (num3 <= radius && Mathf.Abs(angleBetweenUserHorizontal) < 30f)
				{
					EnemyState enemyState = new EnemyState();
					enemyState.enemy = item.Value;
					if (num < num2)
					{
						enemyState.isHead = false;
					}
					else
					{
						enemyState.isHead = true;
					}
					list.Add(enemyState);
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			foreach (EnemyState item2 in list)
			{
				Transform transform = mainCamera.transform;
				Vector3 vector = ((!item2.isHead) ? item2.enemy.GetBodyPosition() : item2.enemy.GetHeadPosition());
				Vector3 normalized = (vector - transform.position).normalized;
				Ray ray = new Ray(transform.position - 1.8f * normalized, normalized);
				float distance = Vector3.Distance(transform.position, vector);
				RaycastHit hitInfo;
				if (!Physics.Raycast(ray, out hitInfo, distance, (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.TRANSPARENT_WALL) | (1 << PhysicsLayer.FLOOR)))
				{
					return item2;
				}
			}
			return null;
		}
	}

	public class NoAim : AimProcess
	{
		protected override void OnUpdate(bool hasData, Vector3 enemyPos, Vector3 deltaAngle)
		{
			SetUpdate(false);
		}
	}

	public class ImmediateAim : AimProcess
	{
		public ImmediateAim()
		{
			SetRadius(120f);
			SetProperty(10);
		}

		protected override void OnStart(bool hasData, Vector3 enemyPos, Vector3 deltaAngle)
		{
			SetRotation(new Vector2(deltaAngle.y, 0f - deltaAngle.x));
		}

		protected override void OnUpdate(bool hasData, Vector3 enemyPos, Vector3 deltaAngle)
		{
			SetRotation(Vector2.zero);
			SetUpdate(false);
		}

		protected override void OnNotifyStop(AimProcess aimProcess)
		{
			Stop();
		}
	}

	public class SynchronousAim : AimProcess
	{
		private float moveSpeed = 20f;

		public SynchronousAim()
		{
			SetRadius(40f);
			SetProperty(5);
		}

		protected override void OnStart(bool hasData, Vector3 enemyPos, Vector3 deltaAngle)
		{
			SetRotation(new Vector2(deltaAngle.y, 0f - deltaAngle.x));
		}

		protected override void OnUpdate(bool hasData, Vector3 enemyPos, Vector3 deltaAngle)
		{
			SetRotation(new Vector2(deltaAngle.y * Time.deltaTime * moveSpeed, (0f - deltaAngle.x) * Time.deltaTime * moveSpeed));
		}

		protected override void OnNotifyStop(AimProcess aimProcess)
		{
			Stop();
		}
	}

	public class DelayAim : AimProcess
	{
		private float moveSpeed = 10f;

		private Vector3 mEnemyPos;

		private Vector3 mDeltaAngle;

		private bool mHasData;

		public DelayAim()
		{
			SetRadius(80f);
			SetProperty(1);
		}

		protected override void OnStart(bool hasData, Vector3 enemyPos, Vector3 deltaAngle)
		{
			mHasData = hasData;
			mEnemyPos = enemyPos;
			mDeltaAngle = deltaAngle;
		}

		protected override void OnUpdate(bool hasData, Vector3 enemyPos, Vector3 deltaAngle)
		{
			if (!(HUDManager.instance != null))
			{
				return;
			}
			mHasData = hasData;
			mEnemyPos = enemyPos;
			mDeltaAngle = deltaAngle;
			Vector3 vector = enemyPos - HUDManager.instance.m_InfoManager.m_SightHeadState.transform.localPosition;
			if (IsNotifyStop())
			{
				SetRotation(new Vector2(deltaAngle.y * Time.deltaTime * moveSpeed, (0f - deltaAngle.x) * Time.deltaTime * moveSpeed));
				if (vector.magnitude < 1f && deltaAngle.magnitude < 1f)
				{
					Stop();
				}
			}
			HUDManager.instance.m_InfoManager.m_SightHeadState.transform.localPosition += new Vector3(vector.x * Time.deltaTime * moveSpeed, vector.y * Time.deltaTime * moveSpeed, vector.z);
		}

		protected override void OnStop(AimProcess aimProcess)
		{
			if (HUDManager.instance != null)
			{
				if (aimProcess != null && aimProcess is SynchronousAim && mHasData)
				{
					float num = Vector3.Distance(GetSightHeadCenter(), mEnemyPos);
					float num2 = Vector3.Distance(GetSightHeadCenter(), HUDManager.instance.m_InfoManager.m_SightHeadState.transform.localPosition);
					SetRotation(new Vector2(mDeltaAngle.y * num2 / num, (0f - mDeltaAngle.x) * num2 / num));
					mHasData = false;
				}
				HUDManager.instance.m_InfoManager.m_SightHeadState.transform.localPosition = GetSightHeadCenter();
			}
		}
	}

	private static NoAim NO_AIM;

	private static ImmediateAim IMMEDIATE_AIM;

	private static SynchronousAim SYNCHRONOUS_AIM;

	private static DelayAim DELAY_AIM;

	private Vector2 cameraRotation = new Vector2(0f, 0f);

	private AimProcess aimProcess;

	private AimProcess nextAimProcess;

	private bool bAimAssistEnable
	{
		get
		{
			return GameApp.GetInstance().GetGlobalState().GetAimAssist();
		}
	}

	public Vector2 CameraRotation
	{
		get
		{
			return cameraRotation;
		}
		set
		{
			cameraRotation = value;
		}
	}

	public void Init()
	{
		NO_AIM = new NoAim();
		IMMEDIATE_AIM = new ImmediateAim();
		SYNCHRONOUS_AIM = new SynchronousAim();
		DELAY_AIM = new DelayAim();
		aimProcess = NO_AIM;
	}

	public void Process()
	{
		if (bAimAssistEnable)
		{
			aimProcess.Update();
			if (aimProcess.IsStop())
			{
				aimProcess = ((nextAimProcess != null) ? nextAimProcess : NO_AIM);
				aimProcess.Init();
				nextAimProcess = null;
			}
			CheckAim();
			cameraRotation = aimProcess.GetRotation();
		}
		else
		{
			cameraRotation = Vector3.zero;
		}
	}

	private bool StartAim(AimProcess process)
	{
		if (aimProcess.Equals(process))
		{
			return false;
		}
		aimProcess.TryToStop(process);
		nextAimProcess = process;
		return true;
	}

	private void CheckAim()
	{
		LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer.InAimState)
		{
			StartAim(IMMEDIATE_AIM);
		}
		else if (localPlayer.inputController.inputInfo.fire)
		{
			StartAim(SYNCHRONOUS_AIM);
		}
		else if (Input.touchCount > 0 || Input.GetKey(KeyCode.P))
		{
			StartAim(DELAY_AIM);
		}
		else
		{
			StartAim(NO_AIM);
		}
	}
}
