using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    Animator animator;

    [SerializeField] private float curHp;
    [SerializeField] private float maxHp = 20;
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private int minusLife = 1;
    [SerializeField] private int dropCoin = 5;

    [SerializeField] private List<Vector3> path;
    [SerializeField] private int currentIndex = 0;
    [SerializeField] private RectTransform dropTarget;
    private bool isDead = false;
    public bool IsDead {  get { return isDead; } }
    private Coroutine moveRoutine;

    private void Start()
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
        GameManager.Instance.CommanderUnit.GetDamage(minusLife);
        Destroy(gameObject);
    }

    #region 데미지 관련
    public void GetDamage(float dam)
    {
        curHp = Mathf.Clamp(curHp - dam, 0, maxHp);
        Debug.Log($"{gameObject.name}이 공격받음! Dam : {dam}");
        UIManager.Instance.show<DamageText>((prefab) =>
        {
            prefab.SetDamage(dam, transform.position);
        });
        Death();
    }

    private void Death()
    {
        if (curHp <= 0)
        {
            isDead = true;
            PlayAniAndDestroy(animator,"Death");
        }
    }
    #endregion

    private void PlayAniAndDestroy(Animator animator, string stateName)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);
        StartCoroutine(PlayAndDestroyRoutine(animator, stateName));
    }

    private IEnumerator PlayAndDestroyRoutine(Animator animtor, string stateName)
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

    private void OnDestroy()
    {
        GameManager.Instance.RemoveEnemyInList(this);
    }
}