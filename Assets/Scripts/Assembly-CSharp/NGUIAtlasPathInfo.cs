using UnityEngine;

public class NGUIAtlasPathInfo
{
	private string mFileName;

	private string mPath;

	public string FullPath
	{
		set
		{
			int num = value.LastIndexOf("/");
			mPath = value.Substring(0, num + 1).Replace("Assets/Resources/", string.Empty);
			mFileName = value.Substring(num + 1).Replace(".prefab", string.Empty);
			Debug.Log("mPath : " + mPath);
			Debug.Log("mFileName : " + mFileName);
		}
	}

	public string FileName
	{
		get
		{
			return mFileName;
		}
	}

	public string Path
	{
		get
		{
			return mPath;
		}
	}
}
