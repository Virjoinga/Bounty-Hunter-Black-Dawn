using UnityEngine;

public class UIProgressBar : MonoBehaviour
{
	public float time = 15f;

	public UIFilledSprite upper;

	public GameObject call;

	public string fuctionName;

	private float startTime;

	private bool bOverTime;

	private void OnEnable()
	{
		startTime = Time.time;
		bOverTime = false;
	}

	private void Update()
	{
		if (bOverTime)
		{
			return;
		}
		float num = (time - (Time.time - startTime)) / time;
		upper.fillAmount = ((!(num < 0f)) ? num : 0f);
		if (upper.fillAmount == 0f)
		{
			if (call != null)
			{
				call.SendMessage(fuctionName, SendMessageOptions.DontRequireReceiver);
			}
			bOverTime = true;
		}
	}
}
