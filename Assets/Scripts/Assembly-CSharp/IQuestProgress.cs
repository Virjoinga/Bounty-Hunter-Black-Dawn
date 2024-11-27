public interface IQuestProgress
{
	void OnQuestProgressEnemyKill(int enemyGroupId);

	void OnQuestProgressItemCollection(short itemId);

	void OnQuestProgressTalkWithNPC(int npcId);

	void OnQuestProgressEnemyKillall(short questId);
}
