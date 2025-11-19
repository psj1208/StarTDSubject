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
    BoxCollider2D col;
    [SerializeField] protected int level = 0;
    public int Level {  get { return level; } }
    [SerializeField] protected UnitType type = UnitType.White;
    public UnitType Type { get { return type; } }
    [SerializeField] protected float atk = 1f;
    protected float curTime;
    [SerializeField] protected float attackTerm = 1f;
    [SerializeField] protected float attackRadius = 1f;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected List<Collider2D> hits = new List<Collider2D>();
    protected abstract void Attack();

    protected virtual void Start()
    {
        col =GetComponent<BoxCollider2D>();
        Util.SetCollider2DWorldSize(col, TileAbout.tileSize, attackRadius);
        curTime = 0;
    }

    protected virtual void Update()
    {
        curTime += Time.deltaTime;
        Attack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Enter: " + collision.name);
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (hits.Contains(collision))
                return;
            hits.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (hits.Contains(collision))
                hits.Remove(collision);
        }
    }

    protected void OnDrawGizmosSelected()
    {
        float tileSize = TileAbout.tileSize;
        Gizmos.color = Color.red;
        Vector2 size = new Vector2(tileSize * (attackRadius * 2 + 1) - tileSize * 0.5f, tileSize * (attackRadius * 2 + 1) - tileSize * 0.5f);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
