using System.Collections.Generic;
using UnityEngine;

public class SummonButtonScript : MonoBehaviour
{
	private void OnClick()
	{
		Player localPlayer = GameApp.GetInstance().GetGameWorld().GetLocalPlayer();
		if (localPlayer == null)
		{
			return;
		}
		CharacterSkillManager characterSkillManager = localPlayer.GetCharacterSkillManager();
		List<CharacterInstantSkill> initiativeSkillList = characterSkillManager.GetInitiativeSkillList();
		for (int i = 0; i < initiativeSkillList.Count; i++)
		{
			if (initiativeSkillList[i].SEffectType == SkillEffectType.Summon)
			{
				initiativeSkillList[i].ApplySkill(localPlayer);
				break;
			}
		}
	}
}
