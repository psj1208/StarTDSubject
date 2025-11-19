using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointerObject : MonoBehaviour
{
    bool buildable;
    SpriteRenderer buildImage;
    private void Start()
    {
        buildable = true;
        buildImage = GetComponentInChildren<SpriteRenderer>();
        buildImage.enabled = false;
        BuildManager.Instance.AddObject(this);
    }

    private void OnMouseDown()
    {
        Debug.Log($"Clicked tile object at {transform.position}");
    }

    public void ControlBuildImage(bool value = false)
    {
        if (buildable)
            buildImage.enabled = value;
        else
            buildImage.enabled = false;
    }
}
