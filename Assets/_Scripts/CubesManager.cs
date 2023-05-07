using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CubesManager : MonoBehaviour
{
    private static CubesManager _instance; 
    [SerializeField] GameUIController gameUIController;

    [SerializeField] Transform target;
    [SerializeField] GameObject cubePrefab;

    Queue<GameObject> cubesQueue = new Queue<GameObject>();
    [SerializeField] Ease easeType;
    [SerializeField] float duration;

    public static CubesManager Instance 
    {     
        get
        {
            if (_instance == null)
            {
                Debug.LogError("CubesManager Null");
            }
            return _instance;
        }
    }
    private void Awake() {
        _instance = this;
        PrepareCubes();
    }
    private void PrepareCubes(){

        for (int i = 0; i < 50; i++)
        {
            GameObject cube;
            cube = Instantiate(cubePrefab);
            //* Scale eklenebilir
            cube.transform.parent = transform;
            cube.SetActive(false);
            cubesQueue.Enqueue(cube);
        }
    }
    
    private void Animate(Vector3 castlePosition){
        if (cubesQueue.Count>0)
            {
                GameObject cube = cubesQueue.Dequeue(); 
                cube.SetActive(true);
                cube.transform.position = castlePosition;
                //cube.transform.parent = target;
                

                cube.transform.DOMove(target.position,duration)
                    .SetEase(easeType)
                    .OnComplete(()=>{
                        cube.SetActive(false);
                        cubesQueue.Enqueue(cube);
                        gameUIController.cubesCount++;
                    }
                );
            }        
    }  

    public void AddCube(Vector3 collectedCoinPosition){
        Animate(collectedCoinPosition);
    }

}
