using System.Collections;
using UnityEngine;

public class OrderHandlerv2 : MonoBehaviour
{
    [SerializeField] private GameObject[] surroundingObjects;
    [SerializeField] private GameObject overlay;
    private float lerpSpeed = 1f;

    void Start(){
      SetOverlayOpacity(0f);
    }

    IEnumerator ReduceOpacity(GameObject[] objects)
    {
        if (objects == null || objects.Length == 0)
            yield break;

        Renderer[] renderers = new Renderer[objects.Length];
        Color[] startColors = new Color[objects.Length];
        Color[] targetColors = new Color[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            Renderer renderer = objects[i].GetComponent<Renderer>();
            if (renderer == null)
                continue;

            renderers[i] = renderer;
            startColors[i] = renderer.material.color;
            targetColors[i] = new Color(startColors[i].r, startColors[i].g, startColors[i].b, 0.2f);
        }

        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (renderers[i] == null)
                    continue;

                renderers[i].material.color = Color.Lerp(startColors[i], targetColors[i], elapsedTime);
            }

            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            if (renderers[i] != null)
                renderers[i].material.color = targetColors[i];
        }
    }

    IEnumerator IncreaseOpacity(GameObject[] objects)
    {
        if (objects == null || objects.Length == 0)
            yield break;

        Renderer[] renderers = new Renderer[objects.Length];
        Color[] startColors = new Color[objects.Length];
        Color[] targetColors = new Color[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            Renderer renderer = objects[i].GetComponent<Renderer>();
            if (renderer == null)
                continue;

            renderers[i] = renderer;
            startColors[i] = renderer.material.color;
            targetColors[i] = new Color(startColors[i].r, startColors[i].g, startColors[i].b, 1f);
        }

        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                if (renderers[i] == null)
                    continue;

                renderers[i].material.color = Color.Lerp(startColors[i], targetColors[i], elapsedTime);
            }

            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        for (int i = 0; i < objects.Length; i++)
        {
            if (renderers[i] != null)
                renderers[i].material.color = targetColors[i];
        }
    }

     private void SetOverlayOpacity(float opacity)
    {
        Renderer renderer = overlay.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color color = renderer.material.color;
            color.a = opacity;
            renderer.material.color = color;
        }
    }

    IEnumerator ChangeOpacityOverTime(GameObject obj, float targetOpacity)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null)
            yield break;

        Material material = renderer.material;
        Color startColor = material.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetOpacity);

        float elapsedTime = 0f;
        while (elapsedTime < 0.5f)
        {
            material.color = Color.Lerp(startColor, targetColor, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        material.color = targetColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(ChangeOpacityOverTime(overlay, 0.95f));
        StartCoroutine(ReduceOpacity(surroundingObjects));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StartCoroutine(ChangeOpacityOverTime(overlay, 0f));
        StartCoroutine(IncreaseOpacity(surroundingObjects));
    }
}
