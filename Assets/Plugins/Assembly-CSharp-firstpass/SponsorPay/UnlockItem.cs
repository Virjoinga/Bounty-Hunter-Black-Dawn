namespace SponsorPay
{
	public class UnlockItem
	{
		public string Id { get; private set; }

		public string Name { get; private set; }

		public bool IsUnlocked { get; private set; }

		public long UnlockTimestamp { get; private set; }

		public UnlockItem(string id, string name, bool isUnlocked, long unlockTimestamp)
		{
			Id = ((id == null) ? string.Empty : id);
			Name = ((name == null) ? string.Empty : name);
			IsUnlocked = isUnlocked;
			UnlockTimestamp = unlockTimestamp;
		}
	}
}
