using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Tower : MonoBehaviour
{
    [SerializeField]private GameObject enemyPrefab, particlePrefab;
    [SerializeField] private TMP_Text towerHPText;

    [SerializeField] GameObject progressBar;
    public static float towerHp = 100f;
    private int maxHP=100, givedCubeCount=0;
    void Start()
    {
        InvokeRepeating("InstantiateAndThrow",2, 1f);
    }

    private void Update() {
        towerHPText.text = Mathf.Ceil(towerHp).ToString();
        GiveCube();
    }

    private void InstantiateAndThrow(){
        GameObject go = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
        go.transform.DOJump(new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y, transform.position.z - 1), 0.2f, 1, 0.2f);
    }

    private void GiveCube(){
        if (maxHP-towerHp > givedCubeCount)
        {
            if(this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !this.gameObject.GetComponent<Animator>().IsInTransition(0))
                this.GetComponent<Animator>().Play("TowerHitAnimaton", -1, 0);
            if(!this.transform.GetChild(0).GetChild(11).GetComponent<ParticleSystem>().isPlaying)
                this.transform.GetChild(0).GetChild(11).GetComponent<ParticleSystem>().Play();
            CubesManager.Instance.AddCube(transform.position);
            givedCubeCount++;
        }
    }

    private void OnDestroy() {
        progressBar.SetActive(true);
        Destroy(progressBar,2f);
        Instantiate(particlePrefab, transform.position, Quaternion.identity);
    }
}
