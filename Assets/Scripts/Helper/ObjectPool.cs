using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对象池
/// </summary>
public class ObjectPool : MonoBehaviour
{
    // 该对象池存储的可复用对象
    public RecycleGameObject prefab;

    // 存放可复用对象实例的列表容器
    List<RecycleGameObject> instances = new List<RecycleGameObject>();

    /// <summary>
    /// 从列表容器中获取下一个可复用对象实例，并且重置它
    /// </summary>
    public RecycleGameObject NextObject(Vector3 position)
    {
        RecycleGameObject instance = null;

        foreach (RecycleGameObject recycleGameObject in instances)
        {
            if (recycleGameObject.gameObject.activeSelf != true)
            {
                instance = recycleGameObject;
                instance.transform.position = position;
            }
        }

        if (null == instance)
        {
            instance = CreateInstance(position);
        }

        instance.Restart();

        return instance;
    }

    /// <summary>
    /// 创建新的可复用对象实例，并把它加入到列表容器中
    /// </summary>
    private RecycleGameObject CreateInstance(Vector3 position)
    {
        RecycleGameObject clone = GameObject.Instantiate(prefab) as RecycleGameObject;

        clone.transform.position = position;
        clone.transform.parent = transform;

        instances.Add(clone);

        return clone;
    }
}
