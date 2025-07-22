using UnityEngine;

public class StateAim : MonoBehaviour
{
	public GameObject circle;

	public GameObject square;

	private Vector3 initScale;

	private void Awake()
	{
		int num = (int)((float)Screen.height * (float)UIRatio.BASE_SCREEN_WIDTH / (float)UIRatio.BASE_SCREEN_HEIGHT);
		float num2 = (float)Screen.width * 1.01f / (float)num;
		base.transform.localScale = Vector3.one * ((!(num2 > 1f)) ? (1f / num2) : num2);
	}

	private void OnEnable()
	{
		if (GameApp.GetInstance().GetGameWorld() != null && GameApp.GetInstance().GetGameWorld().GetLocalPlayer() != null)
		{
			if (GameApp.GetInstance().GetGameWorld().GetLocalPlayer()
				.GetWeapon()
				.Name.Equals("sniper03"))
			{
				circle.SetActive(false);
				square.SetActive(true);
			}
			else
			{
				circle.SetActive(true);
				square.SetActive(false);
			}
		}
	}
}
