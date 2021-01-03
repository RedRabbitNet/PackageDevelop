using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestManager.Instance.Initialize();
    }
}
