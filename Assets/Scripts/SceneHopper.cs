using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHopper : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private ControlManager controlManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.O) || controlManager.select) 
        {
            SceneManager.LoadScene(sceneName); 
        }
    }
}
