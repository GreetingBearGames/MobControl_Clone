using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallBarController : MonoBehaviour
{
    
    public Image fillImageBlue;
    [SerializeField] Image fillImageYellow;
    [SerializeField] GameObject releaseImage;

    [SerializeField] public float shootMaxValue;
    private float _shootedValue;
    public float shootedValue // The amount will be increased as the shot is made
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
        fillImageBlue.fillAmount = shootedValue / shootMaxValue;

        if (fillImageBlue.fillAmount == 1)
        {            
            fillImageYellow.gameObject.SetActive(true);
            fillImageBlue.gameObject.SetActive(false);
            releaseImage.SetActive(true);
        }
    }

    public void DischargeTheBar(){ // This will be run when strong shot is made
        shootedValue = 0;
        releaseImage.SetActive(false);
        fillImageBlue.gameObject.SetActive(true);
        fillImageYellow.gameObject.SetActive(false);        
    }
}
