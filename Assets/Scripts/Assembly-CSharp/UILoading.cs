using UnityEngine;

internal class UILoading : MonoBehaviour
{
	public const string m_animName = "RPG_anim_150";

	public static UILoading m_instance;

	public FadeAnimationScript m_fade;

	public GameObject m_loadAnim;

	public UILabel m_txt;

	public GameObject m_background;

	private float m_animPercent;

	private void Awake()
	{
		m_instance = this;
	}

	private void Start()
	{
		SetTxt(LocalizationManager.GetInstance().GetString("LOC_LOADING"));
		FadeOutBlack();
		Debug.Log("fade out......");
	}

	private void Update()
	{
		if (FadeOutComplete())
		{
			UpdateAnim("RPG_anim_150");
		}
	}

	public bool IsCompleted()
	{
		if (AnimationPlayed("RPG_anim_150", 1f))
		{
			return true;
		}
		return false;
	}

	public void UpdateAnim(string name)
	{
		if (AnimationPlayed(name, m_animPercent))
		{
			PauseAnim(name);
		}
		else
		{
			ResumeAnim(name);
		}
	}

	public void SetTxt(string txt)
	{
		m_txt.text = txt;
	}

	public void SetAnimPercent(float percent)
	{
		m_animPercent = percent;
	}

	private void PauseAnim(string name)
	{
		m_loadAnim.GetComponent<Animation>()[name].speed = 0f;
	}

	private void ResumeAnim(string name)
	{
		m_loadAnim.GetComponent<Animation>()[name].speed = 1f;
	}

	public void PlayAnimation(string name, WrapMode mode, float percent)
	{
		if (!m_loadAnim.GetComponent<Animation>().IsPlaying(name) || mode != WrapMode.ClampForever)
		{
			m_loadAnim.GetComponent<Animation>()[name].wrapMode = mode;
			m_loadAnim.GetComponent<Animation>().CrossFade(name, 0.2f);
			m_animPercent = percent;
		}
	}

	public bool AnimationPlayed(string name, float percent)
	{
		if (m_instance == null || m_loadAnim.GetComponent<Animation>()[name] == null)
		{
			return false;
		}
		if (m_loadAnim.GetComponent<Animation>()[name].speed >= 0f)
		{
			if (m_loadAnim.GetComponent<Animation>()[name].time >= m_loadAnim.GetComponent<Animation>()[name].clip.length * percent)
			{
				return true;
			}
			return false;
		}
		if (m_loadAnim.GetComponent<Animation>()[name].time <= m_loadAnim.GetComponent<Animation>()[name].clip.length * (1f - percent))
		{
			return true;
		}
		return false;
	}

	private void OnDestroy()
	{
		m_instance = null;
	}

	public bool FadeOutComplete()
	{
		return m_fade.FadeOutComplete();
	}

	public void FadeOutBlack()
	{
		m_fade.FadeOutBlack();
	}
}
