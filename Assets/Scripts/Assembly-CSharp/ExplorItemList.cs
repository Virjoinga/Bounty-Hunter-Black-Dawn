using UnityEngine;

public class ExplorItemList : MonoBehaviour
{
	public GameObject EffectObject;

	public int[] ExplorItemIDs;

	public float[] AppearRates;

	public bool m_DestroyAfterFinished;

	public int m_id;

	public int m_questId;

	public int m_posId;

	public byte ID_in_Block { get; set; }

	private void Awake()
	{
		ID_in_Block = (byte)m_id;
		Debug.Log("Awake! " + ID_in_Block);
	}
}
