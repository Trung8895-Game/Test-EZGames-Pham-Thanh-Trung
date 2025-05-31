using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform healthBarParent;

    [SerializeField] private GameObject energyBarPrefab;
    [SerializeField] private Transform energyBarParent;

    [SerializeField] private GameObject enemyImagePrefab;
    [SerializeField] private GameObject playerImagePrefab;
    [SerializeField] private GameObject subPlayerImagePrefab;
    [SerializeField] private Transform imageParent;

    [SerializeField] private VictoryLosePanel victoryLosePanel;

    private Dictionary<BaseCharacter, HealthBarUI> healthBars = new();
    private Dictionary<BaseCharacter, EnergyBar> energyBars = new();


    public static UIManager Instance = null;

    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
        else if(Instance!=this)
        {
            Destroy(gameObject);
        }
    }

    public void ShowCountdown(Action onComplete)
    {
        StartCoroutine(CountdownCoroutine(onComplete));
    }

    public void ShowVictory()
    {
        victoryLosePanel.ShowVictory();
        Debug.Log("Victory!");
    }

    public void ShowDefeat()
    {
        victoryLosePanel.ShowLose();
        Debug.Log("Defeat...");
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

    public void RegisterCharacter(BaseCharacter character)
    {
        var hb = Instantiate(healthBarPrefab, healthBarParent);
        var ui = hb.GetComponent<HealthBarUI>();
        ui.Setup(); 
        healthBars.Add(character, ui);

        var eb = Instantiate(energyBarPrefab, energyBarParent);
        var energyBarUI = eb.GetComponent<EnergyBar>();

        switch(character.characterType)
        {
            case CharacterType.Player:
               Instantiate(playerImagePrefab, imageParent);
                break;
            case CharacterType.SubPlayer:
                Instantiate(subPlayerImagePrefab, imageParent);
                break;
            case CharacterType.Enemy:
                Instantiate(enemyImagePrefab, imageParent);
                break;
            default:
                break;
        }

        energyBarUI.Setup(); 
        energyBars.Add(character, energyBarUI);
    }

    public void UpdateHealth(BaseCharacter character, float value)
    {
        if (healthBars.ContainsKey(character))
            healthBars[character].SetHealth(value,character.maxHealth);
    }

    public void UpdateEnergy(BaseCharacter character, int value)
    {
        if (energyBars.ContainsKey(character))
            energyBars[character].UpdateUI(value,character.MaxEnergy);
    }
    public void ScalingEnergyToZero(BaseCharacter character)
    {
        if (energyBars.ContainsKey(character))
            energyBars[character].StartScaling();
    }
    public void HideUI(BaseCharacter character)
    {
        if (healthBars.TryGetValue(character, out var bar))
            bar.gameObject.SetActive(false);
    }
    
}
