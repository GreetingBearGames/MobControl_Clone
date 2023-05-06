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

    private void Start() {
        // levelde toplanan cube ve gold sayıları managerden alınacak
    }

    IEnumerator CountLoot(){
        int tempGold=0;
        int tempCube=0;

        while(tempGold < goldCount){
            tempGold++;
            goldText.text = tempGold.ToString();
        }

        while(tempCube < cubeCount){
            tempCube++;
            cubesText.text = tempCube.ToString();
        }

        yield return new WaitForSeconds(countWaitTimer);
    }

    public void NextButton(){
        int activeLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(activeLevel+1);
    }
}
