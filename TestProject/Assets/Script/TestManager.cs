using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestManager : Singleton<TestManager>
{
    public void Initialize()
    {
        Debug.Log("TestManagerInitialize");
    }
    
    void Start()
    {
        Debug.Log("Test" + SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Scene2");      
        }
    }
}
