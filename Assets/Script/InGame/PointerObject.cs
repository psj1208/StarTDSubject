using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PointerObject : MonoBehaviour
{
    bool buildable;
    SpriteRenderer buildImage;
    Unit unit;
    BaseUI controlUI;
    public BaseUI ControlUI { get { return controlUI; } }
    public Unit Unit 
    { 
        get { return unit; }
        set
        { 
            unit = value;
            Buildable = false;
            BuildManager.Instance.AddDictionary(this, unit);
        }
    }

    public bool Buildable 
    { 
        get { return buildable; } 
        set 
        { 
            buildable = value;
            BuildManager.Instance.BuildModeRefresh();
        } 
    }

    private void Start()
    {
        transform.parent = BuildManager.Instance.PointParent;
        Buildable = true;
        buildImage = GetComponentInChildren<SpriteRenderer>();
        buildImage.enabled = false;
        BuildManager.Instance.AddObject(this);
    }

    private void OnMouseDown()
    {
        if (controlUI == null)
        {
            if (unit == null && buildable && BuildManager.Instance.BuildMode)
            {
                UIManager.Instance.show<BuyUnitUI>((prefab) =>
                {
                    controlUI = prefab;
                    prefab.Init(this);
                });
            }
            else if (unit != null && unit.Level + 1 < BuildManager.Instance.UnitLevelMax)
            {
                UIManager.Instance.show<UnitUpUI>((prefab) =>
                {
                    controlUI = prefab;
                    prefab.Init(this);
                });
            }
        }
    }

    public void ControlBuildImage(bool value = false)
    {
        if (buildable)
            buildImage.enabled = value;
        else
            buildImage.enabled = false;
    }

    public void UnitRemove()
    {
        if (unit != null)
            Destroy(unit.gameObject);

        BuildManager.Instance.RemoveInDictionary(this);
        Buildable = true;
    }
}
