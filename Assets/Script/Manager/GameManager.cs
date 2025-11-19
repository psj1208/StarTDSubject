using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] List<Enemy> curEnemyList = new List<Enemy>();
    [SerializeField] Commander commander;

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
        AddressManager.Instance.LoadAssetAsync<GameObject>("Commander", (prefab) =>
        {
            commander = Instantiate(prefab, endPos, Quaternion.identity).GetComponent<Commander>();
        });
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
            return;

        EnemyData data = curStage.EnemyList[curWaveCount];

        AddressManager.Instance.LoadAssetAsync<GameObject>(data.EnemyName, (prefab) =>
        {
            StartCoroutine(SpawnCorutine(prefab, data.EnemyCount));
        });
    }

    void WaveEnd()
    {
        //실제 액션 넣을 곳.

        Debug.Log($"[GameManager] Wave{curWaveCount} 종료!");
        curWaveCount++;

        if (curWaveCount == waveEndCount)
        {
            Debug.Log($"Stage {curStageCount} 의 총 Wave {curWaveCount} 종료!");
            return;
        }
    }

    IEnumerator SpawnCorutine(GameObject prefab, int count = 1)
    {
        for (int a = 0; a < count; a++)
        {
            curEnemyList.Add(Instantiate(prefab, startPos, Quaternion.identity).GetComponent<Enemy>());
            yield return new WaitForSeconds(0.5f);
        }

        while (curEnemyList.Count > 0)
        {
            yield return null;
        }

        WaveEnd();
    }

    public void RemoveEnemyInList(Enemy enemy)
    {
        if(curEnemyList.Contains(enemy))
            curEnemyList.Remove(enemy);
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
