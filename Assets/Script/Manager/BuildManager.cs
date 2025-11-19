using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Tilemaps;
using UnityEditorInternal;
using UnityEngine;
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
    Dictionary<string, TaskCompletionSource<bool>> loadTasks = new Dictionary<string, TaskCompletionSource<bool>>();
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
        for (int a = 0; a < unitLevelMax; a++)
            loadTasks[$"Level{a}"] = new TaskCompletionSource<bool>();

        LoadObject();
    }

    public void AddObject(PointerObject obj)
    {
        objects.Add(obj);
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

    /// <summary>
    /// 건설 시도
    /// </summary>
    /// <param name="obj"></param>
    public async void TryFirstBuild(PointerObject obj)
    {
        for (int a = 0; a < unitLevelMax; a++)
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
        unitDictionary[obj] = obj.Unit;
    }

    /// <summary>
    /// 에셋 로드
    /// </summary>
    private void LoadObject()
    {
        for (int a = 0; a < unitLevelMax; a++)
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
