using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestScript : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.SetPath();

        GameManager.Instance.GameStart(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Q 입력");
            BuildManager.Instance.ControlBuildMode(true);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E 입력");
            BuildManager.Instance.ControlBuildMode(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R 입력");
            GameManager.Instance.WaveStart();
        }
    }
}
