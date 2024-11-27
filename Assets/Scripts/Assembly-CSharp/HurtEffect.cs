using System.Collections.Generic;
using UnityEngine;

public class HurtEffect : MonoBehaviour
{
	public class InjuredUnit
	{
		public StateIcon icon;

		private UISprite region;

		private float alpha = 1f;

		public InjuredUnit(StateIcon icon)
		{
			this.icon = icon;
			region = icon.GetComponentInChildren<UISprite>();
		}

		public void FadeOut()
		{
			alpha -= 0.0125f;
			region.color = new Color(region.color.r, region.color.g, region.color.b, alpha);
		}

		public bool IsHide()
		{
			return alpha <= 0f;
		}

		public void Show()
		{
			region.color = new Color(region.color.r, region.color.g, region.color.b, 1f);
		}
	}

	public StateIcon clone;

	public GameObject deadIcon;

	private List<InjuredUnit> iconlist;

	private InjuredUnit oneIcon;

	private float multiple;

	private void OnEnable()
	{
		iconlist = new List<InjuredUnit>();
		NGUITools.SetActive(clone.gameObject, false);
		NGUITools.SetActive(deadIcon, false);
	}

	private void Update()
	{
		if (base.gameObject.activeSelf)
		{
			UpdateOne();
		}
	}

	private void UpdateOne()
	{
		if (oneIcon != null)
		{
			float angleBetweenUserHorizontal = MathUtil.GetAngleBetweenUserHorizontal(oneIcon.icon.unit.Position);
			oneIcon.icon.UpdatePosition(angleBetweenUserHorizontal);
			oneIcon.FadeOut();
			if (oneIcon.IsHide())
			{
				NGUITools.SetActive(oneIcon.icon.gameObject, false);
				oneIcon = null;
			}
		}
		else
		{
			List<UserStateHUD.GameUnitHUD> list = UserStateHUD.GetInstance().PopUnitWhoAttacksUser();
			if (UserStateHUD.GetInstance().GetUserHp() > 0 && list.Count > 0)
			{
				clone.unit = list[list.Count - 1];
				NGUITools.SetActive(clone.gameObject, true);
				oneIcon = new InjuredUnit(clone);
				oneIcon.Show();
			}
		}
	}

	private void UpdateCurRegion()
	{
		int num = 0;
		while (num < iconlist.Count)
		{
			float angleBetweenUserHorizontal = MathUtil.GetAngleBetweenUserHorizontal(iconlist[num].icon.unit.Position);
			iconlist[num].icon.UpdatePosition(angleBetweenUserHorizontal);
			iconlist[num].FadeOut();
			if (iconlist[num].IsHide())
			{
				Object.Destroy(iconlist[num].icon.gameObject);
				iconlist.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	private void UpdateNewRegion()
	{
		List<UserStateHUD.GameUnitHUD> list = UserStateHUD.GetInstance().PopUnitWhoAttacksUser();
		if (UserStateHUD.GetInstance().GetUserHp() > 0)
		{
			foreach (UserStateHUD.GameUnitHUD item2 in list)
			{
				StateIcon stateIcon = Object.Instantiate(clone) as StateIcon;
				stateIcon.transform.parent = base.transform;
				stateIcon.transform.localScale = clone.transform.localScale;
				stateIcon.transform.rotation = clone.transform.rotation;
				stateIcon.transform.position = clone.transform.position;
				stateIcon.unit = item2;
				InjuredUnit item = new InjuredUnit(stateIcon);
				iconlist.Add(item);
			}
		}
		list = null;
	}
}
