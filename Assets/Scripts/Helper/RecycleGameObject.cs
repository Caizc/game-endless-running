using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 可复用对象
/// </summary>
public class RecycleGameObject : MonoBehaviour
{
    // 保存可复用组件的列表集合
    private List<IRecycle> recycleComponents;

    void Awake()
    {
        recycleComponents = new List<IRecycle>();

        MonoBehaviour[] components = GetComponents<MonoBehaviour>();

        // 遍历 GameObject 中的所有组件，如果该组件实现了 IRecycle 接口，则保存该组件引用到可复用组件列表
        foreach (MonoBehaviour component in components)
        {
            if (component is IRecycle)
            {
                recycleComponents.Add(component as IRecycle);
            }
        }
    }

    /// <summary>
    /// 重置该可复用对象
    /// </summary>
    public void Restart()
    {
        gameObject.SetActive(true);

        // 同时重置 GameObject 中所有实现了 IRecycle 接口的组件
        foreach (IRecycle component in recycleComponents)
        {
            component.Restart();
        }
    }

    /// <summary>
    /// 停用该可复用对象
    /// </summary>
    public void Shutdown()
    {
        gameObject.SetActive(false);

        // 同时停用 GameObject 中所有实现了 IRecycle 接口的组件
        foreach (IRecycle component in recycleComponents)
        {
            component.Shutdown();
        }
    }
}


/// <summary>
/// 可复用对象接口
/// </summary>
public interface IRecycle
{
    /// <summary>
    /// 重置该可复用对象
    /// </summary>
    void Restart();

    /// <summary>
    /// 停用该可复用对象
    /// </summary>
    void Shutdown();
}