using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusInfo : MonoBehaviour
{
    [SerializeField] private Transform mineralRight;
    [SerializeField] private Transform mineralLeft;
    bool directionRight = true;

    private void Awake()
    {
        GameResourceManager.Instance.AddDictionary<NexusInfo>(this);
    }

    public void CreateProbe()
    {
        directionRight = !directionRight;
        AddressManager.Instance.LoadAssetAsync<GameObject>("Probe", (prefab) =>
        {
            GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
            Transform target = directionRight ? mineralRight : mineralLeft;
            obj.GetComponent<Probe>().Init(transform, target);
        });
    }
}
