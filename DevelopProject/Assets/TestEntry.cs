using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEntry : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return StartCoroutine(SoundManager.Instance.InitializeCoroutine());
        SoundManager.Instance.PlayWithLoad("test", AudioMixerGroupEnum.SE);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SoundManager.Instance.PlayWithLoad("test", AudioMixerGroupEnum.SE);   
        }
    }
}
