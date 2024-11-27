using UnityEngine;

public class TDMCapturePointScript : MonoBehaviour
{
	public int PointID;

	public float StartCaptureTime = 3f;

	public GameObject CanCaptureEffect;

	public GameObject CapturingEffect;

	public GameObject LockCaptureEffect;

	protected Timer StartCaptureTimer = new Timer();

	protected Timer InsideIntervalTimer = new Timer();

	protected float mInsideIntervalTime = 1f;

	protected bool mIsStartCapture;

	protected int mPrevOwner = -1;

	protected GameObject mFX;

	protected Timer waitTimer = new Timer();

	protected bool mChange;

	private void Start()
	{
		mIsStartCapture = false;
		StartCaptureTimer.Disable();
		waitTimer.SetTimer(1.5f, false);
		mChange = false;
		if (CanCaptureEffect != null)
		{
			CanCaptureEffect.SetActive(false);
		}
		if (CapturingEffect != null)
		{
			CapturingEffect.SetActive(false);
		}
		if (LockCaptureEffect != null)
		{
			LockCaptureEffect.SetActive(false);
		}
	}

	private void Update()
	{
		VSTDMManager vSTDMManager = (VSTDMManager)GameApp.GetInstance().GetVSManager();
		if (mChange)
		{
			if (!waitTimer.Ready())
			{
				return;
			}
			mChange = false;
			if (vSTDMManager != null && vSTDMManager.pointInfo.ContainsKey(PointID))
			{
				byte owner = vSTDMManager.pointInfo[PointID].GetOwner();
				string text = "RPG_JD_002";
				switch (owner)
				{
				case 2:
					text = "RPG_JD_003";
					break;
				case 1:
					text = "RPG_JD_001";
					break;
				}
				mPrevOwner = owner;
				Object.Destroy(mFX);
				GameObject original = Resources.Load("RPG_effect/" + text) as GameObject;
				mFX = Object.Instantiate(original) as GameObject;
				mFX.transform.parent = base.transform;
				mFX.transform.localPosition = Vector3.zero;
				Debug.Log("changed " + owner);
			}
		}
		else if (vSTDMManager != null && vSTDMManager.pointInfo.ContainsKey(PointID))
		{
			byte owner2 = vSTDMManager.pointInfo[PointID].GetOwner();
			if (owner2 != mPrevOwner)
			{
				mChange = true;
				waitTimer.Do();
				Debug.Log("ready: " + owner2);
				switch (owner2)
				{
				case 2:
				{
					GameObject original3 = Resources.Load("RPG_effect/PVP_change_002") as GameObject;
					GameObject gameObject2 = Object.Instantiate(original3) as GameObject;
					gameObject2.transform.parent = base.transform;
					gameObject2.transform.localPosition = Vector3.zero;
					break;
				}
				case 1:
				{
					GameObject original2 = Resources.Load("RPG_effect/PVP_change_001") as GameObject;
					GameObject gameObject = Object.Instantiate(original2) as GameObject;
					gameObject.transform.parent = base.transform;
					gameObject.transform.localPosition = Vector3.zero;
					break;
				}
				}
			}
		}
		if (mIsStartCapture)
		{
			if (CanCaptureEffect != null && CanCaptureEffect.activeSelf)
			{
				CanCaptureEffect.SetActive(false);
			}
			if (CapturingEffect != null && !CapturingEffect.activeSelf)
			{
				CapturingEffect.SetActive(true);
			}
			if (LockCaptureEffect != null && LockCaptureEffect.activeSelf)
			{
				LockCaptureEffect.SetActive(false);
			}
		}
		else if (CanCapture())
		{
			if (CanCaptureEffect != null && !CanCaptureEffect.activeSelf)
			{
				CanCaptureEffect.SetActive(true);
			}
			if (CapturingEffect != null && CapturingEffect.activeSelf)
			{
				CapturingEffect.SetActive(false);
			}
			if (LockCaptureEffect != null && LockCaptureEffect.activeSelf)
			{
				LockCaptureEffect.SetActive(false);
			}
		}
		else
		{
			if (CanCaptureEffect != null && CanCaptureEffect.activeSelf)
			{
				CanCaptureEffect.SetActive(false);
			}
			if (CapturingEffect != null && CapturingEffect.activeSelf)
			{
				CapturingEffect.SetActive(false);
			}
			if (LockCaptureEffect != null && !LockCaptureEffect.activeSelf)
			{
				LockCaptureEffect.SetActive(true);
			}
		}
	}

	private void OnTriggerEnter(Collider c)
	{
		if (c.GetComponent<Collider>().gameObject.layer == PhysicsLayer.PLAYER_COLLIDER && CanCapture())
		{
			UserStateHUD.GetInstance().SetUserInBattleFieldPoint(PointID);
		}
	}

	protected virtual void OnTriggerExit(Collider c)
	{
		if (c.GetComponent<Collider>().gameObject.layer == PhysicsLayer.PLAYER_COLLIDER)
		{
			mIsStartCapture = false;
			StartCaptureTimer.Disable();
			UserStateHUD.GetInstance().SetUserOutBattleFieldPoint();
		}
	}

	private void OnTriggerStay(Collider c)
	{
		if (c.GetComponent<Collider>().gameObject.layer != PhysicsLayer.PLAYER_COLLIDER || !CanCapture())
		{
			return;
		}
		if (!mIsStartCapture)
		{
			if (!StartCaptureTimer.Enable())
			{
				StartCaptureTimer.SetTimer(StartCaptureTime, false);
				Debug.Log("Enter Point " + PointID);
			}
			else if (StartCaptureTimer.Ready())
			{
				mIsStartCapture = true;
				InsideIntervalTimer.SetTimer(mInsideIntervalTime, false);
				Debug.Log("Start Capture Point " + PointID);
			}
		}
		else if (InsideIntervalTimer.Ready())
		{
			InsideIntervalTimer.Do();
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer() && GameApp.GetInstance().GetGameMode().IsVSMode())
			{
				VSTDMCapturingPointRequest request = new VSTDMCapturingPointRequest((short)PointID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
		}
	}

	public bool CanCapture()
	{
		bool result = false;
		VSTDMManager vSTDMManager = (VSTDMManager)GameApp.GetInstance().GetVSManager();
		if (vSTDMManager != null)
		{
			result = vSTDMManager.PointCanCapture(PointID) && GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InPlayingState();
		}
		return result;
	}

	public bool IsSameTeamWithLocalPlayer()
	{
		bool result = false;
		VSTDMManager vSTDMManager = (VSTDMManager)GameApp.GetInstance().GetVSManager();
		if (vSTDMManager != null && GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.Team == TeamName.Red && vSTDMManager.pointInfo[PointID].GetOwner() == 1 && GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.Team == TeamName.Blue && vSTDMManager.pointInfo[PointID].GetOwner() == 2)
		{
			result = true;
		}
		return result;
	}
}
