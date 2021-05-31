using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 为了让信息面板能够在程序开始时初始化的下下策
public class InitPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        panel.SetActive(false);
    }
}
