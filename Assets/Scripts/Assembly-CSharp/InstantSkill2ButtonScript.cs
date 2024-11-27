using System.Collections.Generic;
using UnityEngine;

public class InstantSkill2ButtonScript : MonoBehaviour
{
	public UILabel Label;

	protected CharacterInstantSkill skill;

	protected Player player;

	public static InstantSkill2ButtonScript mInstance;

	protected bool UpdateEnabled;

	private void OnClick()
	{
		if (skill != null && player != null)
		{
			skill.ApplySkill(player);
		}
	}

	private void Update()
	{
		if (!UpdateEnabled)
		{
			return;
		}
		if (player == null)
		{
			player = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		}
		if (skill == null && player != null)
		{
			CharacterSkillManager characterSkillManager = player.GetCharacterSkillManager();
			if (characterSkillManager != null)
			{
				List<CharacterInstantSkill> initiativeSkillList = characterSkillManager.GetInitiativeSkillList();
				int num = 0;
				for (int i = 0; i < initiativeSkillList.Count; i++)
				{
					if (initiativeSkillList[i].STriggerType == SkillTriggerType.Initiative)
					{
						if (num != 0)
						{
							skill = initiativeSkillList[i];
							break;
						}
						num++;
					}
				}
			}
		}
		if (Label != null && skill != null && player != null)
		{
			Label.text = skill.Name + "(" + skill.GetCoolDownLeftTime() + ")";
		}
	}

	private void Start()
	{
		mInstance = this;
		UpdateEnabled = true;
	}

	private void OnDestroy()
	{
		mInstance = null;
	}

	public void EnableUpdate()
	{
		UpdateEnabled = true;
	}
}