public interface IBattleState
{
	void Init();

	void WriteToBuffer(BytesBuffer buffer);

	void ReadFromBuffer(BytesBuffer buffer);

	void SetState(IBattleState state);
}
