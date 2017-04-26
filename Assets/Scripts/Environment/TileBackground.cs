using UnityEngine;

/// <summary>
/// 根据实际屏幕尺寸动态填充地面对象的背景纹理
/// </summary>
public class TileBackground : MonoBehaviour
{
    // floor 纹理图片的大小
    [SerializeField]
    int textureSize = 128;

    // Unity 中每个单元的像素大小
    [SerializeField]
    float pixelToUnit = 100f;

    void Start()
    {
        // 铺满地面所需的纹理图片数目，等于实际屏幕宽度除以缩放后的纹理图片大小，同时为了保证地面的长度至少超过屏幕宽度，需要向上取整
        float count = Mathf.Ceil(Screen.width / (textureSize * PixelPerfectCamera.scale));

        // 地面对象的尺寸为纹理数目乘以纹理大小，同时需要除以每单元像素，以转换为 Unity 统一的单位尺寸
        transform.localScale = new Vector3((count * textureSize) / pixelToUnit, 1, 1);

        // 设置填充地面对象所需的纹理数目
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(count, 1);
    }
}
