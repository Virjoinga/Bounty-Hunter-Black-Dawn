public class BulletStateScript : IgnoreTimeScale
{
	public UILabel Label;

	public WeaponType BulletType;

	private float lastUpdateTime;

	private float updateTimeInterval = 0.2f;

	private bool updateEnable = true;

	private new void OnEnable()
	{
		updateEnable = true;
		Refresh();
	}

	private void Update()
	{
		lastUpdateTime += UpdateRealTimeDelta();
		if (updateEnable && lastUpdateTime > updateTimeInterval)
		{
			lastUpdateTime = 0f;
			Refresh();
		}
	}

	public void SetUpdateEnable(bool enable)
	{
		updateEnable = enable;
	}

	private void Refresh()
	{
		if (Label != null)
		{
			LocalPlayer localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
			short num = (short)(GameApp.GetInstance().GetUserState().GetBulletByWeaponType(BulletType) + localPlayer.GetBulletInGuns(BulletType));
			short maxBulletByWeaponType = GameApp.GetInstance().GetUserState().GetMaxBulletByWeaponType(BulletType);
			ItemInfiniteBullet itemInfiniteBullet = (ItemInfiniteBullet)GameApp.GetInstance().GetGlobalState().GetIAPitemState()
				.GetGlobalIAPItem(IAPItemState.ItemType.InfiniteBullet);
			bool flag = itemInfiniteBullet.IsUnlimitedBullet(BulletType);
			Label.text = ((!flag) ? (num + "\n" + maxBulletByWeaponType) : "/max\n/max");
		}
	}
}
