using UnityEngine;
using TMPro;
using DG.Tweening;

public class VictoryLosePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI messageText;

    private void Awake()
    {
        canvasGroup.alpha = 0;
        canvasGroup.gameObject.SetActive(false);
    }

    public void ShowVictory()
    {
        ShowMessage("VICTORY", Color.yellow);
    }

    public void ShowLose()
    {
        ShowMessage("DEFEAT", Color.red);
    }

    private void ShowMessage(string text, Color color)
    {
        canvasGroup.gameObject.SetActive(true);
        messageText.text = text;
        messageText.color = color;
        canvasGroup.DOFade(1, 1f).SetEase(Ease.OutBounce);
        messageText.transform.DOScale(Vector3.one * 1.5f, 1f).From(Vector3.zero).SetEase(Ease.OutBack);
    }
}
