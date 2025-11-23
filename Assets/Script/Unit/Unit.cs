using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum UnitType
{
    White
}

public abstract class Unit : MonoBehaviour
{
    [Header("버프 관련")]
    [SerializeField] protected BuffState buff;
    [Space]
    [Header("스킬 관련")]
    [SerializeField] protected List<SkillBase> skills = new List<SkillBase>();
    public bool HasSkill { get { return skills.Count > 0 ? true : false; } }
    [Space]
    BoxCollider2D col;
    [SerializeField] protected int level = 0;
    public int Level {  get { return level; } }
    [SerializeField] protected UnitType type = UnitType.White;
    public UnitType Type { get { return type; } }
    [Space]
    [Header("About Attack")]
    [SerializeField] protected float atk = 1f;
    [SerializeField] protected float atkPercentPerReinforce = .3f;
    [SerializeField] protected float ReinforceLevel;
    public float TotalAtk 
    { 
        get 
        {
            if (buff != null && buff.isBuff)
                return atk * (1 + (atkPercentPerReinforce * ReinforceLevel) + buff.atkPercent);
            else
                return atk * (1 + (atkPercentPerReinforce * ReinforceLevel));
        } 
    }
    protected float curTime;
    [SerializeField] protected float attackTerm = 1f;
    [SerializeField] protected float attackRadius = 1f;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected List<Enemy> hits = new List<Enemy>();
    [SerializeField] protected Animator animator;
    protected abstract void Attack();

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        col =GetComponent<BoxCollider2D>();
        Util.SetCollider2DWorldSize(col, TileAbout.tileSize, attackRadius);
        ReinforceLevel = GameResourceManager.Instance.GetReinForceLevel(level);
        curTime = 0;
    }

    public virtual void SetBuff(BuffState state)
    {
        buff = state;
    }

    protected virtual void OnEnable()
    {
        GameResourceManager.Instance.ReinforecUpAction += GetReinforceLevel;
    }

    protected virtual void OnDisable()
    {
        GameResourceManager.Instance.ReinforecUpAction -= GetReinforceLevel;
    }

    protected virtual void Update()
    {
        curTime += Time.deltaTime;
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (hits.Contains(enemy))
                return;
            hits.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (hits.Contains(enemy))
                hits.Remove(enemy);
        }
    }

    private void GetReinforceLevel(int le,int num)
    {
        if (this.level == le)
            ReinforceLevel = num;
        else
            ReinforceLevel = 0;
    }

    public virtual void AddSkill(SkillBase skill)
    {
        skills.Add(skill);
    }

    public virtual void RemoveSkill(SkillBase skill)
    {
        if (skills.Contains(skill))
            skills.Remove(skill);
    }
    protected void OnDrawGizmosSelected()
    {
        float tileSize = TileAbout.tileSize;
        Gizmos.color = Color.red;
        Vector2 size = new Vector2(tileSize * (attackRadius * 2 + 1) - tileSize * 0.5f, tileSize * (attackRadius * 2 + 1) - tileSize * 0.5f);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
