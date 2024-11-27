public class QuestItemWindow : ItemPopMenuWindow
{
	public UILabel Name;

	protected override void OnSetNGUIBaseItem(NGUIBaseItem nguiBaseItem)
	{
		Name.text = AddItemQualityColor(nguiBaseItem, nguiBaseItem.DisplayName);
	}
}
