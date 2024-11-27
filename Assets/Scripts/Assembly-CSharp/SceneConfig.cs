using UnityEngine;

public class SceneConfig
{
	public byte SceneID { get; set; }

	public string SceneFileName { get; set; }

	public byte AreaID { get; set; }

	public byte FatherSceneID { get; set; }

	public string SceneName { get; set; }

	public string SceneIntro { get; set; }

	public Vector2 MiniMapSize { get; set; }

	public Vector3 MapTopLeft { get; set; }

	public Vector3 MapBottomLeft { get; set; }

	public Vector3 MapTopRight { get; set; }

	public Vector3 MapBottomRight { get; set; }

	public byte ArenaBelongToWhichSceneID { get; set; }

	public bool Hide { get; set; }
}
