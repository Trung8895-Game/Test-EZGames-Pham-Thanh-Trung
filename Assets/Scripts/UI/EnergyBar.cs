using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    private PlayerController playerController;

    public void Init(PlayerController _playerController)
    {
        playerController = _playerController;
        playerController.OnEnergyChanged += UpdateUI;
    }

    public void UpdateUI(float currentEnergy, float maxEnergy)
    {
        fill.transform.localScale =new Vector3(currentEnergy/maxEnergy,1,1);
    }
    public void Setup()
    {
       
        fill.transform.localScale = new Vector3(0f, 1f, 1f);
    }
    public void StartScaling()
    {
        StartCoroutine(ScaleXOverTime(8.5f));
    }

    private IEnumerator ScaleXOverTime(float duration)
    {
        Vector3 startScale = fill.transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float newX = Mathf.Lerp(startScale.x, 0f, t);
            fill.transform.localScale = new Vector3(newX, startScale.y, startScale.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        
        fill.transform.localScale = new Vector3(0f, startScale.y, startScale.z);
    }
}
