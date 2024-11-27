using UnityEngine;

public class ItemPopMenuWindowCollision : MonoBehaviour
{
	private void Awake()
	{
		BoxCollider boxCollider = base.gameObject.GetComponent<Collider>() as BoxCollider;
		boxCollider.size = new Vector3(Screen.width, Screen.height, 0f);
	}

	private void OnClick()
	{
		ItemPopMenu.instance.Close();
	}
}
