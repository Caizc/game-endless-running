using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : MonoBehaviour
{
    // 地面对象
    [SerializeField]
    GameObject floor;

    // 障碍物生成器
    [SerializeField]
    Spawner spawner;

    // Player Prefab
    [SerializeField]
    GameObject playerPrefab;

    // 时间管理器
    [SerializeField]
    TimeManager timeManager;

    // 提示信息 Text
    [SerializeField]
    Text infoText;

    // 得分信息 Text
    [SerializeField]
    Text scoreText;

    // 背景音乐 AudioSource 对象
    [SerializeField]
    AudioSource bgmAudioSource;

    // 声音特效 AudioSource 对象
    [SerializeField]
    AudioSource sfxAudioSource;

    // 游戏中的声音特效片段
    [SerializeField]
    AudioClip goClip;
    [SerializeField]
    AudioClip gameOverClip;
    [SerializeField]
    AudioClip newRecordClip;

    // Player 对象
    private GameObject player;

    // 游戏是否已开始的标识
    private bool gameStarted = false;

    // 提示信息累计闪烁次数
    private float blinkTime = 0f;
    // 提示信息是否正在闪烁标识
    private bool blink;

    // 玩家已经通关的时间
    private float timeElapsed = 0f;
    // 玩家最佳的通关时间
    private float bestTime = 0f;
    // 玩家本次游玩是否打破了最佳通关时间
    private bool beatBestTime = false;

    void Start()
    {
        // 根据屏幕尺寸自动布局地面的高度
        FloorAutoLayoutVoid();

        // 提示玩家“按任意键开始游戏”
        infoText.text = "PRESS ANY BUTTON TO START";

        // 暂停游戏直到玩家按下任意键开始游戏
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (!gameStarted && Time.timeScale == 0f)
        {
            if (Input.anyKeyDown)
            {
                // 如果游戏未开始且处以暂停状态时，玩家按下了任意键，则缓慢恢复游戏动画到正常播放速度
                timeManager.ManipulateTime(1.0f, 1.0f);

                // 重置游戏状态以开始新的游戏
                ResetGame();
            }
        }

        // 闪烁界面上的提示信息引起玩家的注意
        BlinkText();

        // 显示得分信息
        ScoreDisplay();
    }

    /// <summary>
    /// 重置游戏，即重新开始游戏
    /// </summary>
    void ResetGame()
    {
        // 激活障碍物生成器
        spawner.gameObject.SetActive(true);

        // 在屏幕中央正上方初始化 Player
        player = GameObjectUtil.Instantiate(playerPrefab, new Vector3(0, Screen.height / PixelPerfectCamera.pixelToUnit / 2, 0));

        // 获取 Player 的 DestoryOffScreen 脚本的引用
        DestoryOffScreen playerDestroyScript = player.GetComponent<DestoryOffScreen>();
        // 注册 OnPlayerKilled() 方法到 DestoryOffScreen 的 DestroyCallback 回调中，当 Player 掉出屏幕外部而被销毁时，OnPlayerKilled() 将会被调用
        playerDestroyScript.DestroyCallback += OnPlayerKilled;

        // 标识游戏已经开始
        gameStarted = true;

        // 隐藏提示信息
        infoText.canvasRenderer.SetAlpha(0);

        // 重置已通关时间
        timeElapsed = 0f;

        // 从玩家配置中读取历史通关最佳时间
        bestTime = PlayerPrefs.GetFloat("BestTime");
        beatBestTime = false;

        // 开始生成游戏中的障碍物和敌人之前的处理
        StartCoroutine(StartSpawnEnemy());
    }

    IEnumerator StartSpawnEnemy()
    {
        // 播放 go 声音特效片段
        sfxAudioSource.clip = goClip;
        sfxAudioSource.Play();

        // 稍等片刻
        yield return new WaitForSeconds(1.0f);

        // 然后开始循环播放游戏背景音乐
        bgmAudioSource.Play();
    }

    /// <summary>
    /// Player 挑战失败事件处理
    /// </summary>
    void OnPlayerKilled()
    {
        // 停止障碍物生成器
        spawner.gameObject.SetActive(false);

        // 获取 Player 的 DestoryOffScreen 脚本的引用
        DestoryOffScreen playerDestroyScript = player.GetComponent<DestoryOffScreen>();
        // 从 DestoryOffScreen 的 DestroyCallback 回调中注销 OnPlayerKilled()
        playerDestroyScript.DestroyCallback -= OnPlayerKilled;

        // 停止 Player 的一切运动
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // 缓慢停止游戏动画
        timeManager.ManipulateTime(0, 8.5f);

        // 标识游戏已经结束
        gameStarted = false;

        // 提示玩家“按任意键重新开始游戏”
        infoText.text = "PRESS ANY BUTTON TO RESTART";

        // 停止游戏背景音乐
        bgmAudioSource.Stop();

        // 播放游戏结束的声音特效片段
        sfxAudioSource.clip = gameOverClip;
        sfxAudioSource.Play();

        // 如果本局游戏的通关时间优于历史最佳通关时间，则更新保存在玩家配置中的信息
        if (timeElapsed > bestTime)
        {
            bestTime = timeElapsed;

            PlayerPrefs.SetFloat("BestTime", bestTime);

            beatBestTime = true;

            // 播放创造新记录的声音特效片段
            sfxAudioSource.clip = newRecordClip;
            sfxAudioSource.Play();
        }
    }

    /// <summary>
    /// 根据屏幕尺寸自动布局地面的高度
    /// </summary>
    private void FloorAutoLayoutVoid()
    {
        float floorHeight = floor.transform.localScale.y;

        Vector3 position = floor.transform.position;
        position.x = 0f;
        position.y = -(Screen.height / PixelPerfectCamera.pixelToUnit / 2) + (floorHeight / 2);

        floor.transform.position = position;
    }

    /// <summary>
    /// 闪烁界面上的提示信息
    /// </summary>
    private void BlinkText()
    {
        if (!gameStarted)
        {
            blinkTime++;

            // 对游戏的 FPS（每秒帧数）求余，实现大约每秒闪烁一次的效果
            if (blinkTime % 74 == 0)
            {
                blink = !blink;
            }

            // 通过设置文本的 Alpha 属性来实现文本的显示和隐藏效果
            infoText.canvasRenderer.SetAlpha(blink ? 1 : 0);
        }
    }

    /// <summary>
    /// 得分信息显示
    /// </summary>
    private void ScoreDisplay()
    {
        if (!gameStarted)
        {
            string textColor = beatBestTime ? "red" : "white";

            // 通过在 string 中使用 <color="red">some text</color> 来实现文本标红效果
            scoreText.text = "Time  " + FormatTime(timeElapsed) + "\n<color='" + textColor + "'>Best  " + FormatTime(bestTime) + "</color>";
        }
        else
        {
            // 如果游戏正在进行中，则只实时刷新当前已通关时间

            timeElapsed += Time.deltaTime;
            scoreText.text = "Time  " + FormatTime(timeElapsed);
        }
    }

    /// <summary>
    /// 格式化时间值
    /// </summary>
    /// <param name="value">float 类型的时间值</param>
    /// <returns> 01:23 格式的时间字符串</returns>
    private string FormatTime(float value)
    {
        TimeSpan t = TimeSpan.FromSeconds(value);

        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
