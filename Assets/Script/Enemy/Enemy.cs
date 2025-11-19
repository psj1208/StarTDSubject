using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float curHp;
    [SerializeField] private float maxHp = 20;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private int minusLife = 1;

    [SerializeField] private List<Vector3> path;
    [SerializeField] private int currentIndex = 0;

    private void Start()
    {
        curHp = maxHp;
        SetPath(GameManager.Instance.Path);

        StartCoroutine(MoveAlongPath());
    }

    public void SetPath(List<Vector3> newPath)
    {
        path = newPath;
        currentIndex = 0;
    }

    private IEnumerator MoveAlongPath()
    {
        while (path == null || path.Count == 0)
            yield return null;

        while (currentIndex < path.Count)
        {
            Vector3 target = path[currentIndex];

            while (Vector3.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(
                        transform.position,
                        target,
                        moveSpeed * Time.deltaTime
                    );

                yield return null;
            }

            transform.position = target;
            currentIndex++;
        }

        OnDestination();
    }

    private void OnDestination()
    {
        Debug.Log("목적지 도착!");
        Destroy(gameObject);
    }

    #region 데미지 관련
    public void GetDamage(float dam)
    {
        curHp = Mathf.Clamp(curHp - dam, 0, maxHp);
        Debug.Log($"{gameObject.name}이 공격받음! Dam : {dam}");
        Death();
    }

    private void Death()
    {
        if (curHp <= 0)
            Destroy(gameObject);
    }
    #endregion
}
