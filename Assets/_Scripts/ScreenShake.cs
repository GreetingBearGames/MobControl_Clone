using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public AnimationCurve curve;
    private float duration = 1f;
    bool isFinished = false;


    public IEnumerator Shaking()
    {
        if(!isFinished){
            Vector3 startPosition = Camera.main.transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float strength = curve.Evaluate(elapsedTime / duration);
                Camera.main.transform.position = startPosition + Random.insideUnitSphere * strength;
                yield return null;
            }

            Camera.main.transform.position = startPosition;
            isFinished=true;
        }
        
    }
}
