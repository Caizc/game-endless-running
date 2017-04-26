using UnityEngine;

/// <summary>
/// 为具有刚体组件的游戏对象赋予即时速度
/// </summary>
public class InstantVelocity : MonoBehaviour
{
    [SerializeField]
    Vector2 velocity = Vector2.zero;

    Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rigidBody.velocity = velocity;
    }
}
