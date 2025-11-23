using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class BuffState
{
    public bool isBuff;
    public float atkPercent;

    public BuffState(float atkPercent = 0)
    {
        isBuff = true;
        this.atkPercent = atkPercent;
    }

    public void SetBuffState(bool value)
    {
        isBuff = value;
        if (!value)
            Reset();
    }

    public void Reset()
    {
        atkPercent = 0f;
    }
}

public class PointerObject : MonoBehaviour
{
    [Header("버프 관련")]
    [SerializeField] protected SpriteRenderer buffSprite;
    [SerializeField] protected BuffState buff;
    public BuffState Buff { get {  return buff; } }
    [Space]
    protected bool buildable;
    protected SpriteRenderer buildImage;
    protected Unit unit;
    protected BaseUI controlUI;
    public BaseUI ControlUI { get { return controlUI; } }
    public Unit Unit 
    { 
        get { return unit; }
        set
        { 
            unit = value;
            Buildable = false;
            unit.SetBuff(Buff);
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
    protected virtual void Start()
    {
        transform.parent = BuildManager.Instance.PointParent;
        buildImage = GetComponentInChildren<SpriteRenderer>();
        buildImage.enabled = false;
        BuildManager.Instance.AddObject(this);
        Buildable = true;
    }

    protected virtual void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (controlUI == null)
        {
            if (unit != null && unit.Level == BuildManager.Instance.UnitLevelMax)
            {
                if (unit.HasSkill)
                    return;
                UIManager.Instance.show<ExceedUnitUI>((prefab) =>
                {
                    controlUI = prefab;
                    prefab.Init(this);
                });
            }
            else if (unit != null && unit.Level < BuildManager.Instance.UnitLevelMax)
            {
                if (unit.Level == BuildManager.Instance.UnitLevelMax - 1 && GameManager.Instance.finalUnit != null)
                    return;
                UIManager.Instance.show<UnitUpUI>((prefab) =>
                {
                    controlUI = prefab;
                    prefab.Init(this);
                });
            }
            else if (unit == null && buildable && BuildManager.Instance.BuildMode)
            {
                UIManager.Instance.show<BuyUnitUI>((prefab) =>
                {
                    controlUI = prefab;
                    prefab.Init(this);
                });
            }
        }
    }

    public void ControlBuildImage(bool value = false)
    {
        /*
        if (buildable)
            buildImage.enabled = value;
        else
            buildImage.enabled = false;
        */
    }

    public void ControlBuff(bool value, BuffState state = null)
    {
        if (value)
        {
            buffSprite.enabled = true;
            buff = state;
            buff.SetBuffState(true);
        }
        else
        {
            buffSprite.enabled = false;
            if (buff != null)
                buff.SetBuffState(false);
        }
    }

    public void UnitRemove()
    {
        if (unit != null)
            Destroy(unit.gameObject);

        BuildManager.Instance.RemoveInDictionary(this);
        Buildable = true;
    }

    private void OnDestroy()
    {
        BuildManager.Instance.RemoveObject(this);
    }
}
