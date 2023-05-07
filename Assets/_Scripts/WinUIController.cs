using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinUIController : MonoBehaviour
{
    [SerializeField] TMP_Text cubesText;
    [SerializeField] TMP_Text goldText;

    [SerializeField] float countWaitTimer;

    int cubeCount;
    int goldCount;
    int tempGold=0;
    int tempCube=0;
    private void Start() {

        goldCount = PlayerPrefs.GetInt("goldsCount");
        cubeCount = PlayerPrefs.GetInt("cubesCount");
        StartCoroutine(CountLoot());
    }

    IEnumerator CountLoot(){
        if(tempGold < goldCount){
            tempGold++;
            goldText.text = tempGold.ToString();
        }

        if(tempCube < cubeCount){
            tempCube++;
            cubesText.text = tempCube.ToString();
        }

        yield return new WaitForSeconds(countWaitTimer);
        if (tempGold == goldCount && tempCube == cubeCount)
        {
            StopCoroutine(CountLoot());
        }
        StartCoroutine(CountLoot());
    }

    public void NextButton(){
        int activeLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeLevel+1);
    }
}
