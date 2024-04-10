using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    float t;
    Vector3 creditStartPosition;
    Vector3 creditTarget;

    public TextMeshProUGUI creditText;

    public Canvas parentCanvas;

    private void Start()
    {
        creditTarget = new Vector3(parentCanvas.GetComponent<RectTransform>().position.x, 5500f, 0f);
        creditStartPosition = new Vector3(parentCanvas.GetComponent<RectTransform>().position.x, -1700f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime/60f;

        creditText.transform.position = Vector3.Lerp(creditStartPosition, creditTarget, t);

        if (creditText.transform.position.y == creditTarget.y)
        {
            StartCoroutine(WaitThenJump());
        }
    }

    IEnumerator WaitThenJump()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Credits done");
        SceneManager.LoadScene("MainMenu");
    }
}
