using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData<T1, T2, T3>
{
    [SerializeField]
    public List<T1> list1;

    [SerializeField]
    public List<T2> list2;

    [SerializeField]
    public List<T3> list3;
}