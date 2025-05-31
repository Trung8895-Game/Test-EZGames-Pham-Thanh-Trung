using UnityEngine;

public class EnemyAIController : BaseCharacter
{
    public float aiAggressiveness = 1f;
    private float timeAttack;
    protected override void Start()
    {
        base.Start();
        UIManager.Instance.RegisterCharacter(this);
    }

    private void Update()
    {
        if (fightTarget == null) return;

        
        Vector3 direction = -fightTarget.forward;

        
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

       
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            70f * Time.deltaTime
        );
        if(MatchManager.Instance.CanAttack)
        {
            if (timeAttack == 0)
            {
                timeAttack = randomTimeAttack();
            }
            else if (timeAttack < 0)
            {
                var type = randomSkillAttack();
                performAttack(type);
                timeAttack = 0;
            }
            else
            {
                timeAttack -= Time.deltaTime;
            }
        }
        
    }
    public void SetAIAggressiveness(float aiAggressiveness)
    {

        this.aiAggressiveness = aiAggressiveness;

    }
    private float randomTimeAttack()
    {
        var time = Random.Range((4f/aiAggressiveness), (6f/aiAggressiveness));
        return time;
    }
    private AttackType randomSkillAttack()
    {
        var r = Random.Range(0, 4);
        var type = (AttackType)r;
        return type;
    }
}
