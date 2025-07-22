public class AchievementTrigger
{
	public enum Type
	{
		Start = 0,
		Stop = 1,
		Reset = 2,
		Data = 3,
		Report = 4
	}

	private AchievementID id;

	private int mAdditionalPart;

	private int mData;

	private Type type;

	private static AchievementTrigger Trigger = new AchievementTrigger();

	public AchievementID ID
	{
		get
		{
			return id;
		}
	}

	public int AdditionalPart
	{
		get
		{
			return mAdditionalPart;
		}
	}

	public int Data
	{
		get
		{
			return mData;
		}
	}

	public Type TriggerType
	{
		get
		{
			return type;
		}
	}

	private void SetId(AchievementID id)
	{
		this.id = id;
	}

	private void SetType(Type type)
	{
		this.type = type;
	}

	public void PutData(int data)
	{
		PutData(data, 0);
	}

	public void PutData(int data, int additionalPart)
	{
		mData = data;
		mAdditionalPart = additionalPart;
	}

	public static AchievementTrigger Create(AchievementID id)
	{
		return Create(id, Type.Data);
	}

	public static AchievementTrigger Create(AchievementID id, Type type)
	{
		Trigger.SetId(id);
		Trigger.SetType(type);
		Trigger.PutData(0, 0);
		return Trigger;
	}
}
