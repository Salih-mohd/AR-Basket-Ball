using System.Collections.Generic;
using UnityEngine;

public class ScorePopPool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject popPrefab;
    [SerializeField] public int size=10;

    private Queue<GameObject> poolQ=new Queue<GameObject> ();

    private void Awake()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(popPrefab);
            obj.SetActive(false);
            poolQ.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        if (poolQ.Count == 0)
        {
            return null;
        }

        GameObject item = poolQ.Dequeue();
        //item.SetActive(true);
        return item;
    }

    public void Return(GameObject item)
    {
        item.SetActive(false);
        item.transform.SetParent(null);
        
        poolQ.Enqueue(item);
    }

}
