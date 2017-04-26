using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏对象管理器
/// </summary>
public class GameObjectUtil
{
    // 字典容器，根据可复用对象引用获取相应的可复用对象池
    private static Dictionary<RecycleGameObject, ObjectPool> pools = new Dictionary<RecycleGameObject, ObjectPool>();

    /// <summary>
    /// 实例化指定的 Prefab，并放置到指定位置上
    /// </summary>
    public static GameObject Instantiate(GameObject prefab, Vector3 position)
    {
        GameObject instance = null;

        RecycleGameObject recycleGameObject = prefab.GetComponent<RecycleGameObject>();
        if (null != recycleGameObject)
        {
            // 如果该 Prefab 为可复用对象，则从相应的对象池中获取对象实例
            ObjectPool pool = GetObjectPool(recycleGameObject);
            instance = pool.NextObject(position).gameObject;
        }
        else
        {
            // 如果该 Prefab 不是可复用对象，则创建该 Prefab 的实例
            instance = GameObject.Instantiate(prefab);
            instance.transform.position = position;
        }

        return instance;
    }

    /// <summary>
    /// 销毁指定的 GameObject
    /// </summary>
    public static void Destroy(GameObject gameObject)
    {
        RecycleGameObject recycleGameObject = gameObject.GetComponent<RecycleGameObject>();
        if (null != recycleGameObject)
        {
            recycleGameObject.Shutdown();
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }

    /// <summary>
    /// 根据可复用对象引用获取相应的可复用对象池
    /// </summary>
    private static ObjectPool GetObjectPool(RecycleGameObject reference)
    {
        ObjectPool pool = null;

        if (pools.ContainsKey(reference))
        {
            pool = pools[reference];
        }
        else
        {
            // 如果该可复用对象未有相应的对象池，创建之
            GameObject poolContainer = new GameObject(reference.gameObject.name + "ObjectPool");

            pool = poolContainer.AddComponent<ObjectPool>();
            pool.prefab = reference;

            // 把新的对象池添加到字典容器中
            pools.Add(reference, pool);
        }

        return pool;
    }
}
