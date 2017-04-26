using UnityEngine;

/// <summary>
/// 游戏中的障碍物
/// </summary>
public class Obstacle : MonoBehaviour, IRecycle
{
    // Sprite 资源数组
    public Sprite[] sprites;

    public void Restart()
    {
        // 获取 Sprite 对象的渲染器
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        // 赋予渲染器随机的 Sprite 皮肤
        renderer.sprite = sprites[Random.Range(0, sprites.Length)];

        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        // 调整游戏障碍物的碰撞盒尺寸为游戏对象的渲染尺寸，提高碰撞盒的精准度
        collider.size = renderer.bounds.size;

        // 修正碰撞盒的位置
        collider.offset = new Vector2(collider.offset.x, collider.size.y / 2);
    }

    public void Shutdown()
    {
        // nothing to do.
    }
}
