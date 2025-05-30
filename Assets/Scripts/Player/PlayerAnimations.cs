using UnityEngine;

public class PlayerAnimations : CharacterAnimations
{
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void OnMove()
    {
        
    }
}
