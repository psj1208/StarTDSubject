using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    protected override bool dontDestroy => false;

    [SerializeField] private List<PointerObject> objects = new List<PointerObject>();

    public void AddObject(PointerObject obj)
    {
        objects.Add(obj);
    }

    public void ControlBuildMode(bool value = true)
    {
        foreach (var obj in objects)
            obj.ControlBuildImage(value);
    }
}
