using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameUIController : MonoBehaviour
{
    [SerializeField] Animator cubesHolderAnimator; 
    [SerializeField] TMP_Text goldsText;
    [SerializeField] TMP_Text cubesText;

    private int _goldsCount;
    public int goldsCount
    {
        get { return _goldsCount; }
        set { _goldsCount = value; 
            goldsText.text = goldsCount.ToString();
        }
    }
    

    private int _cubesCount;
    public int cubesCount
    {
        get { return _cubesCount; }
        set { _cubesCount = value; 
            cubesText.text = cubesCount.ToString();
            cubesHolderAnimator.SetTrigger("Shake");
            }
    }
    
    public void HomeButton(){
        // leveller arası uı a geçilecek
    }
}
