using UnityEngine;

public class PlayerReviveScript : MonoBehaviour
{
	public UILabel PriceLabel;

	public UISprite buttonImageSprite;

	public string blockSpriteName;

	protected int mPrice;

	private GlobalState globalState;

	private void OnEnable()
	{
		globalState = GameApp.GetInstance().GetGlobalState();
		mPrice = UserStateHUD.GetInstance().GetVSBattleFieldState().RevivePrice;
		if (!UserStateHUD.GetInstance().IsUserDead())
		{
			base.gameObject.SetActive(false);
		}
	}

	private void OnClick()
	{
		if (mPrice >= 0 && globalState != null && globalState.BuyWithMithril(mPrice))
		{
			AchievementTrigger achievementTrigger = AchievementTrigger.Create(AchievementID.On_My_Own);
			achievementTrigger.PutData(1);
			AchievementManager.GetInstance().Trigger(achievementTrigger);
			GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.DropAtSpawnPositionVS();
			UserStateHUD.GetInstance().GetVSBattleFieldState().RecordRevive();
			UIVSBattleShop.Close();
			PlayerRebirthRequest request = new PlayerRebirthRequest(-1);
			GameApp.GetInstance().GetNetworkManager().SendRequest(request);
		}
	}

	private void Update()
	{
		if (globalState == null)
		{
			return;
		}
		if (PriceLabel != null)
		{
			PriceLabel.text = string.Empty + mPrice;
		}
		if (globalState.GetMithril() < mPrice)
		{
			base.gameObject.GetComponent<Collider>().enabled = false;
			if (buttonImageSprite != null && !buttonImageSprite.spriteName.Equals(blockSpriteName))
			{
				buttonImageSprite.spriteName = blockSpriteName;
				buttonImageSprite.MakePixelPerfect();
			}
		}
	}
}
