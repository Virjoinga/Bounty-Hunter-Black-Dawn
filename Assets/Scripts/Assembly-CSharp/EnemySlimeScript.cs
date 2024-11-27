using UnityEngine;

public class EnemySlimeScript : MonoBehaviour
{
	public int slimeDamage;

	public float minScale;

	public float maxScale = 1f;

	public float disappearTime = 5f;

	public string colorPropertyName = "_TintColor";

	public float damageInterval = 1f;

	public float diffuseSpeed = 5f;

	private Timer hitPlayerTimer = new Timer();

	private Timer hitSummonedTimer = new Timer();

	private float startTime;

	private bool isScaling = true;

	public Enemy enemy;

	public void Start()
	{
		base.transform.localScale = new Vector3(minScale, 1f, minScale);
		startTime = Time.time;
		hitPlayerTimer.SetTimer(damageInterval, true);
		hitSummonedTimer.SetTimer(damageInterval, true);
	}

	public void Update()
	{
		if (isScaling)
		{
			if (base.transform.localScale.x < maxScale)
			{
				base.transform.localScale += new Vector3(Time.deltaTime * diffuseSpeed, 0f, Time.deltaTime * diffuseSpeed);
				return;
			}
			base.transform.localScale = new Vector3(maxScale, 1f, maxScale);
			startTime = Time.time;
			isScaling = false;
		}
		else if (Time.time - startTime > disappearTime)
		{
			Object.DestroyObject(base.gameObject);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.layer == PhysicsLayer.PLAYER)
		{
			if (hitPlayerTimer.Ready())
			{
				hitPlayerTimer.Do();
				GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.OnHit(slimeDamage, enemy);
			}
		}
		else
		{
			if (other.gameObject.layer != PhysicsLayer.SUMMONED || !hitSummonedTimer.Ready())
			{
				return;
			}
			GameObject controllableByCollider = ControllableUnit.GetControllableByCollider(other);
			if (controllableByCollider != null)
			{
				SummonedItem summonedByName = GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
					.GetSummonedByName(controllableByCollider.name);
				if (summonedByName != null)
				{
					hitSummonedTimer.Do();
					summonedByName.OnHit(slimeDamage);
				}
			}
		}
	}
}
