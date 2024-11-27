using System.Collections.Generic;

public class SceneInfo
{
	public CityInfo mCity;

	public Dictionary<byte, SpawnPointInfo> mSpawnPointDictionary = new Dictionary<byte, SpawnPointInfo>();

	public Dictionary<byte, ExploreItemBlockInfo> mExploreItemBlockDictionary = new Dictionary<byte, ExploreItemBlockInfo>();

	public Dictionary<int, bool> mChestOpenState = new Dictionary<int, bool>();
}
