using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    public void ShowCountdown(Action onComplete)
    {
        StartCoroutine(CountdownCoroutine(onComplete));
    }

    private IEnumerator CountdownCoroutine(Action onComplete)
    {
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            countdownText.transform.localScale = Vector3.zero;
            countdownText.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "FIGHT!";
        countdownText.transform.DOScale(Vector3.one * 1.2f, 0.4f).SetLoops(2, LoopType.Yoyo);
        yield return new WaitForSeconds(1f);

        countdownText.text = "";
        onComplete?.Invoke();
    }
}
