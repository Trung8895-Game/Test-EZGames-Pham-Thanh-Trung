using System;
using System.Collections;
using UnityEngine;

public enum CharacterType { Player, Enemy, SubPlayer }
public enum AttackType
{
    KidneyLeft=0,
    KidneyRight=1,
    Stomach=2,
    Head=3
}

public abstract class BaseCharacter : MonoBehaviour
{
    public ParticleSystem KnockOutVFX;
    public ParticleSystem HeadHitVFX;
    public ParticleSystem KidneyLeftHitVFX;
    public ParticleSystem KidneyRightHitVFX;
    public ParticleSystem StomachHitVFX;

    public ParticleSystem RightHandFireVFX;
    public ParticleSystem LeftHandFireVFX;

    public ParticleSystem HexagonVFX;
    protected bool isDead = false;

    protected bool canAttack = true;
    public bool isInCombo = false;
    private bool isAttacked = false;

    public BaseCharacter target;

    public Transform fightPosition;
    public Transform fightTarget;
    [Header("Core Settings")]
    public Animator animator;

    public CharacterType characterType;
    public bool isAI = false;



    [Header("Damage Settings")]
    public float kidneyDamage = 10f;
    public float stomachDamage = 15f;
    public float headDamage = 20f;
    public float comboDamage = 40f;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;

    public float MaxEnergy = 100f;
    public float CurrentEnergy;
    public float attackRate = 1f;
    

    public bool IsEnergyFull => CurrentEnergy >= MaxEnergy;
    public bool IsKnockedOut => currentHealth <= 0;

    public event Action<float, float> OnEnergyChanged; // current, max

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        CurrentEnergy = 0f;
        
    }

    public virtual void Init(CharacterType characterType, bool isAI)
    {
        this.characterType = characterType;
        this.isAI = isAI;
    }

    public virtual void SetStats(float health, float attackRate)
    {
        this.maxHealth = Mathf.RoundToInt(health);
        this.currentHealth = this.maxHealth;
        this.attackRate = attackRate;
        
        this.CurrentEnergy = 0f;
    }
    
    public virtual IEnumerator WalkToPosition()
    {
        //animator.SetTrigger("Walk");

        while (Vector3.Distance(transform.position, fightPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, fightPosition.position, Time.deltaTime * 0.6f);
            yield return null;
        }

        transform.position = fightPosition.position;
        animator.SetTrigger("Idle");
    }
    
    public void PlayAttackAnim(string animName)
    {
        animator.SetTrigger(animName);
    }
    
    public void PlayHitAnim(string hitName)
    {
        animator.SetTrigger(hitName);
    }

    public virtual void TakeDamage(BaseCharacter character, float amount)
    {
        if (IsKnockedOut) return;

        currentHealth -= amount;
        UIManager.Instance.UpdateHealth(character, currentHealth);

        if (currentHealth <= 0)
        {
            Knockout();
        }
    }

    protected virtual void Knockout()
    {
        isDead = true;
        KnockOutVFX.gameObject.SetActive(true);
        animator.SetTrigger("KnockOut");
        //this.enabled = false;

        
        StartCoroutine(DisappearAfterSeconds(2f));
    }

    protected IEnumerator DisappearAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
        GameManager.Instance.ReportKnockout(this);
        if(characterType == CharacterType.SubPlayer)
        {
            foreach(var subPlayer in MatchManager.Instance.allSubPlayers)
            {
                if(subPlayer.gameObject.activeSelf)
                {
                    target.target = subPlayer;
                }
            }
        }
        else if(characterType == CharacterType.Enemy)
        {
            foreach (var enemy in MatchManager.Instance.allEnemies)
            {
                if (enemy.gameObject.activeSelf)
                {
                    target.target = enemy;
                }
            }
        }
    }

    public void GainEnergy(float amount)
    {
        if (IsKnockedOut) return;

        CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy + amount);
        OnEnergyChanged?.Invoke(CurrentEnergy, MaxEnergy);
        UIManager.Instance.UpdateEnergy(this, (int)CurrentEnergy);
    }

    public void ResetEnergy()
    {
        CurrentEnergy = 0;
        OnEnergyChanged?.Invoke(CurrentEnergy, MaxEnergy);
        UIManager.Instance.ScalingEnergyToZero(this);
    }
    public void performAttack(AttackType type)
    {
        if (!canAttack || isInCombo || target.isInCombo) return; 
        
        StartCoroutine(PerformAttack(type));
    }

    public IEnumerator PerformAttack(AttackType type)
    {
        if(target.isDead) yield break;

        if (isAttacked)
        {
            isAttacked = false;
            canAttack = true;
            yield break;

        }

        canAttack = false;

        
        string trigger = type.ToString()+"Punch";
        PlayAttackAnim(trigger);
        
        

            switch (type)
        {
            case AttackType.Head:
                yield return new WaitForSeconds(0.6f);
                if (isAttacked)
                {
                    isAttacked = false;
                    canAttack = true;
                    yield break;

                }
                if (target != null)
                {

                    target.isAttacked = true;
                    target.HeadHitVFX.gameObject.SetActive(true);
                    target.receiveHit(type);
                    if(target.isAI)
                    target.target = this;
                    GainEnergy(10f);
                }

                yield return new WaitForSeconds(1.350f);
                target.HeadHitVFX.gameObject.SetActive(false);
                break;
            case AttackType.KidneyLeft:
                yield return new WaitForSeconds(0.5f);
                if (isAttacked)
                {
                    isAttacked = false;
                    canAttack = true;
                    yield break;

                }
                if (target != null)
                {

                    target.isAttacked = true;
                    target.KidneyLeftHitVFX.gameObject.SetActive(true);
                    target.receiveHit(type);
                    if (target.isAI)
                        target.target = this;
                    GainEnergy(10f);
                }

                yield return new WaitForSeconds(1.483f);
                target.KidneyLeftHitVFX.gameObject.SetActive(false);
                break;
            case AttackType.KidneyRight:
                yield return new WaitForSeconds(1.1f);
                if (isAttacked)
                {
                    isAttacked = false;
                    canAttack = true;
                    yield break;

                }
                if (target != null)
                {

                    target.isAttacked = true;
                    target.KidneyRightHitVFX.gameObject.SetActive(true);
                    target.receiveHit(type);
                    if (target.isAI)
                        target.target = this;
                    GainEnergy(10f);
                }

                yield return new WaitForSeconds(1.1f);
                target.KidneyRightHitVFX.gameObject.SetActive(false);
                break;
            case AttackType.Stomach:
                yield return new WaitForSeconds(0.75f);
                if (isAttacked)
                {
                    isAttacked = false;
                    canAttack = true;
                    yield break;

                }
                if (target != null)
                {

                    target.isAttacked = true;
                    target.StomachHitVFX.gameObject.SetActive(true);
                    target.receiveHit(type);
                    if (target.isAI)
                        target.target = this;
                    GainEnergy(10f);
                }

                yield return new WaitForSeconds(1.45f);
                target.StomachHitVFX.gameObject.SetActive(false);
                break;
            default:
                break;
        }

       
        canAttack = true;
        
        
        if (IsEnergyFull)
        {
            yield return new WaitForSeconds(0.5f);
            HexagonVFX.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            RightHandFireVFX.gameObject.SetActive(true);
            LeftHandFireVFX.gameObject.SetActive(true);
            
            performCombo();
        }
    }
    public void performCombo()
    {
        if (!canAttack || isInCombo || !IsEnergyFull || target.isInCombo) return;
        
        StartCoroutine(PerformCombo());
    }
    public IEnumerator PerformCombo()
    {
        if (!canAttack || isInCombo) yield break;

        isInCombo = true;
        canAttack = false;

        ResetEnergy();
        StartCoroutine(PerformAttack(AttackType.KidneyLeft));
        yield return new WaitForSeconds(1.6f);

        StartCoroutine(PerformAttack(AttackType.KidneyRight));
        yield return new WaitForSeconds(2.2f);

        StartCoroutine(PerformAttack(AttackType.Head));
        yield return new WaitForSeconds(2.1f);

        StartCoroutine(PerformAttack(AttackType.Stomach));
        yield return new WaitForSeconds(2f);

        HexagonVFX.gameObject.SetActive(false);
        RightHandFireVFX.gameObject.SetActive(false);
        LeftHandFireVFX.gameObject.SetActive(false);

        ResetCombo();
      
    }

    protected void ResetCombo()
    {
        isInCombo = false;
        canAttack = true;
    }
    public void receiveHit(AttackType type)
    {
        StartCoroutine(ReceiveHit(type));
    }
    public IEnumerator ReceiveHit(AttackType type)
    {
        if (isDead) yield break; ;
        Debug.Log("ReceiveHit");
        switch (type)
        {
            case AttackType.KidneyLeft:
            case AttackType.KidneyRight:
                TakeDamage(this, kidneyDamage*attackRate);
                if (currentHealth<= 0f)
                {
                    Knockout();
                }
                else
                {
                    PlayHitAnim("KidneyHit");
                    yield return new WaitForSeconds(0.933f);
                }
                
                break;

            case AttackType.Stomach:
                TakeDamage(this, stomachDamage*attackRate);
                if (currentHealth <= 0f)
                {
                    Knockout();
                }
                else
                {
                    PlayHitAnim("StomachHit");
                    yield return new WaitForSeconds(1.183f);
                }
                
                break;

            case AttackType.Head:
                TakeDamage(this, headDamage*attackRate);
                if (currentHealth <=0f)
                {
                    Knockout();
                }
                else
                {
                    PlayHitAnim("HeadHit");
                    yield return new WaitForSeconds(1.433f);
                }
                
                break;
        }
        isAttacked = false;
       
    }

    
    protected void Disappear()
    {
        gameObject.SetActive(false);
    }
}
