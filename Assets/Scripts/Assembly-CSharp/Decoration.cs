public class Decoration
{
	public enum State
	{
		Unpurchased = 0,
		Purchased = 1,
		Equipped = 2
	}

	private int mPart;

	private string mName;

	private int mPrice;

	private State mState;

	public int Part
	{
		get
		{
			return mPart;
		}
	}

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

	public Decoration(int part, string name, int price)
	{
		mPart = part;
		mName = name;
		mPrice = price;
		mState = State.Unpurchased;
	}

	public void SetState(State state)
	{
		mState = state;
	}
}
