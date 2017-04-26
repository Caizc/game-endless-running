using UnityEngine;

/// <summary>
/// 正交摄像机的自动对焦，以适配不同的屏幕分辨率
/// </summary>
public class PixelPerfectCamera : MonoBehaviour
{
    // Unity 中每个单元的像素大小
    public static float pixelToUnit = 100f;

    // 实际分辨率与默认分辨率的缩放比例
    public static float scale = 1f;

    // 正交摄像机的 Size
    public static float orthographicSize = 0f;

    // 默认分辨率为 1136*640（16:9）
    [SerializeField]
    Vector2 nativeResolution = new Vector2(1136, 640);

    void Awake()
    {
        Camera camera = GetComponent<Camera>();

        if (camera.orthographic)
        {
            // 缩放比例为实际的屏幕高度比上默认的屏幕高度
            scale = Screen.height / nativeResolution.y;

            // 每单元像素（Pixel per Unit）也需要同比例缩放
            pixelToUnit *= scale;

            // 正交摄像机的 Size 为实际屏幕高度的一半再除以每单元像素，转换为 Unity 统一的单位尺寸
            camera.orthographicSize = (Screen.height / 2) / pixelToUnit;

            orthographicSize = camera.orthographicSize;
        }
    }
}
