using UnityEngine;

public class SkillIcon : MonoBehaviour
{
	public int id;

	public GameObject m_Button;

	public UIFilledSprite m_Mask;

	public UIFilledSprite m_Icon;

	public UITweenX m_Tween;

	private UserStateHUD.SkillHUD m_SkillHUD;

	private bool bSkillEnable;

	private bool bCDStart;

	private void Awake()
	{
		m_SkillHUD = ((id != 0) ? UserStateHUD.GetInstance().Skill2 : UserStateHUD.GetInstance().Skill1);
	}

	private void Start()
	{
		NGUITools.SetActive(m_Button, false);
	}

	private void OnEnable()
	{
		NGUITools.SetActive(m_Button, false);
	}

	private void Update()
	{
		if (m_SkillHUD.Enable)
		{
			if (!m_Button.activeSelf)
			{
				NGUITools.SetActive(m_Button, true);
				UIFilledSprite mask = m_Mask;
				UIAtlas characterSkillIconAtlas = SkillTreeMgr.CharacterSkillIconAtlas;
				m_Icon.atlas = characterSkillIconAtlas;
				mask.atlas = characterSkillIconAtlas;
				UIFilledSprite mask2 = m_Mask;
				string iconFileName = m_SkillHUD.IconFileName;
				m_Icon.spriteName = iconFileName;
				mask2.spriteName = iconFileName;
				m_Mask.MakePixelPerfect();
				m_Icon.MakePixelPerfect();
				m_Icon.fillAmount = 1f;
			}
			if (bCDStart)
			{
				m_Icon.fillAmount = 1f - m_SkillHUD.CDPercent;
				if (m_SkillHUD.CDPercent <= 0f)
				{
					m_Tween.PlayForward();
					bCDStart = false;
				}
			}
			else if (m_SkillHUD.CDPercent > 0f)
			{
				bCDStart = true;
				m_Icon.fillAmount = 0f;
			}
			else if (m_Icon.fillAmount < 1f)
			{
				m_Icon.fillAmount = 1f;
			}
		}
		else if (m_Button.activeSelf)
		{
			NGUITools.SetActive(m_Button, false);
		}
	}
}
