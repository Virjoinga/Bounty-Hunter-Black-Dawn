public abstract class RobotUserState
{
	private RobotUser mRobotUser;

	public void SetRobotUser(RobotUser robotUser)
	{
		mRobotUser = robotUser;
	}

	protected RobotUser GetRobotUser()
	{
		return mRobotUser;
	}

	public void Create()
	{
		OnCreate();
	}

	protected virtual void OnCreate()
	{
	}

	public void Start()
	{
		OnStart();
	}

	protected virtual void OnStart()
	{
	}

	public void Exit()
	{
		OnExit();
	}

	protected virtual void OnExit()
	{
	}

	public void Destroy()
	{
		OnDestroy();
	}

	protected virtual void OnDestroy()
	{
	}

	public void Update(RobotStateEvent eventID)
	{
		OnUpdate(eventID);
	}

	protected virtual void OnUpdate(RobotStateEvent eventID)
	{
	}
}
