using UnityEngine;

public class IAPItem
{
	public string ID { get; set; }

	public string Desc { get; set; }

	public int Mithril { get; set; }
}
public class IapItem : MonoBehaviour
{
	public IAPName iapItemName = IAPName.None;
}
