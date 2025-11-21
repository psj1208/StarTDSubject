using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : BaseUI
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float duration = 1f;

    private Color originalColor;
    private float timer = 0f;

    public void Init(float num, Vector3 pos)
    {
        transform.position = pos;
        text.text = num >= 0 ? $"+{num}" : $"{num}";
        text.color = num >= 0 ? Color.blue : Color.red;
        originalColor = text.color;
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
