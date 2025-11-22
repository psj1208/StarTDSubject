using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Probe : MonoBehaviour
{
    [SerializeField] private GameObject mineralObject;
    [SerializeField] private float MiningWaitTime = 1f;
    [SerializeField] List<Transform> moveList = new List<Transform>();
    int currentIndex = 0;

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float arriveDistance = 0.1f;

    private SpriteRenderer sprite;
    private bool isWaiting = false;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Init(params Transform[] trans)
    {
        mineralObject.SetActive(false);
        moveList.Clear();
        foreach (Transform t in trans)
            moveList.Add(t);
        currentIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveList.Count == 0 || isWaiting) return;

        Transform target = moveList[currentIndex];

        if (sprite != null)
            sprite.flipX = transform.position.x > target.position.x ? true : false;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < arriveDistance)
        {
            if (currentIndex == 0 && mineralObject.activeSelf == true)
            {
                //¹Ì³×¶ö Å‰µæ
                GameResourceManager.Instance.AddResource(GameResType.Mineral, 8);
            }
            currentIndex++;
            mineralObject.SetActive(false);

            if (currentIndex >= moveList.Count)
            {
                currentIndex = 0;
                StartCoroutine(WaitAndReturnToStart());
            }
        }
    }

    private IEnumerator WaitAndReturnToStart()
    {
        isWaiting = true;
        yield return new WaitForSeconds(MiningWaitTime);

        currentIndex = 0;
        mineralObject.SetActive(true);
        isWaiting = false;
    }
}
