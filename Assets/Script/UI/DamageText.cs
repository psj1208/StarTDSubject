using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageText : BaseUI
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float duration = 1f;

    private Color originalColor;
    private float timer = 0f;

    protected override void Start()
    {
        base.Start();
        originalColor = text.color;
    }

    public void SetDamage(float dam, Vector3 pos)
    {
        text.text = dam.ToString();
        transform.position = pos;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0f, timer / duration);
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (timer >= duration)
            Destroy(gameObject);
    }
}
