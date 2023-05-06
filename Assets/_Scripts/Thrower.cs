using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Thrower : MonoBehaviour
{
    [SerializeField]private GameObject humanPrefab, superHumanPrefab;
    [SerializeField]private float cannonSpeed;
    private bool isMousePressed = false, isMoving = false, release, instantiateSuperHuman;
    [SerializeField]private float xRange, xCenter, direction, slideSpeed, mouseDeltaX, zStartPos, releaseAmountPerHuman, totalRelease, currentRelease;
    Vector3 mousePos;
    public List <Transform> targets;
    private IEnumerator coroutine;
    private void Awake() {
        release = false;
        currentRelease = 0.0f;
        releaseAmountPerHuman = 2.0f;
        totalRelease = 20.0f;
        zStartPos = transform.position.z;
        xCenter = transform.position.x;
        xRange = this.gameObject.GetComponent<Renderer>().bounds.size.x;
    }
    private void Start() {
        coroutine = MoveToTarget(targets[HumanMove.i].position);
        HumanMove.i++;
        StartCoroutine(coroutine);
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isMousePressed = true;
        }
        else
        {
            isMousePressed = false;
        }
        DestroyTower();
        Release();
    }
    private void OnMouseDown()
    {
        mousePos = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        InvokeRepeating("InstantiateAndThrow", 0f, 0.5f);
    }

    private void OnMouseUp()
    {
        CancelInvoke("InstantiateAndThrow");
        if(release){
            release = false;
            instantiateSuperHuman = true;
        }
    }
    private void OnMouseDrag() {
        var slideVector = Camera.main.ScreenToWorldPoint(Input.mousePosition - mousePos);
        var slideAmount = slideVector.x;
        Mathf.Clamp(slideAmount, xCenter-xRange, xCenter+xRange);
        transform.DOLocalMoveX(slideAmount, 0.1f);
    }
    private void InstantiateAndThrow(){
        if(isMousePressed && instantiateSuperHuman){
            GameObject go = Instantiate(superHumanPrefab, this.transform.position, Quaternion.identity);
            go.transform.DOJump(new Vector3(transform.position.x, transform.position.y, transform.position.z + 5), 0.4f, 1, 0.2f);
            instantiateSuperHuman = false;
            currentRelease = 0;
        }
        else if (isMousePressed && !instantiateSuperHuman){
            currentRelease += releaseAmountPerHuman;
            GameObject go = Instantiate(humanPrefab, this.transform.position, Quaternion.identity);
            go.transform.DOJump(new Vector3(transform.position.x, transform.position.y, transform.position.z + 5), 0.2f, 1, 0.2f);
        }
    }
    private IEnumerator MoveToTarget(Vector3 target){
        isMoving = true;
        transform.DOLocalMove(target, Vector3.Distance(target, transform.position)/cannonSpeed);
        yield return new WaitForSeconds(1);
        isMoving = false;
    }
    private void DestroyTower(){
        if(HumanMove.isWin){
            CancelInvoke("InstantiateAndThrow");
            coroutine = MoveToTarget(targets[HumanMove.i].position);
            HumanMove.i++;
            StartCoroutine(coroutine);
            HumanMove.isWin = false;
            foreach(var item in GameObject.FindGameObjectsWithTag("Human")){
                Destroy(item);
            }
            foreach(var item in GameObject.FindGameObjectsWithTag("SuperHuman")){
                Destroy(item);
            }
            foreach(var item in GameObject.FindGameObjectsWithTag("Enemy")){
                Destroy(item);
            }
            foreach(var item in GameObject.FindGameObjectsWithTag("Gate")){
                Destroy(item);
            }

        }
    }
    private void Release(){
        Debug.Log("Current release - Total Release : " + currentRelease + "-" + totalRelease);
        if(currentRelease >= totalRelease){
            Debug.Log("Now Release");
            release = true;
        }
    }
}
