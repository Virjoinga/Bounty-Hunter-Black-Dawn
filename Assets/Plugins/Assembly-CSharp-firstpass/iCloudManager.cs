using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iCloudManager : MonoBehaviour
{
	public class iCloudDocument
	{
		public string filename;

		public bool isDownloaded;

		public DateTime contentChangedDate;

		public iCloudDocument(Hashtable ht)
		{
			if (ht.Contains("filename"))
			{
				filename = ht["filename"].ToString();
			}
			if (ht.Contains("isDownloaded"))
			{
				isDownloaded = bool.Parse(ht["isDownloaded"].ToString());
			}
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			if (ht.Contains("contentChangedDate"))
			{
				double value = double.Parse(ht["contentChangedDate"].ToString());
				contentChangedDate = dateTime.AddSeconds(value);
			}
		}

		public static List<iCloudDocument> fromJSON(string json)
		{
			ArrayList arrayList = json.arrayListFromJson();
			List<iCloudDocument> list = new List<iCloudDocument>(arrayList.Count);
			foreach (Hashtable item in arrayList)
			{
				list.Add(new iCloudDocument(item));
			}
			return list;
		}

		public override string ToString()
		{
			return string.Format("<iCloudDocument> filename: {0}, isDownloaded: {1}, contentChangedDate: {2}", filename, isDownloaded, contentChangedDate);
		}
	}

	public static event Action<ArrayList> keyValueStoreDidChangeEvent;

	public static event Action entitlementsMissingEvent;

	public static event Action<List<iCloudDocument>> documentStoreUpdatedEvent;

	private void Awake()
	{
		base.gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	private void keyValueStoreDidChange(string param)
	{
		if (iCloudManager.keyValueStoreDidChangeEvent != null)
		{
			ArrayList obj = param.arrayListFromJson();
			iCloudManager.keyValueStoreDidChangeEvent(obj);
		}
	}

	private void entitlementsMissing(string empty)
	{
		if (iCloudManager.entitlementsMissingEvent != null)
		{
			iCloudManager.entitlementsMissingEvent();
		}
	}

	private void documentStoreUpdated(string json)
	{
		if (iCloudManager.documentStoreUpdatedEvent != null)
		{
			List<iCloudDocument> obj = iCloudDocument.fromJSON(json);
			iCloudManager.documentStoreUpdatedEvent(obj);
		}
	}
}
