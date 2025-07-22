using UnityEngine;

public interface UIRouletteListener
{
	bool IsRouletteCanBeTouchInThisPos(Vector2 pos);

	void OnRouletteFirstTouchAfterRotation();

	void OnRouletteStart();

	void OnRouletteStop(int index);
}
