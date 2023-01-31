using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
   

    public Queue<GameObject> pool;

    private void Start()
    {
        pool = new Queue<GameObject>();
    }
    public void Attach(GameObject obj)
    {
        pool.Enqueue(obj);
        //obj.SetActive(false);
    }
    public GameObject Detach()
    {
        return pool.Dequeue();
    }
    public int count()
    {
        return pool.Count;
    }
}
