using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    void Start()
    {
        MasterDataManagerSample.Instance.Initialize();
        UserDataManagerSample.Instance.Initialize();
    }
}
