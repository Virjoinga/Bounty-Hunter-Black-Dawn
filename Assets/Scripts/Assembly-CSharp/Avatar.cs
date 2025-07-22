public class Avatar
{
	public enum State
	{
		Unpurchased = 0,
		Purchased = 1,
		Equipped = 2
	}

	private string mName;

	private int mPrice;

	private CharacterClass mCharClass;

	private Sex mCharSex;

	private State mState;

	public string Name
	{
		get
		{
			return mName;
		}
	}

	public int Price
	{
		get
		{
			return mPrice;
		}
	}

	public State CurrentState
	{
		get
		{
			return mState;
		}
	}

	public CharacterClass CharClass
	{
		get
		{
			return mCharClass;
		}
	}

	public Sex CharSex
	{
		get
		{
			return mCharSex;
		}
	}

	public Avatar(string name, int price, CharacterClass characterClass, Sex sex)
	{
		mName = name;
		mPrice = price;
		mCharClass = characterClass;
		mCharSex = sex;
		mState = State.Unpurchased;
	}

	public void SetState(State state)
	{
		mState = state;
	}
}
