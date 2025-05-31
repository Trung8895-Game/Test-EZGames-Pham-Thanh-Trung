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
    [SerializeField] private Transform enemyHealthBarParent;
    [SerializeField] private Transform playerHealthBarParent;

    [SerializeField] private GameObject energyBarPrefab;
    [SerializeField] private Transform enemyEnergyBarParent;
    [SerializeField] private Transform playerEnergyBarParent;

    [SerializeField] private GameObject enemyImagePrefab;
    [SerializeField] private GameObject playerImagePrefab;
    [SerializeField] private GameObject subPlayerImagePrefab;
    [SerializeField] private Transform enemyImageParent;
    [SerializeField] private Transform playerImageParent;

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
        
        

        switch(character.characterType)
        {
            case CharacterType.Player:
                var phb = Instantiate(healthBarPrefab, playerHealthBarParent);
                var pui = phb.GetComponent<HealthBarUI>();
                pui.Setup();
                healthBars.Add(character, pui);

                var peb = Instantiate(energyBarPrefab, playerEnergyBarParent);
                var playerEnergyBarUI = peb.GetComponent<EnergyBar>();
                Instantiate(playerImagePrefab, playerImageParent);
                playerEnergyBarUI.Setup();
                energyBars.Add(character, playerEnergyBarUI);
                break;
            case CharacterType.SubPlayer:
                var sphb = Instantiate(healthBarPrefab, playerHealthBarParent);
                var spui = sphb.GetComponent<HealthBarUI>();
                spui.Setup();
                healthBars.Add(character, spui);

                var speb = Instantiate(energyBarPrefab, playerEnergyBarParent);
                var subPlayerEnergyBarUI = speb.GetComponent<EnergyBar>();
                Instantiate(subPlayerImagePrefab, playerImageParent);
                subPlayerEnergyBarUI.Setup();
                energyBars.Add(character, subPlayerEnergyBarUI);
                break;
            case CharacterType.Enemy:
                var hb = Instantiate(healthBarPrefab, enemyHealthBarParent);
                var ui = hb.GetComponent<HealthBarUI>();
                ui.Setup();
                healthBars.Add(character, ui);

                var eb = Instantiate(energyBarPrefab, enemyEnergyBarParent);
                var energyBarUI = eb.GetComponent<EnergyBar>();
                Instantiate(enemyImagePrefab, enemyImageParent);
                energyBarUI.Setup();
                energyBars.Add(character, energyBarUI);
                break;
            default:
                break;
        }

        
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
