using UnityEngine;

public class FloatLaserLockScript : MonoBehaviour
{
	public int mDamage;

	public Enemy mEnemy;

	public GameObject mSpark;

	public Vector3 mTargetPos;

	private Ray mRay;

	private RaycastHit mRaycastHit;

	private float mLength;

	private LocalPlayer mLocalPlayer;

	private Timer mPlayerHitTimer = new Timer();

	private Timer mSummonedHitTimer = new Timer();

	public void Start()
	{
		mRaycastHit = default(RaycastHit);
		mLocalPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		mPlayerHitTimer.SetTimer(2f, true);
		mSummonedHitTimer.SetTimer(0.3f, true);
	}

	public void Update()
	{
		if (mEnemy != null)
		{
			Vector3 normalized = (mTargetPos - base.transform.position).normalized;
			mRay = new Ray(base.transform.position, normalized);
			int layerMask = (1 << PhysicsLayer.WALL) | (1 << PhysicsLayer.FLOOR);
			if (Physics.Raycast(mRay, out mRaycastHit, 500f, layerMask))
			{
				float magnitude = (mRaycastHit.point - base.transform.position).magnitude;
				base.transform.localScale = new Vector3(base.transform.localScale.x, base.transform.localScale.y, (magnitude + 0.1f) / 10f);
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (mEnemy != null && other.gameObject.layer == PhysicsLayer.PLAYER && mLocalPlayer != null && mLocalPlayer.GetTransform() != null && mPlayerHitTimer.Ready())
		{
			mPlayerHitTimer.Do();
			SlowDown(mLocalPlayer);
		}
	}

	private void SlowDown(LocalPlayer player)
	{
		CharacterStateSkill characterStateSkill = new CharacterStateSkill();
		characterStateSkill.skillID = 0;
		characterStateSkill.IsPermanent = false;
		characterStateSkill.Duration = 2f;
		characterStateSkill.ModifierOfBuff1 = BuffModifier.PERCENTAGE;
		characterStateSkill.PropertyChangeTypeOfBuff1 = PropertyChangeType.ChangeMoveSpeed;
		characterStateSkill.BuffValueY1 = -100f;
		characterStateSkill.FunctionType1 = BuffFunctionType.PropertyChange;
		characterStateSkill.IconName = "gong3_04";
		CharacterSkillManager characterSkillManager = player.GetCharacterSkillManager();
		characterSkillManager.AddSkill(characterStateSkill);
		characterStateSkill.StartBuff();
	}
}
