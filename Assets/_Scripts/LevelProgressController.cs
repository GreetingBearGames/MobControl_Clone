using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressController : MonoBehaviour
{
    private int _fillImageIndex=0;
    public int fillImageIndex

    {
        get { return _fillImageIndex; }
        set { _fillImageIndex = value; 
            ActivateTheImage();
        }
    }
    
    private void ActivateTheImage(){
        if (fillImageIndex-1 == 0)
        {
            gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);

        }
        else if (fillImageIndex-1 == 1)
        {
            gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().fillAmount=1;
            gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        }
        else if(fillImageIndex-1 == 2)
        {

        }
        StartCoroutine(CloseCanvas());
    }

    IEnumerator CloseCanvas(){
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
