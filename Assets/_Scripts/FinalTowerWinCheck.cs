using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTowerWinCheck : MonoBehaviour
{
    [SerializeField] private int towerHP;


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Mob")
        {
            towerHP--;

            if (towerHP == 0)
            {
                Win();
            }
        }
    }

    private void Win()
    {
        //VictoryTextParticle
    }
}
