using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log(fillImageIndex);
        gameObject.transform.GetChild(0).GetChild(fillImageIndex-1).gameObject.SetActive(true);
        StartCoroutine(CloseCanvas());
    }

    IEnumerator CloseCanvas(){
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
