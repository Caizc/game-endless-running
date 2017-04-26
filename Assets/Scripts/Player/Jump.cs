using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player 的跳跃控制
/// </summary>
public class Jump : MonoBehaviour
{
    // 跳跃时 Y 轴正方向上的初速度
    [SerializeField]
    float jumpSpeed = 20.0f;

    // 跳跃时 X 轴正方向上的初速度
    [SerializeField]
    float forwardSpeed = 6.0f;

    [SerializeField]
    AudioSource audioSource;

    private Rigidbody2D rigidBody;
    private InputState inputState;

    // Player 向前跳跃的界限，防止 Player 向前移动超过屏幕范围
    private float forwardBounds;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        inputState = GetComponent<InputState>();

        // Player 向前移动的界限为屏幕中心点稍微往右的地方
        forwardBounds = Screen.width / PixelPerfectCamera.pixelToUnit * 0.1f;
    }

    void Update()
    {
        // 如果 Player 站立在地面上，而且接收到了动作指令，则执行跳跃动作
        if (inputState.isStanding && inputState.actionButton)
        {
            // 播放跳跃的声音
            audioSource.Play();

            // rigidBody.velocity = new Vector2(((transform.position.x < 0) ? forwardSpeed : 0f), jumpSpeed);

            // 向前跳跃
            StartCoroutine(JumpAndForward());
        }
    }

    IEnumerator JumpAndForward()
    {
        // 首先赋予 Player Y 轴正方向上的初速度以向上跳跃
        rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);

        // 延迟两帧以让 Player 上升到足够高度
        yield return null;
        yield return null;

        // 然后赋予 Player X 轴正方向上的初速度以越过障碍物
        rigidBody.velocity = new Vector2(((transform.position.x < forwardBounds) ? forwardSpeed : 0f), rigidBody.velocity.y);
    }
}
