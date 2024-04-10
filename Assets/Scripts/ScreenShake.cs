using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool start = false;
    public float shakeTime = 1f;
    public AnimationCurve curve;
    public void BeginShake(float duration, float intesity)
    {
        start = false;
        StartCoroutine(ShakeAnimation()); //When it is time to start, start the coroutine
    }

    // Update is called once per frame
    void Update()
    {
        if (start == true) //It is time to start
        {
            BeginShake(0f, 0f);
        }
    }

    IEnumerator ShakeAnimation()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeTime)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / shakeTime);
            Vector2 randOffset = Random.insideUnitCircle; //Add some random direction in a unit sphere
            transform.position = startPosition + new Vector3(randOffset.x, randOffset.y, 0f) * strength;
            yield return null; //Pause till the next frame
        }

        transform.position = startPosition;
        yield return null;
    }
}
