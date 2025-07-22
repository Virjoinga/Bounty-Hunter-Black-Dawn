using UnityEngine;

public class EnemyLevelColor
{
	private enum EnemyLevel
	{
		SuperInferior = 0,
		Inferior = 1,
		Equal = 2,
		Superior = 3,
		SuperSuperior = 4
	}

	private static Color[] levelColors = new Color[5]
	{
		new Color(0.7490196f, 0.7490196f, 0.7490196f),
		new Color(0.57254905f, 0.8156863f, 16f / 51f),
		new Color(1f, 1f, 16f / 51f),
		new Color(0.8862745f, 0.41960785f, 2f / 51f),
		new Color(1f, 0f, 0f)
	};

	public static Color GetColorByDeltaLevel(int deltaLevel)
	{
		if (deltaLevel <= -2)
		{
			return levelColors[0];
		}
		switch (deltaLevel)
		{
		case -1:
			return levelColors[1];
		case 0:
			return levelColors[2];
		case 1:
			return levelColors[3];
		default:
			return levelColors[4];
		}
	}
}
