using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    protected override bool dontDestroy => false;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] List<Enemy> curEnemyList = new List<Enemy>();
    [SerializeField] Commander commanderUnit;
    public Commander CommanderUnit {  get { return commanderUnit; } }
    public event Action winAction;
    public event Action loseAction;
    public event Action<int> timeChangeAction;

    private List<Vector3> path;
    public List<Vector3> Path { get { return path; } }

    private Vector3 startPos;
    private Vector3 endPos;
    WaveData curStage;
    bool isWaveOnGoing = false;
    bool isGameEnd = false;
    int curStageCount = 0;
    int curWaveCount = 0;
    int waveEndCount = 0;

    //상수
    float curTime = 0;
    public const float waitTimePerWave = 10f;

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
            commanderUnit = Instantiate(prefab, endPos, Quaternion.identity).GetComponent<Commander>();
        });
    }

    public void MakeStage(int stage = 0)
    {
        curStage = Static.WaveLists[stage];
        curStageCount = stage;
        curWaveCount = 0;
        waveEndCount = curStage.EnemyList.Count;
    }

    public void GetWaitingTime()
    {
        BuildManager.Instance.ControlBuildMode(true);
        curTime = waitTimePerWave;
        StartCoroutine(waitTimeRoutine());
    }

    IEnumerator waitTimeRoutine()
    {
        timeChangeAction?.Invoke((int)curTime);
        
        while (curTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            curTime--;

            timeChangeAction?.Invoke((int)curTime);
        }
        curTime = 0;
        timeChangeAction?.Invoke(0);

        TryWaveStart();
    }

    public void TryWaveStart()
    {
        if (isGameEnd)
            return;

        if (isWaveOnGoing)
        {
            Debug.Log("이미 Wave가 진행 중 입니다.");
            return;
        }

        UIManager.Instance.show<WaveStartUI>();
    }

    public void WaveStart()
    {
        isWaveOnGoing = true;
        EnemyData data = curStage.EnemyList[curWaveCount];

        AddressManager.Instance.LoadAssetAsync<GameObject>(data.EnemyName, (prefab) =>
        {
            StartCoroutine(SpawnCorutine(prefab, data.EnemyCount));
        });
    }

    //단순히 웨이브 종료를 알림
    public void WaveEnd()
    {
        Debug.Log($"[GameManager] Wave{curWaveCount} 종료!");
        WaveEndAction();
    }

    //실제 동작(스테이지 ++ 같은)
    private void WaveEndAction()
    {
        isWaveOnGoing = false;
        //스테이지의 끝에 도달
        if (curWaveCount >= waveEndCount - 1)
        {
            Debug.Log($"Stage {curStageCount} 의 총 Wave {curWaveCount} 종료!");
            GameEnd(true);
        }
        else
        {
            curWaveCount++;
            UIManager.Instance.show<WaveEndUI>();
        }
    }

    public void GameEnd(bool value)
    {
        if (isGameEnd)
            return;
        //승리
        if (value)
        {
            Debug.Log("게임 승리");
            isGameEnd = true;
            winAction?.Invoke();
        }
        //패배
        else
        {
            Debug.Log("게임 패배");
            isGameEnd = true;
            loseAction?.Invoke();
        }
        BuildManager.Instance.ControlBuildMode(false);
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
