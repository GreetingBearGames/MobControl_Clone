using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallBarController : MonoBehaviour
{
    
    [SerializeField] Image fillImageBlue;
    [SerializeField] Image fillImageYellow;
    [SerializeField] GameObject releaseImage;

    [SerializeField] public int shootMaxValue;
    private int _shootedValue;
    public int shootedValue // The amount will be increased as the shot is made
    {
        get { return _shootedValue; }
        set { _shootedValue = value; 
            FillTheBar();
        }
    }
     
    void Start()
    {
        shootedValue = 0;
    }

    private void FillTheBar(){
        fillImageBlue.fillAmount = (float)shootedValue / (float)shootMaxValue;
        if (fillImageBlue.fillAmount == 1)
        {            
            fillImageYellow.enabled = true;
            fillImageBlue.enabled = false;
            releaseImage.SetActive(true);
        }
    }

    public void DischargeTheBar(){ // This will be run when strong shot is made
        shootedValue = 0;
        releaseImage.SetActive(false);
        fillImageBlue.enabled = true;
        fillImageYellow.enabled = false;        
    }
}
