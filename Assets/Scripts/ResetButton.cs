using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    private string currentSceneName;
    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.R)) SceneManager.LoadScene("MainMenu");
        if (Input.GetKeyUp(KeyCode.Escape)) SceneManager.LoadScene("credits");
    }
}
