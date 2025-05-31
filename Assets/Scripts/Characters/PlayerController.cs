using UnityEngine;
using Lean.Touch;


public class PlayerController : BaseCharacter
{

    

    public Transform head;

    [Header("Lean Touch Settings")]
    public float swipeThreshold = 0.2f;
   

    

    protected override void Awake()
    {
        base.Awake();
        
        
        
    }
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
    }
    
}
