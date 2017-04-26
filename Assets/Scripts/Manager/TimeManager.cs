using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 时间管理器
/// </summary>
public class TimeManager : MonoBehaviour
{
    /// <summary>
    /// 平滑 Time.timeScale 值的变化，实现游戏动画的缓动效果
    /// </summary>
    /// <param name="timeScale"></param>
    /// <param name="duration"></param>
    public void ManipulateTime(float timeScale, float duration)
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 0.1f;
        }

        StartCoroutine(FadeTo(timeScale, duration));
    }

    IEnumerator FadeTo(float value, float time)
    {
        // 注意：Time.timeScale == 0 时，Time.deltaTime == 0
        // 所以在执行以下平滑变化 Time.timeScale 的逻辑之前，必须挂起 1 帧，否则 Time.timeScale 将永远不会变化
        yield return null;

        for (float t = 0; t < 1.0f; t += (Time.deltaTime / time))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, value, t);

            if (Mathf.Abs(value - Time.timeScale) < 0.01f)
            {
                Time.timeScale = value;

                yield break;
            }
        }
    }
}
