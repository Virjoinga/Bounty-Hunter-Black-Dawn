using UnityEngine;

public class ProgramsRunAtStartScript : MonoBehaviour
{
	public bool tokenSent;

	private void Start()
	{
		if (!GameApp.GetInstance().LogoFirstPop)
		{
		}
	}

	private void Update()
	{
	}
}
