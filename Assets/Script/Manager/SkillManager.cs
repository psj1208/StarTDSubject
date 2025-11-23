using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    protected override bool dontDestroy => false;
    private List<Exceed> skillList = new List<Exceed>();

    protected override void Awake()
    {
        base.Awake();
        loadTasks[$"Skill"] = new TaskCompletionSource<bool>();
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadSkills();
    }

    private void LoadSkills()
    {
        AddressManager.Instance.LoadAssetsAsync<Exceed>($"Skill", (prefabList) =>
        {
            skillList = prefabList;
            foreach(var skill in skillList)
            {
                Debug.Log($"[SkillManager] {skill.ToString()} 로드 완료!");
            }
            loadTasks[$"Skill"].TrySetResult(true);
        });
    }

    public List<Exceed> GetRandomSkill(int num)
    {
        List<Exceed> exceeds = new List<Exceed>();

        List<int> indexes = Util.GetRandomIndexes(skillList.Count, num);

        foreach (int idx in indexes)
        {
            exceeds.Add(skillList[idx]);
        }

        return exceeds;
    }
}
