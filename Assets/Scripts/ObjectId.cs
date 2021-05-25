using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectId : MonoBehaviour
{
    // 给每个物品一个GUID，用来给事件系统使用
    public Guid Guid { get; protected set; }

    private void start()
    {
        Guid = Guid.NewGuid();
    }
}
