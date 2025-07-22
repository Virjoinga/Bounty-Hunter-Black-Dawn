namespace HutongGames.PlayMaker
{
	public interface INamedVariable
	{
		string Name { get; }

		bool UseVariable { get; }

		bool NetworkSync { get; }
	}
}
