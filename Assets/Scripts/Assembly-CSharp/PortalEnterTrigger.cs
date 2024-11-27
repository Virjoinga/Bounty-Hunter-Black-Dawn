using UnityEngine;

public class PortalEnterTrigger : MonoBehaviour, EffectsCameraListener
{
	public static PortalEnterTrigger instance;

	public GameObject portalEffect;

	private string destName;

	private int destIndex;

	private float updateTime;

	private bool triggerEnabled;

	private void Awake()
	{
	}

	private void Start()
	{
		instance = this;
		updateTime = Time.time;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void LateUpdate()
	{
		if (!(portalEffect != null) || !((double)(Time.time - updateTime) > 2.0))
		{
			return;
		}
		updateTime = Time.time;
		if (TutorialManager.GetInstance().IsFirstTutorialOk() && TutorialManager.GetInstance().IsShopTutorialOk())
		{
			if (!portalEffect.activeSelf)
			{
				portalEffect.SetActive(true);
			}
		}
		else if (portalEffect.activeSelf)
		{
			portalEffect.SetActive(false);
		}
	}

	public void SetDest(string name, int index)
	{
		destName = name;
		destIndex = index;
		Debug.Log("destName : " + destName);
		Debug.Log("destIndex : " + destIndex);
		GameApp.GetInstance().GetGameWorld().PortalID = destIndex;
	}

	public void OnEffectsEnd(EffectsCamera.Type type)
	{
		EffectsCamera.instance.RemoveListener();
		GameScene gameScene = GameApp.GetInstance().GetGameScene();
		gameScene.LeaveScene();
		Debug.Log(destName);
		Application.LoadLevel(destName);
	}

	private void OnTriggerEnter(Collider obj)
	{
		if (TutorialManager.GetInstance().IsFirstTutorialOk() && TutorialManager.GetInstance().IsShopTutorialOk())
		{
			Debug.Log("OnTriggerEnter : " + obj);
			destName = null;
			UIPortal.Show();
		}
	}

	private void OnTriggerStay(Collider obj)
	{
		if (destName == null || triggerEnabled)
		{
			return;
		}
		triggerEnabled = true;
		if (EffectsCamera.instance != null)
		{
			if (GameApp.GetInstance().GetGameMode().IsMultiPlayer())
			{
				InvitationRequest request = new InvitationRequest(GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetUserID(), InvitationRequest.Type.Story, SubMode.Story, GameApp.GetInstance().GetUserState().GetSceneId(destName), (short)GameApp.GetInstance().GetGameWorld().PortalID);
				GameApp.GetInstance().GetNetworkManager().SendRequest(request);
			}
			else
			{
				EffectsCamera.instance.StartEffect(EffectsCamera.Type.FadeIn, this);
			}
		}
	}

	private void OnTriggerExit(Collider obj)
	{
		if (triggerEnabled)
		{
			triggerEnabled = false;
		}
	}
}
