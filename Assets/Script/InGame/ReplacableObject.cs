using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReplacableObject : PointerObject
{
    protected override void Start()
    {
        transform.parent = BuildManager.Instance.PointParent;
        buildImage = GetComponentInChildren<SpriteRenderer>();
        buildImage.enabled = false;
        Buildable = true;
    }

    protected override void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (controlUI == null)
        {
            if (buildable && BuildManager.Instance.BuildMode)
            {
                UIManager.Instance.show<ReplaceTileUI>((prefab) =>
                {
                    controlUI = prefab;
                    prefab.Init(this);
                });
            }
        }
    }
}
