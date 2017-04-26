using UnityEngine;

/// <summary>
/// 销毁离开屏幕范围的游戏对象
/// </summary>
public class DestoryOffScreen : MonoBehaviour
{
    // 游戏对象离开屏幕范围后的缓冲距离，超过该值时销毁对象
    [SerializeField]
    float offset = 1.0f;

    // 游戏对象是否已经离开屏幕范围的标识
    bool offScreen = false;

    // 游戏对象的活动半径，超过该半径区域的活动对象将被销毁
    float offScreenX = 0f;

    Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // 游戏对象的活动半径，为实际屏幕宽度的一半加上缓冲距离
        offScreenX = Screen.width / PixelPerfectCamera.pixelToUnit / 2 + offset;
    }

    void Update()
    {
        float positionX = transform.position.x;
        float directionX = rigidBody.velocity.x;

        if (Mathf.Abs(positionX) > offScreenX)
        {
            if (directionX < 0 && positionX < -offScreenX)
            {
                offScreen = true;
            }
            else if (directionX > 0 && positionX > offScreenX)
            {
                offScreen = true;
            }
        }
        else
        {
            offScreen = false;
        }

        if (offScreen)
        {
            OnOutOfBounds();
        }
    }

    /// <summary>
    /// 处理离开游戏可活动区域（屏幕范围）的对象
    /// </summary>
    void OnOutOfBounds()
    {
        // 重置该游戏对象是否离开屏幕范围的标识为 false
        offScreen = false;
		
        GameObjectUtil.Destroy(gameObject);

        // 调用监听 OnDestroy() 的回调方法
        if(null != DestroyCallback)
        {
            DestroyCallback();
        }
    }

    /// <summary>
    /// 创建 OnDestroy() 委托
    /// </summary>
    public delegate void OnDestroy();

    /// <summary>
    /// 创建事件回调
    /// </summary>
    public event OnDestroy DestroyCallback;
}
