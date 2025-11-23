using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundSkill : SkillBase
{
    float damage = 3f;
    float radius = 3f;

    private float interval = 5f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            DoDamage();
        }
    }

    private void DoDamage()
    {
        Vector2 size = new Vector2(radius * 2, radius * 2);

        Vector2 center = transform.position;

        LayerMask enemyLayer = LayerMask.GetMask("Enemy");

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, enemyLayer);

        foreach (Collider2D col in hits)
        {
            if (col.TryGetComponent(out Enemy enemy))
            {
                enemy.GetDamage(damage);
            }
        }

#if UNITY_EDITOR
        DebugDrawArea(center, size);
#endif
    }

#if UNITY_EDITOR
    private void DebugDrawArea(Vector2 center, Vector2 size)
    {
        Vector2 half = size / 2;
        Vector3 c = center;

        Debug.DrawLine(c + new Vector3(-half.x, -half.y), c + new Vector3(half.x, -half.y), Color.red, 0.2f);
        Debug.DrawLine(c + new Vector3(half.x, -half.y), c + new Vector3(half.x, half.y), Color.red, 0.2f);
        Debug.DrawLine(c + new Vector3(half.x, half.y), c + new Vector3(-half.x, half.y), Color.red, 0.2f);
        Debug.DrawLine(c + new Vector3(-half.x, half.y), c + new Vector3(-half.x, -half.y), Color.red, 0.2f);
    }
#endif
}
