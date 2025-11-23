using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Tilemaps;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.DebugUI;

public class UnitGroup
{
    List<GameObject> list = new List<GameObject>();
    public List<GameObject> List {  get { return list; } }

    public UnitGroup(List<GameObject> input)
    {
        list = input;
    }
}

public class BuildManager : Singleton<BuildManager>
{
    protected override bool dontDestroy => false;
    [SerializeField] bool buildMode = false;
    public bool BuildMode {  get { return buildMode; } }

    [SerializeField] private Transform pointerParent;
    [SerializeField] private Transform towerParent;
    [SerializeField] private List<PointerObject> objects = new List<PointerObject>();
    private Dictionary<PointerObject, Unit> unitDictionary = new Dictionary<PointerObject, Unit>();

    private Dictionary<int,UnitGroup> groups = new Dictionary<int, UnitGroup>();
    private TileBase buildableTile;
    private int unitLevelMax = 3;
    public int UnitLevelMax { get { return unitLevelMax; } }

    public Transform PointParent
    {
        get
        {
            if (pointerParent == null)
                pointerParent = new GameObject("PointerParent").transform;
            return pointerParent;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        for (int a = 0; a <= unitLevelMax; a++)
            loadTasks[$"Level{a}"] = new TaskCompletionSource<bool>();

        LoadObject();
    }

    public void SetTestBuff()
    {
        List<int> ints = Util.GetRandomIndexes(objects.Count, 2);
        for (int i = 0; i < objects.Count; i++)
        {
            foreach (var val in ints)
            {
                if (i == val)
                {
                    objects[i].ControlBuff(true, new BuffState(.1f));
                    break;
                }
                else
                {
                    objects[i].ControlBuff(false);
                }
            }
        }    
    }
    #region 건설 모드, 건설 관련
    public void AddObject(PointerObject obj)
    {
        objects.Add(obj);
    }

    public void RemoveObject(PointerObject obj)
    {
        if(objects.Contains(obj))
            objects.Remove(obj);
    }

    public void ControlBuildMode(bool value = true)
    {
        foreach (var obj in objects)
            obj.ControlBuildImage(value);
        buildMode = value;
    }

    public void BuildModeRefresh()
    {
        if (buildMode)
        {
            foreach (var obj in objects)
                obj.ControlBuildImage(true);
        }
    }

    public void UnitUpAction(PointerObject obj)
    {
        if (ReturnSame(obj, out var result))
        {
            int nextLevel = obj.Unit.Level + 1;
            result.UnitRemove();
            obj.UnitRemove();
            BuildAction(obj, groups[nextLevel].List[0]);
        }
        else
            Debug.Log("동일한 것이 없습니다.");
    }

    public bool ReturnSame(PointerObject obj, out PointerObject result)
    {
        foreach (var div in unitDictionary)
        {
            if (div.Value.Type == obj.Unit.Type && div.Value.Level == obj.Unit.Level)
            {
                if (div.Key != obj)
                {
                    result = div.Key;
                    return true;
                }
            }
        }
        result = null;
        return false;
    }

    public void AddDictionary(PointerObject obj, Unit unit)
    {
        unitDictionary[obj] = unit;
    }

    public void RemoveInDictionary(PointerObject obj)
    {
        if (unitDictionary.ContainsKey(obj))
        {
            unitDictionary.Remove(obj);
        }
    }
    #endregion

    public void ReplaceTile(Vector3 worldPos)
    {
        TileUtility.ReplaceTile(GameManager.Instance.Tilemap, worldPos, buildableTile);
    }

    /// <summary>
    /// 건설 시도
    /// </summary>
    /// <param name="obj"></param>
    public async void TryFirstBuild(PointerObject obj)
    {
        for (int a = 0; a <= unitLevelMax; a++)
            await loadTasks[$"Level{a}"].Task;

        if (obj.Unit != null)
            return;

        if (towerParent == null)
            towerParent = new GameObject("TowerParent").transform;

        BuildAction(obj, groups[0].List[0]);
    }

    /// <summary>
    /// 실제 건설
    /// </summary>
    /// <param name="obj"></param>
    private void BuildAction(PointerObject obj, GameObject prefab)
    {
        obj.Unit = Instantiate(prefab, obj.transform.position, Quaternion.identity, towerParent).GetComponent<Unit>();
    }

    public void ExceedUnit(PointerObject obj)
    {
        List<Exceed> skills = SkillManager.Instance.GetRandomSkill(3);
        UIManager.Instance.show<SkillSelectUI>((prefab) =>
        {
            prefab.Init(obj.Unit, skills);
        });
    }

    /// <summary>
    /// 에셋 로드
    /// </summary>
    private void LoadObject()
    {
        AddressManager.Instance.LoadAssetAsync<TileBase>("Build_Tile", (prefab) =>
        {
            buildableTile = prefab;
        });
        for (int a = 0; a <= unitLevelMax; a++)
        {
            int level = a;
            AddressManager.Instance.LoadAssetsAsync<GameObject>($"Level{level}", (prefabList) =>
            {
                groups[level] = new UnitGroup(prefabList);
                loadTasks[$"Level{level}"].TrySetResult(true);
            });
        }
    }
}
