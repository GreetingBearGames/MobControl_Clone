using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] Animator cubesHolderAnimator; 

    private int _cubesCount;
    public int cubesCount
    {
        get { return _cubesCount; }
        set { _cubesCount = value; 
            cubesHolderAnimator.SetTrigger("Shake");
            }
    }
    
    public void HomeButton(){
        
    }
}
