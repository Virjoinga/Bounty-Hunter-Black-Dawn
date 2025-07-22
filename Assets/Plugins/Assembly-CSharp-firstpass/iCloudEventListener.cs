using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iCloudEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		iCloudManager.keyValueStoreDidChangeEvent += keyValueStoreDidChangeEvent;
		iCloudManager.entitlementsMissingEvent += entitlementsMissingEvent;
		iCloudManager.documentStoreUpdatedEvent += documentStoreUpdatedEvent;
	}

	private void OnDisable()
	{
		iCloudManager.keyValueStoreDidChangeEvent -= keyValueStoreDidChangeEvent;
		iCloudManager.entitlementsMissingEvent -= entitlementsMissingEvent;
		iCloudManager.documentStoreUpdatedEvent -= documentStoreUpdatedEvent;
	}

	private void keyValueStoreDidChangeEvent(ArrayList keys)
	{
		Debug.Log("keyValueStoreDidChangeEvent.  changed keys: ");
		foreach (string key in keys)
		{
			Debug.Log(key);
		}
	}

	private void entitlementsMissingEvent()
	{
		Debug.Log("entitlementsMissingEvent");
	}

	private void documentStoreUpdatedEvent(List<iCloudManager.iCloudDocument> files)
	{
		foreach (iCloudManager.iCloudDocument file in files)
		{
			Debug.Log(file);
		}
	}
}
