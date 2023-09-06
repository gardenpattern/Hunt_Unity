/using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class PoolManager : MonoBehaviour
{
    //������ ������ ����
    public GameObject[] prefabs;

    //Ǯ ����� ����Ʈ��
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // ������ Ǯ�� ����ִ�(��Ȱ��ȭ��) ���� ������Ʈ ����
        

        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)// ��Ȱ��ȭ����
            {//�߽߰� select ������ �Ҵ�
                select = item;
                select.SetActive(true);
                break;
            }

        }
        
        if (!select)
        {
            // ��ã���� ���Ӱ� �����ؼ� select�� �Ҵ�
            select = Instantiate(prefabs[index],transform);
            pools[index].Add(select);
        }

        return select;
    }
}
