using System.Collections;
using UnityEngine;

/// <summary>
/// 游戏障碍物对象生成器
/// </summary>
public class Spawner : MonoBehaviour
{
    // 障碍物 Prefab 数组
    [SerializeField]
    GameObject[] obstacles;

    // 生成新对象的延迟时间范围
    [SerializeField]
    Vector2 delayRange = new Vector2(3.0f, 6.0f);

    // 生成器的激活开关
    [SerializeField]
    bool active = true;

    void OnEnable()
    {
        // 在生成器可用时，开始执行生成游戏障碍物对象的协程
        StartCoroutine(ObstacleSpawner());
    }

    /// <summary>
    /// 生成游戏障碍物对象的协程
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator ObstacleSpawner()
    {
        yield return null;

        while (active)
        {
            Transform spawnPoint = transform;

            int index = Random.Range(0, obstacles.Length);

            // 随机初始化障碍物 Prefab 数组中的某一个实例
            GameObjectUtil.Instantiate(obstacles[index], spawnPoint.position);

            yield return StartCoroutine(SpawnDelay());

            // yield return SpawnDelay();
        }
    }

    /// <summary>
    /// 执行随机延时的协程
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator SpawnDelay()
    {
        // 当 Time.timeScale == 0 时，Time.deltaTime 为 0，WaitForSeconds() 函数同时会停止执行
        // 所以必须利用 Time.realtimeSinceStartup 自行实现延时功能

        float delay = Random.Range(delayRange.x, delayRange.y);

        float start = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < (start + delay))
        {
            yield return null;
        }
    }

    // WaitForSeconds SpawnDelay()
    // {
    //     float delay = Random.Range(delayRange.x, delayRange.y);
    //     return new WaitForSeconds(delay);
    // }
}
