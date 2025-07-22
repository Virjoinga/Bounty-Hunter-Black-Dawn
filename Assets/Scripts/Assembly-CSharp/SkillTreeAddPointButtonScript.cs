using UnityEngine;

public class SkillTreeAddPointButtonScript : MonoBehaviour
{
	public int TreeID { get; set; }

	public int LayerID { get; set; }

	public int ColID { get; set; }

	public void SetInfo(int treeID, int layerID, int colID)
	{
		TreeID = treeID;
		LayerID = layerID;
		ColID = colID;
	}
}
