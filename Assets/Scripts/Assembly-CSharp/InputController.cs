using UnityEngine;

public class InputController
{
	public InputInfo inputInfo = new InputInfo();

	public InputInfo previousInputInfo = new InputInfo();

	protected Vector3 moveDirection = Vector3.zero;

	protected TouchInfo lastMoveTouch = new TouchInfo();

	protected TouchInfo lastMoveTouch2 = new TouchInfo();

	protected float sensitivityFactor = 1f;

	protected float sensitivityAimFactor = 0.3f;

	protected Touch[] lastTouch = new Touch[2];

	protected Vector2 cameraRotation = new Vector2(0f, 0f);

	protected Vector2 deflection;

	protected Vector2 thumbCenter;

	protected Vector2 thumbCenterToScreen;

	protected Vector2 shootThumbCenter;

	protected Vector2 shootThumbCenterToScreen;

	protected Vector2 lastShootTouch = default(Vector2);

	protected float touchX;

	protected float touchY;

	protected float thumbRadius;

	protected int thumbTouchFingerId = -1;

	protected int shootingTouchFingerId = -1;

	protected int moveTouchFingerId = -1;

	protected int moveTouchFingerId2 = -1;

	protected string phaseStr = ".";

	protected Player player;

	protected Rect blockRect;

	protected bool bBlock;

	protected bool bCanRotateCamera = true;

	protected bool bCanFire = true;

	protected bool bInit;

	protected bool bPressThumbCenter;

	protected bool bPressShootThumbCenter;

	protected int pressThumbFingerId = -1;

	protected int pressShootingFingerId = -1;

	protected float initSensitivityFactor
	{
		get
		{
			return GameApp.GetInstance().GetGlobalState().TouchInputSensitivity * 1.25f + 0.25f;
		}
	}

	public bool CanFire
	{
		get
		{
			return bCanFire;
		}
		set
		{
			bCanFire = value;
		}
	}

	public bool RotateCamera
	{
		get
		{
			return bCanRotateCamera;
		}
		set
		{
			bCanRotateCamera = value;
		}
	}

	public bool Block
	{
		get
		{
			return bBlock;
		}
		set
		{
			bBlock = value;
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

	public Vector2 ThumbCenterToScreen
	{
		get
		{
			return thumbCenterToScreen;
		}
	}

	public Vector2 LastTouchPos
	{
		get
		{
			return new Vector2(thumbCenterToScreen.x + touchX * thumbRadius, thumbCenterToScreen.y + touchY * thumbRadius);
		}
	}

	public Vector2 LastShootTouch
	{
		get
		{
			return lastShootTouch;
		}
	}

	public Vector2 ShootThumbCenterToScreen
	{
		get
		{
			return shootThumbCenterToScreen;
		}
	}

	public void Init()
	{
		if (Screen.width == 1024)
		{
			thumbCenterToScreen = UserStateHUD.GetInstance().Joystick.MoveCenter;
			shootThumbCenterToScreen = UserStateHUD.GetInstance().Joystick.ShootCenter;
			thumbRadius = UserStateHUD.GetInstance().Joystick.Radius;
		}
		else
		{
			thumbCenterToScreen = UserStateHUD.GetInstance().Joystick.MoveCenter;
			shootThumbCenterToScreen = UserStateHUD.GetInstance().Joystick.ShootCenter;
			thumbRadius = UserStateHUD.GetInstance().Joystick.Radius;
		}
		Debug.Log("shootThumbCenterToScreen : " + shootThumbCenterToScreen);
		for (int i = 0; i < 2; i++)
		{
			lastTouch[i] = default(Touch);
		}
		player = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		bBlock = false;
	}

	public void Process()
	{
		Debug.Log("UserStateHUD.GetInstance().Joystick.TouchPos[i] : " + UserStateHUD.GetInstance().Joystick.Radius);
		Debug.Log("Response...." + bBlock);
		if (bBlock)
		{
			cameraRotation = Vector2.zero;
			inputInfo.moving = false;
			inputInfo.fire = false;
			moveDirection = Vector3.zero;
			inputInfo.moveDirection = Vector3.zero;
			return;
		}
		bool flag = false;
		if (Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			cameraRotation = Vector2.zero;
			if (Input.GetButton("Fire1") && bCanFire)
			{
				inputInfo.fire = true;
			}
			else
			{
				inputInfo.fire = false;
			}
			if (Input.GetButtonDown("Jump"))
			{
				inputInfo.jumpDown = true;
			}
			else
			{
				inputInfo.jumpDown = false;
			}
			if (Input.GetButtonUp("BackPack") && SkillTreeUIScript.mInstance != null)
			{
				if (SkillTreeUIScript.mInstance.IsAlreadyOpen)
				{
					SkillTreeUIScript.mInstance.HideUI();
				}
				else
				{
					SkillTreeUIScript.mInstance.ShowUI();
				}
			}
			if (Input.GetButtonUp("Weapon1"))
			{
				player.ChangeWeaponInBag(0);
			}
			if (Input.GetButtonUp("Weapon2"))
			{
				player.ChangeWeaponInBag(1);
			}
			if (Input.GetButtonUp("Weapon3"))
			{
				player.ChangeWeaponInBag(2);
			}
			if (Input.GetButtonUp("Weapon4"))
			{
				player.ChangeWeaponInBag(3);
			}
			if (Input.GetButtonDown("Item"))
			{
				InGameMenuManager.GetInstance().Show(0);
			}
			if (Input.GetButtonDown("Map"))
			{
				InGameMenuManager.GetInstance().Show(1);
			}
			if (Input.GetButtonDown("Quest"))
			{
				InGameMenuManager.GetInstance().Show(2);
			}
			if (Input.GetButtonDown("Achievement"))
			{
				InGameMenuManager.GetInstance().Show(4);
			}
			if (Input.GetButtonDown("Option"))
			{
				InGameMenuManager.GetInstance().Show(5);
			}
			if (Input.GetButtonDown("Skill"))
			{
				InGameMenuManager.GetInstance().Show(3);
			}
			if (Input.GetButtonDown("Skill1"))
			{
				UserStateHUD.GetInstance().Skill1.Apply();
			}
			if (Input.GetButtonDown("Skill2"))
			{
				UserStateHUD.GetInstance().Skill2.Apply();
			}
			if (Input.GetButtonDown("Reload"))
			{
				player.Reload();
			}
			if (Input.GetButtonDown("Grenade"))
			{
				player.ThrowGrenade();
			}
			if (Input.GetButtonDown("MeleeAttack"))
			{
				player.MeleeAttack();
			}
			if (Input.GetButtonDown("Aim") && player.State != Player.RELOAD_STATE)
			{
				if (player.InAimState)
				{
					player.Aim(false);
				}
				else
				{
					player.Aim(true);
				}
			}
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		}
		else
		{
			if (!bInit && UserStateHUD.GetInstance().Joystick.IsInit)
			{
				bInit = true;
				thumbCenterToScreen = UserStateHUD.GetInstance().Joystick.MoveCenter;
				shootThumbCenterToScreen = UserStateHUD.GetInstance().Joystick.ShootCenter;
				thumbRadius = UserStateHUD.GetInstance().Joystick.Radius;
			}
			player = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			float num = ((!player.InAimState) ? 1f : sensitivityAimFactor);
			touchX = 0f;
			touchY = 0f;
			cameraRotation.x = 0f;
			cameraRotation.y = 0f;
			inputInfo.fire = false;
			inputInfo.moving = false;
			if (Input.touchCount == 0)
			{
				thumbTouchFingerId = -1;
				shootingTouchFingerId = -1;
				lastShootTouch = shootThumbCenterToScreen;
			}
			sensitivityFactor = initSensitivityFactor;
			bool flag2 = false;
			for (int i = 0; i < Input.touchCount && i != 2; i++)
			{
				Touch touch = Input.GetTouch(i);
				UserStateHUD.GetInstance().Joystick.TouchPos[i] = touch.position;
				flag = blockRect.Contains(touch.position);
				Vector2 vector = touch.position - thumbCenterToScreen;
				bool flag3 = vector.sqrMagnitude < thumbRadius * thumbRadius;
				bool flag4 = touch.fingerId == thumbTouchFingerId;
				Vector2 vector2 = touch.position - shootThumbCenterToScreen;
				bool flag5 = vector2.sqrMagnitude < thumbRadius * thumbRadius;
				if (touch.phase == TouchPhase.Began)
				{
					if (!UserStateHUD.GetInstance().Joystick.IsFixed && touch.position.x < (float)Screen.width * UserStateHUD.GetInstance().Joystick.MoveJoystickRatio)
					{
						UserStateHUD.GetInstance().Joystick.MoveCenter = (thumbCenterToScreen = touch.position);
						flag3 = (touch.position - thumbCenterToScreen).sqrMagnitude < thumbRadius * thumbRadius;
						flag4 = touch.fingerId == thumbTouchFingerId;
					}
					if (flag3 || flag4)
					{
						bPressThumbCenter = true;
						pressThumbFingerId = touch.fingerId;
						UserStateHUD.GetInstance().Joystick.isMoveJoystickPressed = true;
					}
					if (flag5)
					{
						bPressShootThumbCenter = true;
						pressShootingFingerId = touch.fingerId;
					}
				}
				else if (touch.phase == TouchPhase.Stationary)
				{
					if ((flag3 || flag4) && bPressThumbCenter)
					{
						if (flag3)
						{
							touchX = vector.x / thumbRadius;
							touchY = vector.y / thumbRadius;
						}
						else
						{
							touchX = vector.x / thumbRadius;
							touchY = vector.y / thumbRadius;
							if (Mathf.Abs(touchX) > Mathf.Abs(touchY))
							{
								touchY /= Mathf.Abs(touchX);
								touchX = ((touchX > 0f) ? 1 : (-1));
							}
							else if (touchY != 0f)
							{
								touchX /= Mathf.Abs(touchY);
								touchY = ((touchY > 0f) ? 1 : (-1));
							}
							else
							{
								touchX = 0f;
								touchY = 0f;
							}
						}
						thumbTouchFingerId = touch.fingerId;
						inputInfo.moving = true;
					}
					else if (bPressShootThumbCenter)
					{
						bool flag6 = vector2.sqrMagnitude > thumbRadius * thumbRadius / 25f;
						flag2 = vector2.sqrMagnitude < thumbRadius * 0.1f * (thumbRadius * 0.1f);
						if ((flag5 || shootingTouchFingerId == touch.fingerId) && bCanFire)
						{
							inputInfo.fire = true;
							shootingTouchFingerId = touch.fingerId;
						}
					}
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					if ((flag3 || flag4) && bPressThumbCenter)
					{
						if (flag3)
						{
							touchX = vector.x / thumbRadius;
							touchY = vector.y / thumbRadius;
						}
						else
						{
							touchX = vector.x / thumbRadius;
							touchY = vector.y / thumbRadius;
							if (Mathf.Abs(touchX) > Mathf.Abs(touchY))
							{
								touchY /= Mathf.Abs(touchX);
								touchX = ((touchX > 0f) ? 1 : (-1));
							}
							else if (touchY != 0f)
							{
								touchX /= Mathf.Abs(touchY);
								touchY = ((touchY > 0f) ? 1 : (-1));
							}
							else
							{
								touchX = 0f;
								touchY = 0f;
							}
						}
						thumbTouchFingerId = touch.fingerId;
						inputInfo.moving = true;
					}
					else
					{
						if (lastMoveTouch.phase == TouchPhase.Moved)
						{
							if (touch.fingerId == moveTouchFingerId)
							{
								cameraRotation.x = (touch.position.x - lastMoveTouch.position.x) * 0.3f * sensitivityFactor * num;
								cameraRotation.y = (touch.position.y - lastMoveTouch.position.y) * 0.16f * sensitivityFactor * num;
							}
							else if (touch.fingerId == moveTouchFingerId2)
							{
								cameraRotation.x = (touch.position.x - lastMoveTouch2.position.x) * 0.3f * sensitivityFactor * num;
								cameraRotation.y = (touch.position.y - lastMoveTouch2.position.y) * 0.16f * sensitivityFactor * num;
							}
							FirstPersonCameraScript camera = GameApp.GetInstance().GetGameScene().GetCamera();
							camera.recoilAngleV = 0f;
							UserStateHUD.GetInstance().Joystick.isPressedToRotateCamera = true;
						}
						if (moveTouchFingerId == -1)
						{
							moveTouchFingerId = touch.fingerId;
						}
						if (moveTouchFingerId != -1 && touch.fingerId != moveTouchFingerId)
						{
							moveTouchFingerId2 = touch.fingerId;
						}
						if (touch.fingerId == moveTouchFingerId)
						{
							lastMoveTouch.phase = TouchPhase.Moved;
							lastMoveTouch.position = touch.position;
						}
						if (touch.fingerId == moveTouchFingerId2)
						{
							lastMoveTouch2.phase = TouchPhase.Moved;
							lastMoveTouch2.position = touch.position;
						}
						bool flag7 = vector2.sqrMagnitude > thumbRadius * thumbRadius / 25f;
						flag2 = vector2.sqrMagnitude < thumbRadius * 0.1f * (thumbRadius * 0.1f);
						if (bPressShootThumbCenter && (shootingTouchFingerId == touch.fingerId || flag5) && bCanFire)
						{
							inputInfo.fire = true;
							if (flag5)
							{
								if (flag7)
								{
									cameraRotation.x += Mathf.Clamp(vector2.x, 0f - thumbRadius, thumbRadius) * 0.002f * sensitivityFactor * num;
									lastShootTouch = touch.position;
								}
							}
							else
							{
								Vector2 normalized = (touch.position - shootThumbCenterToScreen).normalized;
								lastShootTouch = shootThumbCenterToScreen + normalized * thumbRadius;
								cameraRotation.x += Mathf.Sign(vector2.x) * thumbRadius * 0.006f * sensitivityFactor * num;
							}
							shootingTouchFingerId = touch.fingerId;
						}
					}
				}
				else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
				{
					UserStateHUD.GetInstance().Joystick.isPressedToRotateCamera = false;
					if (touch.fingerId == thumbTouchFingerId)
					{
						thumbTouchFingerId = -1;
					}
					if (touch.fingerId == shootingTouchFingerId)
					{
						shootingTouchFingerId = -1;
						lastShootTouch = shootThumbCenterToScreen;
					}
					if (touch.fingerId == moveTouchFingerId)
					{
						moveTouchFingerId = -1;
						lastMoveTouch.phase = TouchPhase.Ended;
					}
					if (touch.fingerId == moveTouchFingerId2)
					{
						moveTouchFingerId2 = -1;
						lastMoveTouch2.phase = TouchPhase.Ended;
					}
					if (touch.fingerId == pressThumbFingerId)
					{
						pressThumbFingerId = -1;
						bPressThumbCenter = false;
						UserStateHUD.GetInstance().Joystick.isMoveJoystickPressed = false;
					}
					if (touch.fingerId == pressShootingFingerId)
					{
						pressShootingFingerId = -1;
						bPressShootThumbCenter = false;
					}
				}
				lastTouch[i] = touch;
			}
			moveDirection = new Vector3(touchX, 0f, touchY);
		}
		if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
		{
			if (moveDirection.x > 0f)
			{
				inputInfo.dir = MoveDirection.right;
			}
			else
			{
				inputInfo.dir = MoveDirection.left;
			}
		}
		else if (moveDirection.z > 0f)
		{
			inputInfo.dir = MoveDirection.forward;
		}
		else
		{
			inputInfo.dir = MoveDirection.back;
		}
		float magnitude = moveDirection.magnitude;
		if (magnitude > 0f)
		{
			moveDirection.x /= moveDirection.magnitude;
			moveDirection.z /= moveDirection.magnitude;
			switch (inputInfo.dir)
			{
			case MoveDirection.back:
				moveDirection.x *= 0.7f;
				moveDirection.z *= 0.7f;
				break;
			case MoveDirection.left:
			case MoveDirection.right:
				moveDirection.x *= 0.8f;
				moveDirection.z *= 0.8f;
				break;
			}
		}
		FirstPersonCameraScript component = Camera.main.GetComponent<FirstPersonCameraScript>();
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetTransform()
			.Rotate(new Vector3(0f, 0f - component.WeaponAngleH, 0f));
		moveDirection = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetTransform()
			.TransformDirection(moveDirection);
		GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
			.GetTransform()
			.Rotate(new Vector3(0f, component.WeaponAngleH, 0f));
		if (player.State == Player.FALL_DOWN_STATE || player.InDyingState())
		{
			moveDirection = Vector3.zero;
		}
		inputInfo.moveDirection = moveDirection;
		if (flag)
		{
			cameraRotation.x = 0f;
			cameraRotation.y = 0f;
		}
	}
}
