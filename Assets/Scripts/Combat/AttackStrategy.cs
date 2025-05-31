public interface IAttackStrategy
{
    
    void ExecuteAttack(BaseCharacter attacker, BaseCharacter target);
}

public class AttackStrategy_Head : IAttackStrategy
{
    

    public void ExecuteAttack(BaseCharacter attacker, BaseCharacter target)
    {
        attacker.performAttack(AttackType.Head);
       
        //attacker.playAttackHeadPunch();
        //target.TakeDamage(target,20);
    }
}

public class AttackStrategy_Stomach : IAttackStrategy
{
   

    public void ExecuteAttack(BaseCharacter attacker, BaseCharacter target)
    {
        attacker.performAttack(AttackType.Stomach);
        /*
        attacker.PlayAttackAnim("StomachPunch");
        target.PlayHitAnim("StomachHit");
        target.TakeDamage(target,20);
        */
    }
}

public class AttackStrategy_KidneyLeft : IAttackStrategy
{
    

    public void ExecuteAttack(BaseCharacter attacker, BaseCharacter target)
    {

        attacker.performAttack(AttackType.KidneyLeft);
       /*
        attacker.PlayAttackAnim("KidneyLeftPunch");
        target.PlayHitAnim("KidneyHit");
        target.TakeDamage(target,10);
       */
    }
}

public class AttackStrategy_KidneyRight : IAttackStrategy
{
   

    public void ExecuteAttack(BaseCharacter attacker, BaseCharacter target)
    {
        attacker.performAttack(AttackType.KidneyRight);
        /*
        attacker.PlayAttackAnim("KidneyRightPunch");
        target.PlayHitAnim("KidneyHit");
        target.TakeDamage(target,10);
        */
    }
}
