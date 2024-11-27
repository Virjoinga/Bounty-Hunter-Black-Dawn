using UnityEngine;

public class SetLanguageOnSelection : MonoBehaviour
{
	private void OnSelectionChange(string val)
	{
		LocalizationManager.GetInstance().currentLanguage = val;
	}
}
