using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSelectUI : GameWinUI
{
    [SerializeField] protected Transform skillParentTransform;
    public void Init(Unit inputUnit, List<Exceed> skills)
    {
        foreach (Exceed exceed in skills)
        {
            Exceed temp = exceed;

            AddressManager.Instance.LoadAssetAsync<GameObject>("SkillPrefab", (prefab) =>
            {
                SkillPrefab sp = Instantiate(prefab, skillParentTransform).GetComponent<SkillPrefab>();
                sp.Init(temp, inputUnit, this);
            });
        }
    }

    public void SelectOver()
    {
        Destroy(gameObject);
    }
}
