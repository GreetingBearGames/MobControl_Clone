using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Thrower : MonoBehaviour
{
    [SerializeField]private GameObject humanPrefab, superHumanPrefab,slideToMoveCanvas;
    [SerializeField]private float cannonSpeed, slideSpeed;
    private bool isMousePressed = false, isMoving = false, release, instantiateSuperHuman;
    private float xRange, xCenter, direction, mouseDeltaX, zStartPos, releaseAmountPerHuman, totalRelease;
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
        transform.DOLocalMove(targets[HumanMove.i].position, Vector3.Distance(targets[HumanMove.i].position, transform.position)/cannonSpeed).OnComplete(()=>{
            slideToMoveCanvas.SetActive(true);
            GameObject.FindGameObjectWithTag("CM").GetComponent<Cinemachine.CinemachineFreeLook>().enabled = false;
            });
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
        
        if(HumanMove.i == 3){
            transform.parent.DOLocalMoveX((transform.right.x * slideAmount)+1.86f, 0.1f);
            transform.parent.DOLocalMoveZ((transform.right.x * slideAmount)+1.86f, 0.1f);
        }
        else if(HumanMove.i > 3){
            Debug.Log("slideamount" + (transform.right.x * slideAmount));
            Debug.Log("actual transform" + transform.parent.position);
            transform.parent.DOLocalMoveX((transform.right.x * slideAmount) +13.1f, 0.1f);
        }
        else{
            transform.parent.DOLocalMoveX(transform.right.x * slideAmount, 0.1f);
        }
    }
    private void InstantiateAndThrow(){
        if(isMousePressed && instantiateSuperHuman){
            this.gameObject.GetComponent<Animator>().Play("CannonShoot",  -1, 0f);;
            GameObject go = Instantiate(superHumanPrefab,new Vector3(this.transform.position.x, superHumanPrefab.transform.position.y, this.transform.position.z), Quaternion.identity);
            if(HumanMove.i == 3)
                go.transform.DOJump(new Vector3(transform.position.x - 0.71f, transform.position.y, transform.position.z + 0.71f), 0.4f, 1, 0.2f);
            else    
                go.transform.DOJump(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), 0.4f, 1, 0.2f);
            instantiateSuperHuman = false;
            ballBarController.DischargeTheBar();
            this.gameObject.transform.GetChild(6).GetChild(7).gameObject.GetComponent<ParticleSystem>().Play();
        }
        else if (isMousePressed && !instantiateSuperHuman){
            this.gameObject.GetComponent<Animator>().Play("CannonShoot",  -1, 0f);
            ballBarController.shootedValue+=releaseAmountPerHuman;
            GameObject go = Instantiate(humanPrefab, new Vector3(this.transform.position.x, humanPrefab.transform.position.y, this.transform.position.z), Quaternion.identity);
            if(HumanMove.i == 3)
                go.transform.DOJump(new Vector3(transform.position.x - 0.71f, transform.position.y, transform.position.z + 0.71f), 0.2f, 1, 0.2f);
            else    
                go.transform.DOJump(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), 0.2f, 1, 0.2f);
        }
    }
    private void DestroyTower(){
        if(HumanMove.isWin){
            CancelInvoke("InstantiateAndThrow");
            GameObject.FindGameObjectWithTag("CM").GetComponent<Cinemachine.CinemachineFreeLook>().enabled = true;
            transform.DOMove(targets[HumanMove.i].position, Vector3.Distance(targets[HumanMove.i].position, transform.position)/cannonSpeed).OnComplete(()=>{
                HumanMove.i++;
                transform.DORotateQuaternion(targets[HumanMove.i].rotation, 0.2f).OnComplete(()=>{
                transform.DOMove(targets[HumanMove.i].position, Vector3.Distance(targets[HumanMove.i].position, transform.position)/cannonSpeed);
                if(HumanMove.i == 2){
                    towers[1].SetActive(true);
                    Tower.towerHp = 100f;
                }
                else if(HumanMove.i == 4){
                    towers[2].SetActive(true);
                    towers[3].SetActive(true);
                    Tower.towerHp = 100f;
                }
                HumanMove.i++;
                GameObject.FindGameObjectWithTag("CM").GetComponent<Cinemachine.CinemachineFreeLook>().enabled = false;
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
                if(item.transform.position.z < 30)
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
