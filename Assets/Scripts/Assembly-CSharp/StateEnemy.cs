using UnityEngine;

public class StateEnemy : MonoBehaviour
{
	[SerializeField]
	private UILabel m_LabelEnemyLevel;

	[SerializeField]
	private UILabel m_LabelEnemyName;

	[SerializeField]
	private UIFilledSprite m_SpriteHp;

	[SerializeField]
	private UIFilledSprite m_SpriteMaxHp;

	[SerializeField]
	private UIFilledSprite m_SpriteSp;

	[SerializeField]
	private UIFilledSprite m_SpriteMaxSp;

	[SerializeField]
	private UISprite m_SpriteDanger;

	[SerializeField]
	private GameObject m_Container;

	private void OnEnable()
	{
		m_Container.SetActive(false);
	}

	private void Update()
	{
		UserStateHUD.GameUnitHUD targetAimed = UserStateHUD.GetInstance().TargetAimed;
		if (targetAimed.type == UserStateHUD.GameUnitHUD.Type.Enemy)
		{
			if (!m_Container.activeSelf)
			{
				NGUITools.SetActive(m_Container, true);
			}
			int num = targetAimed.Level - GameApp.GetInstance().GetUserState().GetCharLevel();
			m_LabelEnemyLevel.text = targetAimed.LevelStr;
			m_LabelEnemyLevel.color = EnemyLevelColor.GetColorByDeltaLevel(num);
			m_LabelEnemyName.text = targetAimed.Name;
			m_LabelEnemyName.color = EnemyLevelColor.GetColorByDeltaLevel(num);
			if (num > 2)
			{
				m_SpriteDanger.gameObject.SetActive(true);
			}
			else
			{
				m_SpriteDanger.gameObject.SetActive(false);
			}
			m_SpriteHp.fillAmount = targetAimed.HpPercent;
			if (targetAimed.HasShelid)
			{
				if (!m_SpriteSp.gameObject.activeSelf)
				{
					m_SpriteSp.gameObject.SetActive(true);
				}
				if (!m_SpriteMaxSp.gameObject.activeSelf)
				{
					m_SpriteMaxSp.gameObject.SetActive(true);
				}
				m_SpriteSp.fillAmount = targetAimed.SpPercent;
			}
			else
			{
				if (m_SpriteSp.gameObject.activeSelf)
				{
					NGUITools.SetActive(m_SpriteSp.gameObject, false);
				}
				if (m_SpriteMaxSp.gameObject.activeSelf)
				{
					NGUITools.SetActive(m_SpriteMaxSp.gameObject, false);
				}
			}
		}
		else if (m_Container.activeSelf)
		{
			NGUITools.SetActive(m_Container, false);
		}
	}
}
