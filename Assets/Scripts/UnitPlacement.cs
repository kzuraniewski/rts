using System.Collections.Generic;
using UnityEngine;

public class UnitPlacement
{
	private static float RING_SPACE = 0.3f;
	private static int FIRST_RING_UNITS = 3;

	private static Vector3 GetAngleVector(float angle, float length = 1f)
	{
		return Quaternion.Euler(0, 0, angle) * new Vector3(length, 0);
	}

	private static float GetCircumference(float r)
	{
		return Mathf.PI * r * r;
	}

	private static int GetRingUnits(int n, float radius)
	{
		return Mathf.FloorToInt((FIRST_RING_UNITS * GetCircumference(radius)) / GetCircumference(RING_SPACE));
	}

	public static List<Vector2> GetGridOffsets(int count, float maxRandomOffset = 0f)
	{
		List<Vector2> targetPositions = new List<Vector2>(count);
		List<int> ringUnitCountList = new List<int>();
		float radius = RING_SPACE;

		while (count > 0)
		{
			int unitsCount = GetRingUnits(count, radius);
			ringUnitCountList.Add(unitsCount);
			radius += RING_SPACE;
			count -= unitsCount;
		}

		for (int i = 0; i < ringUnitCountList.Count; i++)
		{
			for (int j = 0; j < ringUnitCountList[i]; j++)
			{
				Vector2 newPosition = GetAngleVector(j * (360f / ringUnitCountList[i]), (i + 1) * RING_SPACE);
				Vector2 randomOffsetVector = GetAngleVector(Random.Range(0, 360f), Random.Range(0, maxRandomOffset));

				targetPositions.Add(newPosition + randomOffsetVector);
			}
		}

		return targetPositions;
	}
}