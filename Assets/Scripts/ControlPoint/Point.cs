using UnityEngine;
using System;

public class Point : MonoBehaviour
{
    // 控制点的行为脚本，用三个正交方向操作控制点
    private Action<Guid> action;
    private void Start()
    {
        // 懒得给事件取名字干脆用lambda得了
        action = (Guid guid) => 
        {
            if (guid == GetComponent<ObjectId>().Guid)
            {
                for (int i = 0; i < 3 ; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);       
                }
            }
        }; 
        GameEvents.current.OnSelectedEvent += action;
    }

    private void Update()
    {
        if (transform.parent.localScale.x != 0f)
            transform.localScale = new Vector3(0.5f / transform.parent.localScale.x, 0.5f / transform.parent.localScale.y, 0.5f / transform.parent.localScale.z);
    }

    private void OnDestroy()
    {
        GameEvents.current.OnSelectedEvent -= action;
    }
}
