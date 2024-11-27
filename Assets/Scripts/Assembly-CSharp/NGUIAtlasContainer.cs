using System.Collections.Generic;
using UnityEngine;

public class NGUIAtlasContainer : MonoBehaviour
{
	[SerializeField]
	private List<string> uiAtlasList = new List<string>();

	[SerializeField]
	private List<string> uiFontList = new List<string>();

	public List<NGUIAtlasPathInfo> UIAtlasInfoList
	{
		get
		{
			return CreateInfoList(uiAtlasList);
		}
	}

	public List<NGUIAtlasPathInfo> UIFontInfoList
	{
		get
		{
			return CreateInfoList(uiFontList);
		}
	}

	private List<NGUIAtlasPathInfo> CreateInfoList(List<string> list)
	{
		List<NGUIAtlasPathInfo> list2 = new List<NGUIAtlasPathInfo>();
		foreach (string item in list)
		{
			NGUIAtlasPathInfo nGUIAtlasPathInfo = new NGUIAtlasPathInfo();
			nGUIAtlasPathInfo.FullPath = item;
			list2.Add(nGUIAtlasPathInfo);
		}
		return list2;
	}

	public void SetUIAtlasPathList(List<string> pathList)
	{
		SetPathList(uiAtlasList, pathList);
	}

	public void SetUIFontPathList(List<string> pathList)
	{
		SetPathList(uiFontList, pathList);
	}

	private void SetPathList(List<string> uiList, List<string> targetPathList)
	{
		uiList.Clear();
		foreach (string targetPath in targetPathList)
		{
			uiList.Add(targetPath);
		}
	}
}
