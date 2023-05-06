using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CastleController : MonoBehaviour
{

    [SerializeField] CastleController otherCastle;
    [SerializeField] TMP_Text lifeText;
    [SerializeField] GameObject winCanvas;
 
    private int _life;
    public int life
    {
        get { return _life; }
        set { _life = value;}
    }
    
    public void Damage(){
        life--;
        CubesManager.Instance.AddCube(transform.position);
        lifeText.text = life.ToString();
        if (life == 0)
        {
            if (otherCastle.life > 0)
            {
                //destroy animasyonu buraya gelecek
            }
            else
            {
                StartCoroutine(LastThreeSeconds());
            }            
        }
    }

    IEnumerator LastThreeSeconds(){
        yield return new WaitForSecondsRealtime(3f);
        winCanvas.SetActive(true);
    }

}
