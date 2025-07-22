using System.Collections.Generic;
using UnityEngine;

internal class AvatarBuilder
{
	protected static AvatarBuilder instance = new AvatarBuilder();

	protected UserState lastUserState;

	public static GameObject gggo;

	public static AvatarBuilder GetInstance()
	{
		return instance;
	}

	public void BindBones(SkinnedMeshRenderer smr, List<Transform> bones, GameObject parentObj)
	{
		Transform[] array = new Transform[smr.bones.Length];
		for (int i = 0; i < smr.bones.Length; i++)
		{
			string name = smr.bones[i].name;
			for (int j = 0; j < bones.Count; j++)
			{
				if (name == bones[j].name)
				{
					array[i] = bones[j];
					break;
				}
			}
		}
		smr.bones = array;
		smr.transform.parent = parentObj.transform;
	}

	public void TraverseBones(Transform t, List<Transform> bones)
	{
		bones.Add(t);
		if (t.childCount > 0)
		{
			for (int i = 0; i < t.childCount; i++)
			{
				TraverseBones(t.GetChild(i), bones);
			}
		}
	}

	public void AddAnimation(string path, string name, GameObject playerObject)
	{
		GameObject original = Resources.Load(path + name) as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		playerObject.GetComponent<Animation>().AddClip(gameObject.GetComponent<Animation>()[name].clip, name);
		playerObject.GetComponent<Animation>().Play(name);
		Object.Destroy(gameObject);
	}

	public void AddAnimationsForThirdPerson(GameObject player, Player p)
	{
		UserState userState = p.GetUserState();
		List<Weapon> weaponList = p.GetWeaponList();
		HashSet<WeaponType> hashSet = new HashSet<WeaponType>();
		string path = string.Concat("Character/Soldier/Animation/Soldier", userState.GetSex(), "_T/");
		if (userState.GetCharacterClass() == CharacterClass.Engineer && userState.GetSex() == Sex.F)
		{
			path = "Character/Engineer/Animation/EngineerF_T/";
		}
		else if ((userState.GetCharacterClass() == CharacterClass.Prayer || userState.GetCharacterClass() == CharacterClass.Sniper) && userState.GetSex() == Sex.M)
		{
			path = "Character/Prayer/Animation/PrayerM_T/";
		}
		foreach (Weapon item in weaponList)
		{
			if (item != null)
			{
				hashSet.Add(item.GetWeaponType());
			}
		}
		AddAnimation(path, "dead", player);
		AddAnimation(path, "dying", player);
		AddAnimation(path, "knife_melee", player);
		if (hashSet.Contains(WeaponType.AssaultRifle) || hashSet.Contains(WeaponType.SubMachineGun))
		{
			AddAnimation(path, "SMG01_idle", player);
			AddAnimation(path, "SMG01_fire", player);
			AddAnimation(path, "SMG01_run_back", player);
			AddAnimation(path, "SMG01_run_forward", player);
			AddAnimation(path, "SMG01_run_left", player);
			AddAnimation(path, "SMG01_run_right", player);
			AddAnimation(path, "SMG01_run_fire_back", player);
			AddAnimation(path, "SMG01_run_fire_forward", player);
			AddAnimation(path, "SMG01_run_fire_left", player);
			AddAnimation(path, "SMG01_run_fire_right", player);
		}
		if (hashSet.Contains(WeaponType.Pistol) || hashSet.Contains(WeaponType.Revolver))
		{
			AddAnimation(path, "revolver01_idle", player);
			AddAnimation(path, "revolver01_fire", player);
			AddAnimation(path, "revolver01_run_back", player);
			AddAnimation(path, "revolver01_run_forward", player);
			AddAnimation(path, "revolver01_run_left", player);
			AddAnimation(path, "revolver01_run_right", player);
			AddAnimation(path, "revolver01_run_fire_back", player);
			AddAnimation(path, "revolver01_run_fire_forward", player);
			AddAnimation(path, "revolver01_run_fire_left", player);
			AddAnimation(path, "revolver01_run_fire_right", player);
		}
		if (hashSet.Contains(WeaponType.ShotGun))
		{
			AddAnimation(path, "shootgun01_idle", player);
			AddAnimation(path, "shootgun01_fire", player);
			AddAnimation(path, "shootgun01_run_back", player);
			AddAnimation(path, "shootgun01_run_forward", player);
			AddAnimation(path, "shootgun01_run_left", player);
			AddAnimation(path, "shootgun01_run_right", player);
			AddAnimation(path, "shootgun01_run_fire_back", player);
			AddAnimation(path, "shootgun01_run_fire_forward", player);
			AddAnimation(path, "shootgun01_run_fire_left", player);
			AddAnimation(path, "shootgun01_run_fire_right", player);
		}
		if (hashSet.Contains(WeaponType.RPG))
		{
			AddAnimation(path, "rpg01_idle", player);
			AddAnimation(path, "rpg01_fire", player);
			AddAnimation(path, "rpg01_run_back", player);
			AddAnimation(path, "rpg01_run_forward", player);
			AddAnimation(path, "rpg01_run_left", player);
			AddAnimation(path, "rpg01_run_right", player);
			AddAnimation(path, "rpg01_run_fire_back", player);
			AddAnimation(path, "rpg01_run_fire_forward", player);
			AddAnimation(path, "rpg01_run_fire_left", player);
			AddAnimation(path, "rpg01_run_fire_right", player);
		}
		if (hashSet.Contains(WeaponType.Sniper))
		{
			AddAnimation(path, "sniper_rifle_idle", player);
			AddAnimation(path, "sniper_rifle_fire", player);
			AddAnimation(path, "sniper_rifle_run_back", player);
			AddAnimation(path, "sniper_rifle_run_forward", player);
			AddAnimation(path, "sniper_rifle_run_left", player);
			AddAnimation(path, "sniper_rifle_run_right", player);
			AddAnimation(path, "sniper_rifle_run_fire_back", player);
			AddAnimation(path, "sniper_rifle_run_fire_forward", player);
			AddAnimation(path, "sniper_rifle_run_fire_left", player);
			AddAnimation(path, "sniper_rifle_run_fire_right", player);
		}
	}

	public void AddAnimationsForFirstPerson(GameObject player, Player p)
	{
		UserState userState = GameApp.GetInstance().GetUserState();
		List<Weapon> weaponList = p.GetWeaponList();
		HashSet<WeaponType> hashSet = new HashSet<WeaponType>();
		string path = string.Concat("Character/Soldier/Animation/Soldier", userState.GetSex(), "/");
		foreach (Weapon item in weaponList)
		{
			if (item != null)
			{
				hashSet.Add(item.GetWeaponType());
			}
		}
		bool flag = false;
		AddAnimation(path, AnimationString.ThrowGrenade, player);
		AddAnimation(path, AnimationString.MeleeAttack, player);
		AddAnimation(path, AnimationString.Dead, player);
		if (flag)
		{
			AddAnimation(path, "fly_idle", player);
			AddAnimation(path, "fly_front", player);
			AddAnimation(path, "fly_back", player);
			AddAnimation(path, "fly_left", player);
			AddAnimation(path, "fly_right", player);
		}
		if ((hashSet.Contains(WeaponType.AssaultRifle) || hashSet.Contains(WeaponType.LaserGun) || hashSet.Contains(WeaponType.LaserRifle) || hashSet.Contains(WeaponType.PlasmaNeo) || hashSet.Contains(WeaponType.SubMachineGun)) && !flag)
		{
			AddAnimation(path, "SMG01_idle", player);
			AddAnimation(path, "SMG01_fire", player);
			AddAnimation(path, "SMG01_aimdown_idle", player);
			AddAnimation(path, "SMG01_aimdown_fire", player);
			AddAnimation(path, "SMG01_reload", player);
			AddAnimation(path, "SMG01_switch01", player);
			AddAnimation(path, "SMG01_switch02", player);
		}
		if ((hashSet.Contains(WeaponType.ShotGun) || hashSet.Contains(WeaponType.GrenadeLauncher) || hashSet.Contains(WeaponType.AdvancedShotGun)) && !flag)
		{
			AddAnimation(path, "shootgun01_idle", player);
			AddAnimation(path, "shootgun01_fire", player);
			AddAnimation(path, "shootgun01_aimdown_idle", player);
			AddAnimation(path, "shootgun01_aimdown_fire", player);
			AddAnimation(path, "shootgun01_reload", player);
			AddAnimation(path, "shootgun01_switch01", player);
			AddAnimation(path, "shootgun01_switch02", player);
		}
		if (hashSet.Contains(WeaponType.Revolver))
		{
			AddAnimation(path, "revolver01_idle", player);
			AddAnimation(path, "revolver01_fire", player);
			AddAnimation(path, "revolver01_aimdown_idle", player);
			AddAnimation(path, "revolver01_aimdown_fire", player);
			AddAnimation(path, "revolver01_reload", player);
			AddAnimation(path, "revolver01_switch01", player);
			AddAnimation(path, "revolver01_switch02", player);
		}
		if (hashSet.Contains(WeaponType.Pistol) && !flag)
		{
			AddAnimation(path, "pistol01_idle", player);
			AddAnimation(path, "pistol01_fire", player);
			AddAnimation(path, "pistol01_aimdown_idle", player);
			AddAnimation(path, "pistol01_aimdown_fire", player);
			AddAnimation(path, "pistol01_reload", player);
			AddAnimation(path, "pistol01_switch01", player);
			AddAnimation(path, "pistol01_switch02", player);
		}
		if (hashSet.Contains(WeaponType.Sniper) && !flag)
		{
			AddAnimation(path, "sniper_rifle_idle", player);
			AddAnimation(path, "sniper_rifle_fire", player);
			AddAnimation(path, "sniper_rifle_aimdown_idle", player);
			AddAnimation(path, "sniper_rifle_aimdown_fire", player);
			AddAnimation(path, "sniper_rifle_reload", player);
			AddAnimation(path, "sniper_rifle_switch01", player);
			AddAnimation(path, "sniper_rifle_switch02", player);
		}
		if ((hashSet.Contains(WeaponType.RPG) || hashSet.Contains(WeaponType.RocketLauncher) || hashSet.Contains(WeaponType.AutoRocketLauncher)) && !flag)
		{
			AddAnimation(path, "rpg01_idle", player);
			AddAnimation(path, "rpg01_fire", player);
			AddAnimation(path, "rpg01_reload", player);
			AddAnimation(path, "rpg01_switch01", player);
			AddAnimation(path, "rpg01_switch02", player);
		}
		if (hashSet.Contains(WeaponType.Sword) || hashSet.Contains(WeaponType.AdvancedSword))
		{
		}
		if (hashSet.Contains(WeaponType.GrenadeLauncher) || hashSet.Contains(WeaponType.AdvancedShotGun))
		{
		}
		if (hashSet.Contains(WeaponType.AutoRocketLauncher))
		{
		}
		if (hashSet.Contains(WeaponType.LaserGun))
		{
		}
		if (hashSet.Contains(WeaponType.LightBow) || hashSet.Contains(WeaponType.AutoBow))
		{
		}
		if (hashSet.Contains(WeaponType.LightFist))
		{
		}
		if (!hashSet.Contains(WeaponType.MachineGun))
		{
		}
	}

	public void AddAnimationsForUI(GameObject player, string path, WeaponType weaponType)
	{
		lastUserState = null;
		switch (weaponType)
		{
		case WeaponType.SubMachineGun:
		case WeaponType.AssaultRifle:
			AddAnimation(path, "SMG01_ui", player);
			break;
		case WeaponType.Pistol:
		case WeaponType.Revolver:
			AddAnimation(path, "revolver01_ui", player);
			break;
		case WeaponType.RPG:
			AddAnimation(path, "rpg01_ui", player);
			break;
		case WeaponType.Sniper:
			AddAnimation(path, "sniper_rifle_ui", player);
			break;
		case WeaponType.ShotGun:
			AddAnimation(path, "shootgun01_ui", player);
			break;
		default:
			AddAnimation(path, "SMG01_ui", player);
			break;
		}
	}

	public GameObject ReBuildAvatar(UserState userState, Player p)
	{
		byte[] array = new byte[5] { 1, 1, 1, 1, 1 };
		if (userState != null)
		{
			array = userState.GetDecoration();
		}
		lastUserState = userState;
		Object original = Resources.Load("Avatar/Bone");
		Object original2 = Resources.Load(string.Format("Avatar/{0:D2}/Head", array[0] + 1));
		Object original3 = Resources.Load(string.Format("Avatar/{0:D2}/Body", array[1] + 1));
		Object original4 = Resources.Load(string.Format("Avatar/{0:D2}/Hand", array[2] + 1));
		Object original5 = Resources.Load(string.Format("Avatar/{0:D2}/Foot", array[3] + 1));
		Object original6 = Resources.Load(string.Format("Avatar/{0:D2}/Bag", array[4] + 1));
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		GameObject gameObject2 = Object.Instantiate(original3) as GameObject;
		GameObject gameObject3 = Object.Instantiate(original2) as GameObject;
		GameObject gameObject4 = Object.Instantiate(original4) as GameObject;
		GameObject gameObject5 = Object.Instantiate(original5) as GameObject;
		GameObject gameObject6 = Object.Instantiate(original6) as GameObject;
		gameObject6.SetActive(false);
		if (array[1] == 5)
		{
			gameObject6.transform.localScale = Vector3.one * WeaponResourceConfig.GetBagSize(array[4]);
		}
		else
		{
			gameObject6.transform.localScale = Vector3.one * WeaponResourceConfig.GetBagSize(array[4]) * 0.8f;
		}
		if (array[4] == 15)
		{
			FlyBagAnimationScript component = gameObject6.GetComponent<FlyBagAnimationScript>();
		}
		List<Transform> list = new List<Transform>();
		Transform transform = gameObject.transform;
		TraverseBones(transform.GetChild(0), list);
		for (int i = 0; i < list.Count; i++)
		{
		}
		SkinnedMeshRenderer component2 = gameObject2.transform.Find("body").GetComponent<SkinnedMeshRenderer>();
		SkinnedMeshRenderer component3 = gameObject3.transform.Find("head").GetComponent<SkinnedMeshRenderer>();
		SkinnedMeshRenderer component4 = gameObject4.transform.Find("hand").GetComponent<SkinnedMeshRenderer>();
		SkinnedMeshRenderer component5 = gameObject5.transform.Find("foot").GetComponent<SkinnedMeshRenderer>();
		BindBones(component2, list, gameObject);
		BindBones(component3, list, gameObject);
		BindBones(component4, list, gameObject);
		BindBones(component5, list, gameObject);
		component2.enabled = false;
		component3.enabled = false;
		component4.enabled = false;
		component5.enabled = false;
		gameObject6.transform.position = gameObject.transform.Find(BoneName.Bag).position;
		if (array[1] == 5)
		{
			gameObject6.transform.position = gameObject.transform.Find(BoneName.Bag).position + Vector3.forward * -0.05f + Vector3.up * 0.03f;
		}
		gameObject6.transform.parent = gameObject.transform.Find(BoneName.Bag);
		gameObject6.name = "Bag";
		Object.Destroy(gameObject2);
		Object.Destroy(gameObject3);
		Object.Destroy(gameObject4);
		Object.Destroy(gameObject5);
		gameObject.transform.position = new Vector3(4f, 0f, -5f);
		gameObject.GetComponent<Animation>().Play(AnimationString.Idle + "_rifle");
		return gameObject;
	}

	public GameObject CreateFirstPersonAvatar(UserState userState, Player p)
	{
		byte[] array = new byte[5] { 1, 1, 1, 1, 1 };
		if (userState != null)
		{
			array = userState.GetDecoration();
		}
		lastUserState = userState;
		string path = string.Concat("Character/", userState.GetCharacterClass(), "/", userState.GetCharacterClass(), userState.GetSex());
		Object original = Resources.Load(path);
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.position = new Vector3(4f, 0f, -5f);
		return gameObject;
	}

	public GameObject CreateThirdPersonAvatar(UserState userState, Player p)
	{
		byte[] array = new byte[5] { 1, 1, 1, 1, 1 };
		if (userState != null)
		{
			array = userState.GetDecoration();
		}
		lastUserState = userState;
		string path = string.Concat("Character/", userState.GetCharacterClass(), "/", userState.GetRoleState().avatarID, "/", userState.GetCharacterClass(), userState.GetSex(), "_T");
		Object original = Resources.Load(path);
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.position = new Vector3(4f, 0f, -5f);
		GameObject original2 = Resources.Load("Effect/Shadow") as GameObject;
		GameObject gameObject2 = Object.Instantiate(original2) as GameObject;
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.up * 0.1f;
		gameObject2.transform.localScale = Vector3.one;
		return gameObject;
	}

	private void ChangeShaderForPlayer(GameObject player)
	{
	}

	private void ChangeShaderForWeapon(GameObject weapon)
	{
	}

	public GameObject CreateUIAvatar(CharacterClass _CharClass, Sex _SexType)
	{
		string text = string.Concat("Character/", _CharClass, "/0/", _CharClass, _SexType);
		string path = string.Concat("Character/Soldier/Animation/Soldier", _SexType, "_T/");
		if (_CharClass == CharacterClass.Engineer && _SexType == Sex.F)
		{
			path = "Character/Engineer/Animation/EngineerF_T/";
		}
		else if ((_CharClass == CharacterClass.Prayer || _CharClass == CharacterClass.Sniper) && _SexType == Sex.M)
		{
			path = "Character/Prayer/Animation/PrayerM_T/";
		}
		text += "_UI";
		Debug.Log("prefabName : " + text);
		Object original = Resources.Load(text);
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.position = new Vector3(4f, 0f, -5f);
		ChangeShaderForPlayer(gameObject);
		string text2 = "Weapon/SMG01";
		WeaponType weaponType = WeaponType.SubMachineGun;
		float num = 1f;
		if (_SexType == Sex.F)
		{
			switch (_CharClass)
			{
			case CharacterClass.Engineer:
				text2 = "Weapon/shotgun02";
				weaponType = WeaponType.ShotGun;
				num = 0.8f;
				break;
			case CharacterClass.Prayer:
				text2 = "Weapon/revolver02";
				weaponType = WeaponType.Revolver;
				num = 1f;
				break;
			case CharacterClass.Sniper:
				text2 = "Weapon/sniper01";
				weaponType = WeaponType.Sniper;
				num = 0.7f;
				break;
			default:
				text2 = "Weapon/SMG01";
				weaponType = WeaponType.SubMachineGun;
				num = 1f;
				break;
			}
		}
		else
		{
			switch (_CharClass)
			{
			case CharacterClass.Engineer:
				text2 = "Weapon/shotgun01";
				weaponType = WeaponType.ShotGun;
				num = 0.9f;
				break;
			case CharacterClass.Prayer:
				text2 = "Weapon/pistol01";
				weaponType = WeaponType.Pistol;
				num = 1f;
				break;
			case CharacterClass.Sniper:
				text2 = "Weapon/sniper01";
				weaponType = WeaponType.Sniper;
				num = 0.8f;
				break;
			default:
				text2 = "Weapon/assault01";
				weaponType = WeaponType.AssaultRifle;
				num = 1f;
				break;
			}
		}
		Object original2 = Resources.Load(text2);
		GameObject gameObject2 = (GameObject)Object.Instantiate(original2, gameObject.transform.position, gameObject.transform.rotation);
		ChangeShaderForWeapon(gameObject2);
		WeaponResourceConfig.RotateGun(gameObject2, 0);
		Transform transform = gameObject.transform.Find(BoneName.Weapon);
		if (transform == null)
		{
			Debug.Log("could not find weapon point!");
		}
		Debug.Log("player : " + gameObject);
		gameObject2.transform.parent = transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.Euler(Vector3.zero);
		gameObject2.transform.localScale = num * Vector3.one;
		GameObject gameObject3 = gameObject.transform.Find("Entity").gameObject;
		AddAnimationsForUI(gameObject3, path, weaponType);
		return gameObject;
	}

	public GameObject CreateUIAvatar(UserState.RoleState roleState)
	{
		return CreateUIAvatar(roleState.CharClass, roleState.SexType);
	}

	public GameObject RefreshAvatarWeapon(GameObject player, UserState.RoleState roleState, bool enableLight)
	{
		string path = string.Concat("Character/Soldier/Animation/Soldier", roleState.SexType, "_T/");
		if (roleState.CharClass == CharacterClass.Engineer && roleState.SexType == Sex.F)
		{
			path = "Character/Engineer/Animation/EngineerF_T/";
		}
		else if ((roleState.CharClass == CharacterClass.Prayer || roleState.CharClass == CharacterClass.Sniper) && roleState.SexType == Sex.M)
		{
			path = "Character/Prayer/Animation/PrayerM_T/";
		}
		string path2 = Weapon.CreatePrefabName(roleState.weaponType, roleState.weaponId);
		Object @object = Resources.Load(path2);
		if (@object == null)
		{
			return player;
		}
		GameObject gameObject = (GameObject)Object.Instantiate(@object, player.transform.position, player.transform.rotation);
		if (enableLight)
		{
			ChangeShaderForWeapon(gameObject);
		}
		Transform transform = player.transform.Find(BoneName.Weapon);
		if (transform == null)
		{
			Debug.Log("could not find weapon point!");
		}
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		switch (roleState.weaponType)
		{
		case WeaponType.Revolver:
			gameObject.transform.localScale = 1.1f * Vector3.one;
			break;
		case WeaponType.ShotGun:
			gameObject.transform.localScale = 0.9f * Vector3.one;
			break;
		case WeaponType.SubMachineGun:
			gameObject.transform.localScale = 1.1f * Vector3.one;
			break;
		case WeaponType.Sniper:
			gameObject.transform.localScale = 0.8f * Vector3.one;
			break;
		default:
			gameObject.transform.localScale = Vector3.one;
			break;
		}
		GameObject gameObject2 = player.transform.Find("Entity").gameObject;
		AddAnimationsForUI(gameObject2, path, roleState.weaponType);
		return player;
	}

	public GameObject CreateUIAvatarWithWeapon(UserState.RoleState roleState, bool enableLight, int avatarID)
	{
		string text = string.Concat("Character/", roleState.CharClass, "/", avatarID, "/", roleState.CharClass, roleState.SexType);
		text = ((!enableLight) ? (text + "_T") : (text + "_UI"));
		Object original = Resources.Load(text);
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.position = new Vector3(4f, 0f, -5f);
		if (enableLight)
		{
			ChangeShaderForPlayer(gameObject);
		}
		gameObject = RefreshAvatarWeapon(gameObject, roleState, enableLight);
		ChangeDecorations(gameObject, roleState);
		GameObject gameObject2 = gameObject.transform.Find("Entity").gameObject;
		gameObject2.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		gameObject2.GetComponent<Animation>().Play(roleState.GetUIIdleAnimation());
		Object.Destroy(gameObject.GetComponent<CharacterController>());
		Object.Destroy(gameObject2.GetComponent<CharacterController>());
		return gameObject;
	}

	public GameObject CreateUIAvatarWithWeapon(UserState.RoleState roleState, bool enableLight)
	{
		return CreateUIAvatarWithWeapon(roleState, enableLight, roleState.avatarID);
	}

	public GameObject CreateUIAvatarWithWeapon(UserState.RoleState roleState, string layer, bool enableLight)
	{
		return CreateUIAvatarWithWeapon(roleState, layer, enableLight, roleState.avatarID);
	}

	public GameObject CreateUIAvatarWithWeapon(UserState.RoleState roleState, string layer, bool enableLight, int avatarID)
	{
		GameObject gameObject = CreateUIAvatarWithWeapon(roleState, enableLight, avatarID);
		Transform[] componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
		Transform[] array = componentsInChildren;
		foreach (Transform transform in array)
		{
			transform.gameObject.layer = LayerMask.NameToLayer(layer);
		}
		return gameObject;
	}

	public GameObject CreateUIAvatarWithWeapon(UserState.RoleState roleState, GameObject father, string layer, bool enableLight)
	{
		return CreateUIAvatarWithWeapon(roleState, father, layer, enableLight, roleState.avatarID);
	}

	public GameObject CreateUIAvatarWithWeapon(UserState.RoleState roleState, GameObject father, string layer, bool enableLight, int avatarID)
	{
		GameObject gameObject = CreateUIAvatarWithWeapon(roleState, layer, enableLight, avatarID);
		gameObject.transform.parent = father.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
		gameObject.transform.localScale = Vector3.one;
		return gameObject;
	}

	public GameObject ReBuildPlayerAvatar(UserState userState, Player p)
	{
		GameObject gameObject = ReBuildAvatar(userState, p);
		GameObject original = Resources.Load("Effect/Shadow") as GameObject;
		GameObject gameObject2 = Object.Instantiate(original) as GameObject;
		gameObject2.transform.position = gameObject.transform.position;
		gameObject2.transform.localScale = Vector3.one * 0.15f;
		gameObject2.transform.parent = gameObject.transform;
		return gameObject;
	}

	public GameObject RebuildAvatar(UserState userState, Player p)
	{
		GameObject gameObject = null;
		if (p.IsLocal())
		{
			return CreateFirstPersonAvatar(userState, p);
		}
		return CreateThirdPersonAvatar(userState, p);
	}

	public void ChangeDecorations(GameObject player, UserState.RoleState roleState)
	{
		byte[] decoration = roleState.decoration;
		if (!(player == null))
		{
			ChangeHead(player, decoration[0] - 1, roleState.CharClass);
			ChangeFace(player, decoration[1] - 1, roleState.CharClass);
			ChangeWaist(player, decoration[2] - 1, roleState.CharClass);
		}
	}

	private void ChangeOne(GameObject player, int id, string boneName, string prefabPath)
	{
		Transform transform = player.transform.Find(boneName);
		if (transform == null)
		{
			Debug.Log("could not find decoration point" + boneName + "!");
			return;
		}
		string text = prefabPath + id;
		Debug.Log("prefabName : " + text);
		GameObject original = Resources.Load(text) as GameObject;
		GameObject gameObject = Object.Instantiate(original) as GameObject;
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localEulerAngles = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.layer = transform.gameObject.layer;
		gggo = gameObject;
	}

	private void DestroyCurrentOne(GameObject player, string boneName)
	{
		Transform transform = player.transform.Find(boneName);
		if (transform == null)
		{
			Debug.Log("could not find decoration point " + boneName + "!");
			return;
		}
		Transform[] componentsInChildren = transform.GetComponentsInChildren<Transform>();
		for (int i = 1; i < componentsInChildren.Length; i++)
		{
			Object.Destroy(componentsInChildren[i].gameObject);
		}
	}

	public void ChangeHead(GameObject player, int id, CharacterClass charclass)
	{
		DestroyCurrentOne(player, BoneName.Head);
		if (id >= 0)
		{
			ChangeOne(player, id, BoneName.Head, string.Concat("Decoration/Head/", charclass, "/"));
		}
	}

	public void ChangeFace(GameObject player, int id, CharacterClass charclass)
	{
		DestroyCurrentOne(player, BoneName.Face);
		if (id >= 0)
		{
			ChangeOne(player, id, BoneName.Face, string.Concat("Decoration/Face/", charclass, "/"));
		}
	}

	public void ChangeWaist(GameObject player, int id, CharacterClass charclass)
	{
		DestroyCurrentOne(player, BoneName.Waist);
		if (id >= 0)
		{
			ChangeOne(player, id, BoneName.Waist, string.Concat("Decoration/Waist/", charclass, "/"));
		}
	}
}
