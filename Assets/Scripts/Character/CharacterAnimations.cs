using UnityEngine;

public abstract class CharacterAnimations : MonoBehaviour
{
    protected Animator animator;

    protected abstract void OnMove();

    public virtual void SetAnimController(RuntimeAnimatorController controller)
    {
        animator.runtimeAnimatorController = controller;
    }
}
