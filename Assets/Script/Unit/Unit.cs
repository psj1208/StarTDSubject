using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected int level = 0;
    [SerializeField] protected float atk = 1f;
    protected float curTime;
    [SerializeField] protected float attackTerm = 1f;
    [SerializeField] protected float attackRadius = 1f;
    [SerializeField] protected LayerMask targetLayer;
    protected Vector2 size;
    protected Vector2 center;
    protected Collider2D[] hits;
    protected abstract void Attack();

    protected virtual void Start()
    {
        Check();
        curTime = 0;
    }

    protected virtual void Update()
    {
        curTime += Time.deltaTime;
        hits = Physics2D.OverlapBoxAll(center, size, 0f, targetLayer);
        Attack();    
    }

    public void Check()
    {
        size = new Vector2(TileAbout.tileSize * (attackRadius * 2) + TileAbout.tileSize / 2, TileAbout.tileSize * (attackRadius * 2) + TileAbout.tileSize / 2);
        center = transform.position;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 size = new Vector2(TileAbout.tileSize * (attackRadius * 2) + TileAbout.tileSize / 2, TileAbout.tileSize * (attackRadius * 2) + TileAbout.tileSize / 2);
        Gizmos.DrawWireCube(transform.position, size);
    }
}
