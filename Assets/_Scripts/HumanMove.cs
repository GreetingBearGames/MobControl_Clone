using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanMove : MonoBehaviour
{
    [SerializeField]private Transform Target;
    [SerializeField]private bool isAttacking, isGameover, isHittingTower;
    [SerializeField]private float speed, hp, towerHp, targetX;
    public static bool isWin = false;
    public static int i = 0;
    private Rigidbody rb;
    private Animator anim;
    private Vector3 target;
    [SerializeField]private GameUIController gameUIController;
    [SerializeField]private GameObject superHumanParticlePrefab, enemyDeathParticle, humanDeathParticle;

    private void Awake() {
        gameUIController = GameObject.FindGameObjectWithTag("GameUITag").GetComponent<GameUIController>();
        if(this.CompareTag("Human") || this.CompareTag("SuperHuman")){
            Target = GameObject.FindGameObjectWithTag("Tower").transform;
        }
        else if(this.CompareTag("Enemy")){
            Target = GameObject.FindGameObjectWithTag("Thrower").transform;
        }
        rb = this.GetComponent<Rigidbody>();
        anim = this.transform.GetChild(0).GetComponent<Animator>();

    }
    private void Start() {
        if(this.CompareTag("SuperHuman"))
            hp = 300;
        else
            hp = 100;
        towerHp = 100;
        isAttacking = false;
        isHittingTower = false;
        anim.SetBool("Running", true);
        targetX = transform.position.x;
    }
    private void FixedUpdate() {
        if(this.CompareTag("Human") || this.CompareTag("SuperHuman")){
            target = new Vector3(targetX, this.transform.position.y, Target.position.z);
        }
        else if(this.CompareTag("Enemy")){
            target = new Vector3(this.transform.position.x, this.transform.position.y, Target.position.z);
        }
        StartCoroutine("CheckHpDecrease");
        MoveToTarget();
    }

    private void MoveToTarget(){
        if(!isAttacking && !isHittingTower){
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            transform.LookAt(target);
        }
    }
    private void Freeze(){
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ
            | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
    
    private void OnCollisionStay(Collision other) {
        if((other.gameObject.CompareTag("Enemy") && (this.gameObject.CompareTag("Human") || this.CompareTag("SuperHuman")))
            || ((other.gameObject.CompareTag("Human") || this.CompareTag("SuperHuman")) && this.gameObject.CompareTag("Enemy") )){
            isAttacking = true;
            Freeze();
            hp -= 100f * Time.deltaTime;
            if(hp <= 0){
                if(this.gameObject.CompareTag("SuperHuman")){
                    Instantiate(superHumanParticlePrefab, this.transform.position + new Vector3(0,4.2f,0), Quaternion.identity);
                }
                else if(other.gameObject.CompareTag("Enemy")){
                    gameUIController.goldsCount++;
                    Instantiate(enemyDeathParticle, this.transform.position + new Vector3(0,2.6f,0), Quaternion.identity);
                }
                else if(other.gameObject.CompareTag("Human")){
                    gameUIController.goldsCount++;
                    Instantiate(humanDeathParticle, this.transform.position + new Vector3(0,2.6f,0), Quaternion.identity);
                }

                Destroy(this.gameObject);
            }
        }
        else if((this.gameObject.CompareTag("Human")  || this.CompareTag("SuperHuman")) && other.gameObject.CompareTag("Tower")){
            isHittingTower = true;
            Tower.towerHp -= 1f * Time.deltaTime;
            if(Tower.towerHp <= 0){
                isWin = true;
                Destroy(other.gameObject);
            }
        }
    }
    private void OnCollisionEnter(Collision other) {
        if(this.gameObject.CompareTag("Enemy") && other.gameObject.CompareTag("Thrower")){
            isGameover = true;
            StartCoroutine(Camera.main.GetComponent<ScreenShake>().Shaking());
        }
        else if((this.gameObject.CompareTag("Human") || this.gameObject.CompareTag("SuperHuman")) && other.gameObject.CompareTag("Gate")){
            if(!other.gameObject.transform.parent.GetChild(5).GetComponent<ParticleSystem>().isPlaying){
                other.gameObject.transform.parent.GetChild(5).GetComponent<ParticleSystem>().Play();
            }
            var text = other.transform.parent.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text.ToString();
            int multiplier = int.Parse(text.Substring(0, text.Length - 1));
            for(int i = 0; i < multiplier-1 ; i++){
                var pos = transform.position;
                pos.x = pos.x + Random.Range(-0.25f, 0.25f);
                pos.z = pos.z + Random.Range(0.5f, 1f);
                Instantiate(this.gameObject, pos, Quaternion.identity);
            }
        }
        else if((this.gameObject.CompareTag("Human") || this.CompareTag("SuperHuman"))&& other.gameObject.CompareTag("TowerArea")){
            targetX = Target.position.x + Random.Range(-1f, 1f);
        }
    }
    private IEnumerator CheckHpDecrease(){
        var current = hp;
        yield return new WaitForSeconds(0.1f);
        if(current == hp)
            isAttacking = false;
    }
}
