using UnityEngine;

public class ChatQuickPhraseContainer : MonoBehaviour
{
	public UIGrid m_PopMenuGrid;

	public GameObject m_PopMenuBackground;

	public ChatQuickPhrase m_ChatQuickPhraseSample;

	private int phraseCount;

	private void Awake()
	{
		m_ChatQuickPhraseSample.gameObject.SetActive(false);
		phraseCount = 0;
	}

	public void AddPhrase(string text)
	{
		m_ChatQuickPhraseSample.SetPhrase(text);
		GameObject gameObject = Object.Instantiate(m_ChatQuickPhraseSample.gameObject) as GameObject;
		gameObject.transform.parent = m_PopMenuGrid.gameObject.transform;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.SetActive(true);
		m_PopMenuGrid.repositionNow = true;
		phraseCount++;
		m_PopMenuBackground.transform.localScale = new Vector3(m_PopMenuBackground.transform.localScale.x, (float)phraseCount * m_PopMenuGrid.cellHeight + 10f, m_PopMenuBackground.transform.localScale.z);
	}
}
