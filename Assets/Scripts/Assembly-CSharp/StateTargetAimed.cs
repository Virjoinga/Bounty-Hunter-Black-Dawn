using UnityEngine;

public class StateTargetAimed : MonoBehaviour
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
	private UISprite m_SpriteCaptain;

	[SerializeField]
	private GameObject m_Container;

	private void OnEnable()
	{
		m_Container.SetActive(false);
	}

	private void Update()
	{
		UserStateHUD.GameUnitHUD targetAimed = UserStateHUD.GetInstance().TargetAimed;
		if (targetAimed.type != 0)
		{
			if (!m_Container.activeSelf)
			{
				m_Container.SetActive(true);
				m_SpriteCaptain.gameObject.SetActive(false);
				m_SpriteDanger.gameObject.SetActive(false);
				m_SpriteMaxSp.gameObject.SetActive(false);
			}
			m_LabelEnemyLevel.text = targetAimed.LevelStr;
			m_LabelEnemyName.text = targetAimed.Name;
			if (targetAimed.type == UserStateHUD.GameUnitHUD.Type.Enemy)
			{
				m_LabelEnemyLevel.color = targetAimed.Color;
				m_LabelEnemyName.color = targetAimed.Color;
				if (targetAimed.IsDangerEnemy)
				{
					if (!m_SpriteDanger.gameObject.activeSelf)
					{
						m_SpriteDanger.gameObject.SetActive(true);
					}
				}
				else if (m_SpriteDanger.gameObject.activeSelf)
				{
					m_SpriteDanger.gameObject.SetActive(false);
				}
			}
			else if (targetAimed.type == UserStateHUD.GameUnitHUD.Type.RemotePlayer)
			{
				m_LabelEnemyLevel.color = targetAimed.Color;
				m_LabelEnemyName.color = targetAimed.Color;
				if (targetAimed.IsRoomMaster)
				{
					if (!m_SpriteCaptain.gameObject.activeSelf)
					{
						m_SpriteCaptain.gameObject.SetActive(true);
					}
				}
				else if (m_SpriteCaptain.gameObject.activeSelf)
				{
					m_SpriteCaptain.gameObject.SetActive(false);
				}
			}
			m_SpriteHp.fillAmount = targetAimed.HpPercent;
			if (targetAimed.HasShelid)
			{
				if (!m_SpriteSp.gameObject.activeSelf)
				{
					m_SpriteSp.gameObject.SetActive(true);
				}
				m_SpriteSp.fillAmount = targetAimed.SpPercent;
			}
			else if (m_SpriteSp.gameObject.activeSelf)
			{
				m_SpriteSp.gameObject.SetActive(false);
			}
		}
		else if (m_Container.activeSelf)
		{
			m_Container.SetActive(false);
		}
	}
}
