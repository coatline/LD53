using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Extensions
{
    public static float AngleFromPosition(Vector3 pivotPosition, Vector3 pos)
    {
        float angleRad = Mathf.Atan2(pos.y - pivotPosition.y, pos.x - pivotPosition.x);
        float angleDeg = (180 / Mathf.PI) * angleRad - 90;
        return angleDeg;
    }

    public static Vector2Int ToOctant(Vector2 vec)
    {
        float angle = Mathf.Atan2(vec.y, vec.x);
        int octant = Mathf.RoundToInt(8 * angle / (2 * Mathf.PI) + 8) % 8;

        CompassDir dir = (CompassDir)octant;

        switch (dir)
        {
            case CompassDir.NE: return new Vector2Int(1, 1);
            case CompassDir.N: return new Vector2Int(0, 1);
            case CompassDir.E: return new Vector2Int(1, 0);
            case CompassDir.SE: return new Vector2Int(1, -1);
            case CompassDir.S: return new Vector2Int(0, -1);
            case CompassDir.SW: return new Vector2Int(-1, -1);
            case CompassDir.W: return new Vector2Int(-1, 0);
            case CompassDir.NW: return new Vector2Int(-1, 1);
            default: return Vector2Int.zero;
        }
    }

    enum CompassDir
    {
        E = 0, NE = 1,
        N = 2, NW = 3,
        W = 4, SW = 5,
        S = 6, SE = 7
    };
}

