using UnityEngine;

public static class AnimatorHash
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Walk = Animator.StringToHash("Walk");
    public static readonly int Run = Animator.StringToHash("Run");
    public static readonly int Jump = Animator.StringToHash("Jump");
    public static readonly int Attack = Animator.StringToHash("Attack");
    public static readonly int Die = Animator.StringToHash("Die");


    public static readonly int isDone = Animator.StringToHash("isDone");
}