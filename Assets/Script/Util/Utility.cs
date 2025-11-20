using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class Util
{
    /// <summary>
    /// 타일 1칸의 사이즈에 맞춰 해당 콜라이더의 사이즈를 Radius만큼 조절
    /// </summary>
    /// <param name="col">콜라이더</param>
    /// <param name="tileSize">타일 1칸 크기</param>
    /// <param name="radius">범위</param>
    public static void SetCollider2DWorldSize(BoxCollider2D col, float tileSize, float radius)
    {
        float worldSize = tileSize * (2 * radius + 1);
        Vector3 lossy = col.transform.lossyScale;

        Vector2 newLocalSize = new Vector2(worldSize / lossy.x - tileSize * 0.5f, worldSize / lossy.y - tileSize * 0.5f);

        col.size = newLocalSize;
    }

    /// <summary>
    /// 월드의 좌표를 캔버스(오버레이) 상의 좌표로 변환
    /// </summary>
    /// <param name="world">타겟 좌표</param>
    /// <param name="canvasRect">계산의 지표가 되어줄 캔버스</param>
    /// <returns></returns>
    public static Vector2 WorldToCanvasInOverlay(Vector2 world, RectTransform canvasRect)
    {
        Vector2 screen = Camera.main.WorldToScreenPoint(world);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screen, null, out Vector2 localPos))
        {
            return localPos;
        }
        return Vector2.zero;
    }

    /// <summary>
    /// 월드의 좌표를 캔버스(카메라) 상의 좌표로 변환
    /// </summary>
    /// <param name="worldPos"></param>
    /// <param name="canvasRect"></param>
    /// <param name="uiCamera"></param>
    /// <returns></returns>
    public static Vector2 WorldToCanvasInCameraSpace(Vector3 worldPos, RectTransform canvasRect, Camera uiCamera)
    {
        Vector2 screenPos = uiCamera.WorldToScreenPoint(worldPos);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out Vector2 localPos))
        {
            return localPos;
        }

        return Vector2.zero;
    }

    public static Vector2 LocalToCanvasPosition(RectTransform target, RectTransform canvas)
    {
        Vector2 anchoredPos = target.anchoredPosition;
        // 부모 Rect 기준 위치를 Canvas로 변환
        return (Vector2)canvas.InverseTransformPoint(target.TransformPoint(anchoredPos));
    }
}

public static class TileUtility
{
    private static readonly Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0)
    };

    public static List<Vector3> PathFinderTilemap(Tilemap groundTilemap)
    {
        Vector3Int startTile = FindTileByType<StartTile>(groundTilemap);
        Vector3Int endTile = FindTileByType<EndTile>(groundTilemap);

        if (!groundTilemap.HasTile(startTile) || !groundTilemap.HasTile(endTile))
        {
            Debug.LogError("StartTile 또는 GoalTile을 찾지 못했습니다.");
            return null;
        }

        Queue<Vector3Int> q = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> parent = new Dictionary<Vector3Int, Vector3Int>();

        q.Enqueue(startTile);
        visited.Add(startTile);

        while (q.Count > 0)
        {
            Vector3Int cur = q.Dequeue();

            if (cur == endTile)
            {
                return BuildWorldPath(groundTilemap, parent, startTile, endTile);
            }

            foreach (var dir in directions)
            {
                Vector3Int next = cur + dir;

                if (groundTilemap.HasTile(next) && next != endTile) continue;
                if (visited.Contains(next)) continue;

                visited.Add(next);
                q.Enqueue(next);
                parent[next] = cur;
            }
        }

        return null;
    }

    private static List<Vector3> BuildWorldPath(
        Tilemap map,
        Dictionary<Vector3Int, Vector3Int> parent,
        Vector3Int start,
        Vector3Int end)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int cur = end;

        while (cur != start)
        {
            path.Add(cur);
            cur = parent[cur];
        }

        path.Add(start);
        path.Reverse();

        List<Vector3> worldPath = new List<Vector3>();
        foreach (var p in path)
            worldPath.Add(map.GetCellCenterWorld(p));

        return worldPath;
    }

    /// <summary>
    /// 특정 타입의 타일 모두 찾기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public static List<Vector3Int> FindTilesByType<T>(Tilemap map) where T : TileBase
    {
        List<Vector3Int> result = new List<Vector3Int>();

        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            if (!map.HasTile(pos)) continue;

            TileBase tile = map.GetTile(pos);
            if (tile is T)
                result.Add(pos);
        }

        return result;
    }

    /// <summary>
    /// 특정 타입의 타일 하나만 찾기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="map"></param>
    /// <returns></returns>
    public static Vector3Int FindTileByType<T>(Tilemap map) where T : TileBase
    {
        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            if (!map.HasTile(pos)) continue;

            TileBase tile = map.GetTile(pos);
            if (tile is T)
                return pos;
        }

        return new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
    }

    public static Vector3 CellToWorld(Tilemap map, Vector3Int cellPos)
    {
        return map.GetCellCenterWorld(cellPos);
    }
}
