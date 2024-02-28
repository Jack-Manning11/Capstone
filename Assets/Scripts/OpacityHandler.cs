using System.Collections;
using UnityEngine;

public class OpacityHandler : MonoBehaviour
{
    private Coroutine opacityCoroutine;
    private const float transitionDuration = 0.1f; // Duration of the opacity transition

    public void StartOpacityTransition(SpriteRenderer renderer, float targetOpacity)
    {
        if (opacityCoroutine != null)
        {
            StopCoroutine(opacityCoroutine);
        }
        opacityCoroutine = StartCoroutine(ChangeOpacity(renderer, targetOpacity));
    }

    private IEnumerator ChangeOpacity(SpriteRenderer renderer, float targetOpacity)
    {
        float elapsedTime = 0f;
        float startOpacity = renderer.color.a;

        while (elapsedTime < transitionDuration)
        {
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / transitionDuration);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, newOpacity);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, targetOpacity);
    }
}
