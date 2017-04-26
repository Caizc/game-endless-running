using UnityEngine;

/// <summary>
/// 让游戏对象的纹理循环播放
/// </summary>
public class AnimatedTexture : MonoBehaviour
{
    // 循环播放的速度
    [SerializeField]
    Vector2 speed = Vector2.zero;

    // 累加的偏移量
    private Vector2 offset = Vector2.zero;

    // 游戏对象的 Material 组件
    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        offset = material.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        // 不断累加纹理的偏移量，以实现循环播放效果
        offset += speed * Time.deltaTime;

        material.SetTextureOffset("_MainTex", offset);
    }
}
