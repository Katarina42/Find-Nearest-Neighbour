using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    public static Pooler Instance;
    private Queue<GameObject> pool = new Queue<GameObject>();
    public int defaultPoolSize;
    public GameObject prefab;

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < defaultPoolSize; i++)
            Instantiate();
    }

    private GameObject Instantiate()
    {
        GameObject obj = Instantiate(prefab);
        pool.Enqueue(obj);
        obj.SetActive(false);
        return obj;
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
            Instantiate();

        var obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject gameObject)
    {
        gameObject.SetActive(false);
        pool.Enqueue(gameObject);
    }
}
