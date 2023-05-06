using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tower : MonoBehaviour
{
    [SerializeField]private GameObject prefab;
    public static float towerHp = 100f;
    void Start()
    {
        InvokeRepeating("InstantiateAndThrow",2, 1f);
    }

    private void InstantiateAndThrow(){
        GameObject go = Instantiate(prefab, this.transform.position, Quaternion.identity);
        go.transform.DOJump(new Vector3(transform.position.x + Random.Range(-2f, 2f), transform.position.y, transform.position.z - 5), 0.2f, 1, 0.2f);
    }
}
