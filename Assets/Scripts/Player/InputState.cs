using UnityEngine;

/// <summary>
/// 游戏设备的输入状态
/// </summary>
public class InputState : MonoBehaviour
{
    // 游戏设备是否有点击输入
    public bool actionButton;

    // 游戏对象在 X 轴方向的绝对速度
    [SerializeField]
    float absVelocityX = 0f;

    // 游戏对象在 Y 轴方向的绝对速度
    [SerializeField]
    float absVelocityY = 0f;

    // 游戏对象是否站在地面上
    public bool isStanding;

    // 游戏对象站立边界阈值
    [SerializeField]
    float standingThreshold = 1.0f;

    private Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        actionButton = Input.anyKeyDown;
    }

    void FixedUpdate()
    {
        absVelocityY = Mathf.Abs(rigidBody.velocity.y);

        // 如果该游戏对象在 Y 轴方向上的绝对速度小于或等于阈值，则认为它站立在地面上
        isStanding = (absVelocityY <= standingThreshold);
    }
}
