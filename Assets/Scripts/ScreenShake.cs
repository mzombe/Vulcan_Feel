using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool start = false;
    public float duration = 1f, additionalStrength = 2f;
    public AnimationCurve curve;

    void Update()
    {
        if (start){
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking(){
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        while (elapsedTime < duration){
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            Vector3 shake = new Vector3(startPosition.x,startPosition.y,0f);
            shake.x += Random.insideUnitSphere.x * strength * additionalStrength;
            shake.y += Random.insideUnitSphere.y * strength * additionalStrength;
            shake.z = transform.position.z;
            transform.position = shake;
            yield return null;
        }
        transform.position = new Vector3(startPosition.x, startPosition.y, transform.position.z);
    }
}
