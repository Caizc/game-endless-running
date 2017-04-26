using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player 动画控制器管理类
/// </summary>
public class PlayerAnimationManager : MonoBehaviour
{
    private Animator animator;

    private InputState inputState;

    void Awake()
    {
        animator = GetComponent<Animator>();
        inputState = GetComponent<InputState>();
    }

    void Update()
    {
        // 根据 Player 的状态设置动画状态
        animator.SetBool("Running", inputState.isStanding);
    }
}
