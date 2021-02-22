using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private TestPopup popupPrefab;
    private TestPopup popup;
    
    IEnumerator Start()
    {
        popup = Instantiate(popupPrefab, canvas.transform);
        popup.Initialize();
        popup.Open();
        
        yield return null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            popup.Cloase();
        }
    }
}
