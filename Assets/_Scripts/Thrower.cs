using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Thrower : MonoBehaviour
{
    [SerializeField]private GameObject humanPrefab, superHumanPrefab,slideToMoveCanvas;
    [SerializeField]private float cannonSpeed;
    private bool isMousePressed = false, isMoving = false, release, instantiateSuperHuman;
    private float xRange, xCenter, direction, slideSpeed, mouseDeltaX, zStartPos, releaseAmountPerHuman, totalRelease;
    Vector3 mousePos;
    public List <Transform> targets;
    public List <GameObject> towers;
    [SerializeField]private BallBarController ballBarController;
    private void Awake() {
        release = false;
        releaseAmountPerHuman = 2.0f;
        totalRelease = 20.0f;
        ballBarController.shootMaxValue = totalRelease;
        zStartPos = transform.position.z;
        xCenter = transform.position.x;
        //xRange = this.gameObject.GetComponent<Renderer>().bounds.size.x;
    }
    private void Start() {
        transform.DOLocalMove(targets[HumanMove.i].position, Vector3.Distance(targets[HumanMove.i].position, transform.position)/cannonSpeed);
        HumanMove.i++;
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
        GlowParticleScaleChange();
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
            this.gameObject.GetComponent<Animator>().Play("CannonShoot",  -1, 0f);;
            GameObject go = Instantiate(superHumanPrefab, this.transform.position, Quaternion.identity);
            go.transform.DOJump(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), 0.4f, 1, 0.2f);
            instantiateSuperHuman = false;
            ballBarController.DischargeTheBar();
            this.gameObject.transform.GetChild(6).GetChild(7).gameObject.GetComponent<ParticleSystem>().Play();
        }
        else if (isMousePressed && !instantiateSuperHuman){
            this.gameObject.GetComponent<Animator>().Play("CannonShoot",  -1, 0f);

            ballBarController.shootedValue+=releaseAmountPerHuman;
            GameObject go = Instantiate(humanPrefab, this.transform.position, Quaternion.identity);
            go.transform.DOJump(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), 0.2f, 1, 0.2f);
        }
    }
    private IEnumerator MoveToTarget(Transform target){        
        isMoving = true;
        transform.DOLocalMove(target.position, Vector3.Distance(target.position, transform.position)/cannonSpeed);        
        HumanMove.i++;
        yield return new WaitForSeconds(0.5f);
        slideToMoveCanvas.SetActive(true);
        transform.DOLocalRotateQuaternion(target.rotation, 0.2f);
        transform.DOLocalMove(target.position, Vector3.Distance(target.position, transform.position)/cannonSpeed);
        isMoving = false;
    }
    private void DestroyTower(){
        if(HumanMove.isWin){
            CancelInvoke("InstantiateAndThrow");
            transform.DOLocalMove(targets[HumanMove.i].position, Vector3.Distance(targets[HumanMove.i].position, transform.position)/cannonSpeed).OnComplete(()=>{
                HumanMove.i++;
                transform.DORotateQuaternion(targets[HumanMove.i].rotation, 0.2f).OnComplete(()=>{
                transform.DOLocalMove(targets[HumanMove.i].position, Vector3.Distance(targets[HumanMove.i].position, transform.position)/cannonSpeed);
                if(HumanMove.i == 2){
                    towers[1].SetActive(true);
                }
                else if(HumanMove.i == 4){
                    towers[2].SetActive(true);
                    towers[3].SetActive(true);
                }
                });
            });
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
        if(ballBarController.shootedValue >= totalRelease){
            release = true;
        }
    }
    private void GlowParticleScaleChange(){
        this.transform.GetChild(6).GetChild(6).GetChild(0).GetComponent<RectTransform>().DOScale(Vector3.one * ballBarController.fillImageBlue.fillAmount, 0.1f);
    }
}
