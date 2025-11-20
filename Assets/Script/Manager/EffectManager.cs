using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public void MadeAndThrowToTarget(string key, Vector2 startPos,int amount, RectTransform targetRect, int value = 1)
    {
        AddressManager.Instance.LoadAssetAsync<GameObject>(key, async (prefab) =>
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = Instantiate(prefab);
                RectTransform rect = obj.GetComponent<RectTransform>();
                RectTransform mainRect = UIManager.Instance.MainCanvas.GetComponent<RectTransform>();
                obj.GetComponent<RectEffectObject>().Init(value);

                rect.SetParent(mainRect, false);

                rect.anchoredPosition = Util.WorldToCanvasInCameraSpace(startPos, mainRect, Camera.main);
                Vector2 targetPos = Util.LocalToCanvasPosition(targetRect, mainRect);

                Vector2 randomOffset = Random.insideUnitCircle * Random.Range(50f, 150f);
                Vector2 spreadPos = rect.anchoredPosition + randomOffset;

                float spreadDuration = Random.Range(0.5f, 0.8f);
                float gatherDuration = Random.Range(0.5f, 2f);

                Sequence seq = DOTween.Sequence();

                seq.Append(rect.DOAnchorPos(spreadPos, spreadDuration).SetEase(Ease.OutQuad))
                   .Append(rect.DOAnchorPos(targetPos, gatherDuration).SetEase(Ease.InBack))
                   .OnComplete(() =>
                   {
                       Destroy(obj);
                   });

                await Task.Yield();
            }
        });
    }
}
