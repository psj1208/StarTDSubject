using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Tilemap tilemap;
    private List<Vector3> path;
    public List<Vector3> Path { get { return path; } }

    private Vector3 startPos;
    private Vector3 endPos;
    WaveData curStage;
    int curStageCount = 0;
    int curWaveCount = 0;
    int waveEndCount = 0;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
    }

    public void SetPath()
    {
        path = TileUtility.PathFinderTilemap(tilemap);

        startPos = TileUtility.CellToWorld(tilemap, TileUtility.FindTileByType<StartTile>(tilemap));
        endPos = TileUtility.CellToWorld(tilemap, TileUtility.FindTileByType<EndTile>(tilemap));
    }

    public void GameStart(int stage = 0)
    {
        curStage = Static.WaveLists[stage];
        curStageCount = stage;
        curWaveCount = 0;
        waveEndCount = curStage.EnemyList.Count;
    }

    public void WaveStart()
    {
        if (curWaveCount == waveEndCount)
        {
            Debug.Log($"Stage {curStageCount} ÀÇ ÃÑ Wave {curWaveCount} Á¾·á!");
            return;
        }

        EnemyData data = curStage.EnemyList[curWaveCount];

        AddressManager.Instance.LoadAssetAsync<GameObject>(data.EnemyName, (prefab) =>
        {
            StartCoroutine(SpawnCorutine(prefab, data.EnemyCount));
        });
        curWaveCount++;
    }

    IEnumerator SpawnCorutine(GameObject prefab, int count = 1)
    {
        for (int a = 0; a < count; a++)
        {
            Instantiate(prefab, startPos, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void OnDrawGizmos()
    {
        if (path == null || path.Count < 2)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Gizmos.DrawLine(path[i], path[i + 1]);
            Gizmos.DrawSphere(path[i], 0.05f);
        }

        Gizmos.color = Color.green;
        if (startPos != null)
            Gizmos.DrawWireCube(startPos, Vector3.one);
        Gizmos.color = Color.blue;
        if (endPos != null)
            Gizmos.DrawWireCube(endPos, Vector3.one);
    }
}
