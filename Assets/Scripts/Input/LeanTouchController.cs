using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeanTouchController : MonoBehaviour
{
    private IAttackStrategy attackStrategy;
    private PlayerController player;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerTap += HandleTap;
        LeanTouch.OnFingerSwipe += HandleSwipe;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= HandleTap;
        LeanTouch.OnFingerSwipe -= HandleSwipe;
    }

    private void HandleTap(LeanFinger finger)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(finger.Index))
        {
           
            return;
        }

        if (!MatchManager.Instance.CanAttack) return;

        Debug.Log("Handle Tap");

        attackStrategy = new AttackStrategy_Head();

        ExecuteAttack();
    }

    private void HandleSwipe(LeanFinger finger)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(finger.Index))
        {
           
            return;
        }

        Debug.Log("Handle Swipe");
        if (!MatchManager.Instance.CanAttack) return;
        
        Vector2 delta = finger.SwipeScreenDelta.normalized;

        if (delta.x < -0.5f)
        {
            attackStrategy = new AttackStrategy_KidneyRight();
        }
        else if (delta.x > 0.5f)
        {
            attackStrategy = new AttackStrategy_KidneyLeft();
            
        }
        else if (delta.y > 0.5f)
        {
            attackStrategy = new AttackStrategy_Stomach();
        }

        ExecuteAttack();
    }

    private void ExecuteAttack()
    {
        if(player==null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        if (attackStrategy != null && player != null)
        {
            var target = MatchManager.Instance.GetCurrentTarget(player);
            attackStrategy.ExecuteAttack(player, target);
            attackStrategy = null;
        }
    }
}
