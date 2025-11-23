using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    Animator animator;

    [SerializeField] protected float curHp;
    [SerializeField] protected float maxHp = 20;
    [SerializeField] protected float moveSpeed = 1.0f;
    [SerializeField] protected int minusLife = 1;
    [SerializeField] protected int dropCoin = 5;

    [SerializeField] protected List<Vector3> path;
    [SerializeField] protected int currentIndex = 0;
    [SerializeField] protected RectTransform dropTarget;
    protected bool isDead = false;
    public bool IsDead {  get { return isDead; } }
    protected Coroutine moveRoutine;

    protected virtual void Start()
    {
        dropTarget = UIManager.Instance.Get<CoinUI>().GetComponent<RectTransform>();
        curHp = maxHp;
        animator = GetComponentInChildren<Animator>();
        SetPath(GameManager.Instance.Path);

        moveRoutine = StartCoroutine(MoveAlongPath());
    }

    public void SetPath(List<Vector3> newPath)
    {
        path = newPath;
        currentIndex = 0;
    }

    protected virtual IEnumerator MoveAlongPath()
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

    protected virtual void OnDestination()
    {
        Debug.Log("목적지 도착!");
        GameManager.Instance.CommanderUnit.GetDamage(minusLife);
        Destroy(gameObject);
    }

    #region 데미지 관련
    public virtual void GetDamage(float dam)
    {
        curHp = Mathf.Clamp(curHp - dam, 0, maxHp);
        Debug.Log($"{gameObject.name}이 공격받음! Dam : {dam}");
        UIManager.Instance.show<DamageText>((prefab) =>
        {
            prefab.SetDamage(dam, transform.position);
        });
        Death();
    }

    protected virtual void Death()
    {
        if (curHp <= 0)
        {
            isDead = true;
            PlayAniAndDestroy(animator,"Death");
        }
    }
    #endregion

    protected virtual void PlayAniAndDestroy(Animator animator, string stateName)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
        StartCoroutine(PlayAndDestroyRoutine(animator, stateName));
    }

    protected virtual IEnumerator PlayAndDestroyRoutine(Animator animtor, string stateName)
    {
        int amount = 5;

        animator.Play(stateName);

        yield return null;

        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float animLength = info.length;

        yield return new WaitForSeconds(animLength);

        yield return new WaitForSeconds(0.5f);
        EffectManager.Instance.MadeAndThrowToTarget("CoinObject", transform.position, amount, dropTarget, dropCoin / amount);

        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        GameManager.Instance.RemoveEnemyInList(this);
    }
}