using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestScript : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameResourceManager.Instance.AddResource(GameResType.Coin, 50);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R ют╥б");
            GameManager.Instance.TryWaveStart();
        }
    }
}
