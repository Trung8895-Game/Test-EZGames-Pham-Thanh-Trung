using System.Collections;
using UnityEngine;

public abstract class BaseCharacter : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Transform fightPosition;

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual IEnumerator WalkToPosition()
    {
        animator.SetTrigger("Walk");
        while (Vector3.Distance(transform.position, fightPosition.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, fightPosition.position, Time.deltaTime * 2f);
            yield return null;
        }
        animator.SetTrigger("Idle");
    }
}
