using UnityEngine;

public class FirstPersonCameraScript : MonoBehaviour
{
	protected float angelH;

	protected float angelV;

	protected float lastUpdateTime;

	protected float deltaTime;

	public Vector3 eyePosition;

	public float cameraSwingSpeed;

	public float minAngelV;

	public float maxAngelV;

	public float fixedAngelV;

	public bool isAngelVFixed;

	protected LocalPlayer localPlayer;

	public Transform root;

	public Transform entity;

	public Transform spine1;

	public Transform head;

	protected CameraClipObject[] lastTransparentObjList = new CameraClipObject[5];

	protected Vector3 moveTo;

	protected bool behindWall;

	public bool lastInWall;

	protected bool started;

	public float CAMERA_NORMAL_FOV = 60f;

	public float CAMERA_AIM_SCOPE_FOV = 10f;

	public float CAMERA_AIM_NO_SCOPE_FOV = 30f;

	public float CAMERA_MORPHINE_FOV = 70f;

	public Vector3 EYEPOSITION_NORMAL;

	public Vector3 EYEPOSITION_AIM;

	public Texture reticle;

	public Texture leftTopReticle;

	public Texture rightTopReticle;

	public Texture leftBottomReticle;

	public Texture rightBottomReticle;

	protected float winTime = -1f;

	protected Vector2 reticlePosition;

	protected Transform cameraTransform;

	protected GameObject pivot;

	public AudioSource loseAudio;

	protected float weaponAngleH;

	protected float initMinAngleV;

	protected float initMaxAngleV;

	protected bool playerDyingInLastFrame;

	public Transform CameraTransform
	{
		get
		{
			return cameraTransform;
		}
	}

	public Vector2 ReticlePosition
	{
		get
		{
			return reticlePosition;
		}
		set
		{
			reticlePosition = value;
		}
	}

	public float AngelV
	{
		get
		{
			return angelV;
		}
		set
		{
			angelV = value;
		}
	}

	public float AngelH
	{
		get
		{
			return angelH;
		}
		set
		{
			angelH = value;
		}
	}

	public float WeaponAngleH
	{
		get
		{
			return weaponAngleH;
		}
	}

	public float recoilAngleV { get; set; }

	protected bool bInvertVertical
	{
		get
		{
			return !GameApp.GetInstance().GetGlobalState().GetVertivalCameraNormal();
		}
	}

	protected bool bInvertHorizontal
	{
		get
		{
			return !GameApp.GetInstance().GetGlobalState().GetHorizontalCamreraNormal();
		}
	}

	public virtual void ZoomIn(float deltaTime)
	{
		if (localPlayer.GetWeapon().HasScope)
		{
			base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, localPlayer.GetWeapon().GetScopeFOV(), deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
			eyePosition = Vector3.Lerp(eyePosition, EYEPOSITION_AIM, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		}
		else
		{
			base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_AIM_NO_SCOPE_FOV, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
			eyePosition = Vector3.Lerp(eyePosition, EYEPOSITION_AIM, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		}
	}

	public virtual void ZoomOut(float deltaTime)
	{
		if (!localPlayer.IsInMorphine)
		{
			base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_NORMAL_FOV, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
			eyePosition = Vector3.Lerp(eyePosition, EYEPOSITION_NORMAL, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		}
		else
		{
			base.GetComponent<Camera>().fov = Mathf.Lerp(base.GetComponent<Camera>().fov, CAMERA_MORPHINE_FOV, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
			eyePosition = Vector3.Lerp(eyePosition, EYEPOSITION_AIM, deltaTime * (float)Global.CAMERA_ZOOM_SPEED);
		}
	}

	public virtual void Init()
	{
		localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		root = localPlayer.GetTransform();
		entity = localPlayer.GetTransform().Find("Entity");
		entity.transform.localRotation = Quaternion.identity;
		entity.transform.localPosition = Vector3.zero;
		spine1 = localPlayer.GetTransform().Find("Entity/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1");
		head = localPlayer.GetTransform().Find("Entity/Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Neck/Bip01 Head/head_Point");
		angelH = root.rotation.eulerAngles.y;
		base.transform.rotation = Quaternion.Euler(0f - angelV, angelH, 0f);
		Screen.lockCursor = true;
		Cursor.visible = true;
		reticlePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0f);
		started = true;
		base.GetComponent<Camera>().fov = CAMERA_NORMAL_FOV;
		for (int i = 0; i < lastTransparentObjList.Length; i++)
		{
			lastTransparentObjList[i] = new CameraClipObject();
		}
		initMinAngleV = minAngelV;
		initMaxAngleV = maxAngelV;
		playerDyingInLastFrame = false;
	}

	public virtual void ResetAngleV()
	{
		minAngelV = initMinAngleV;
		maxAngelV = initMaxAngleV;
	}

	public virtual void CreateScreenBlood(float damage)
	{
	}

	private void Awake()
	{
		cameraTransform = Camera.main.transform;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
		if (!started || root == null)
		{
			return;
		}
		deltaTime = Time.deltaTime;
		RecoverToOriginalAngleV(deltaTime);
		if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			float num = 5f;
			if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.InAimState)
			{
				num *= 0.3f;
			}
			float num2 = Input.GetAxis("Mouse X") * num;
			float num3 = Input.GetAxis("Mouse Y") * num;
			localPlayer.InputController.CameraRotation = new Vector2(num2, num3);
			if (num2 != 0f || num3 != 0f)
			{
				recoilAngleV = 0f;
			}
		}
		float x = localPlayer.InputController.CameraRotation.x;
		float y = localPlayer.InputController.CameraRotation.y;
		x = ((!bInvertHorizontal) ? x : (0f - x));
		y = ((!bInvertVertical) ? y : (0f - y));
		x += localPlayer.AimAssistController.CameraRotation.x;
		y += localPlayer.AimAssistController.CameraRotation.y;
		if (!localPlayer.InputController.RotateCamera || localPlayer.InputController.Block)
		{
			x = 0f;
			y = 0f;
		}
		if (Time.timeScale != 0f)
		{
			angelH += x * cameraSwingSpeed;
			angelV += y * cameraSwingSpeed;
		}
		if (isAngelVFixed)
		{
			angelV = fixedAngelV;
		}
		angelV = Mathf.Clamp(angelV, minAngelV, maxAngelV);
		if (localPlayer.InPlayingState())
		{
			if (localPlayer.DYING_STATE.InDyingState != playerDyingInLastFrame)
			{
				playerDyingInLastFrame = localPlayer.DYING_STATE.InDyingState;
				if (localPlayer.DYING_STATE.InDyingState)
				{
					cameraTransform.rotation = Quaternion.Euler(0f, angelH, 0f);
					root.rotation = Quaternion.Euler(0f, angelH, 0f);
					entity.rotation = cameraTransform.rotation;
					localPlayer.SetEntityLocalPositionInDying();
				}
				else
				{
					cameraTransform.rotation = Quaternion.Euler(0f, angelH, 0f);
					root.rotation = Quaternion.Euler(0f, angelH, 0f);
					entity.rotation = cameraTransform.rotation;
					localPlayer.RestoreEntityLocalPosition();
					localPlayer.StartUnhurt();
				}
			}
			else
			{
				cameraTransform.rotation = Quaternion.Euler(0f - angelV, angelH, 0f);
				cameraTransform.eulerAngles += localPlayer.CameraVibrateController.CameraRotation;
				root.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
				Vector3 position = spine1.position;
				entity.rotation = cameraTransform.rotation;
				entity.position += position - spine1.position;
			}
			moveTo = entity.TransformPoint(eyePosition);
			float num4 = 100f;
			cameraTransform.position = Vector3.Lerp(cameraTransform.position, moveTo, num4 * Time.deltaTime);
		}
		else
		{
			entity.rotation = Quaternion.Euler(0f, entity.rotation.eulerAngles.y, 0f);
			cameraTransform.position = head.position;
			cameraTransform.rotation = head.rotation;
		}
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			deltaTime = 0f;
		}
	}

	private void RecoverToOriginalAngleV(float deltaTime)
	{
		float num = recoilAngleV;
		recoilAngleV -= deltaTime * 4f;
		if (recoilAngleV < 0f)
		{
			recoilAngleV = 0f;
		}
		angelV -= num - recoilAngleV;
	}
}
