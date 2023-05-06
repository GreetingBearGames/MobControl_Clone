using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseLineCheck : MonoBehaviour
{
    private bool isLose = false;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "EnemyMob")
        {
            isLose = true;
            Lose();
        }
    }

    private void Lose()
    {

    }
}
