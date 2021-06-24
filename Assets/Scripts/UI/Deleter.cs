using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyObjectOnLick());
    }
    
    private IEnumerator DestroyObjectOnLick()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                var deleteList = Selector.current.Selected;
                var propertyTable = Inspector.current.PropertyTableList;
                
                for (int i = 0; i < deleteList.Count; i++)
                {
                    GameEvents.current.OnDelete(deleteList[i].GetInstanceID());
                }

                for (int i = 0; i < propertyTable.Count; i++)
                {
                    Destroy(propertyTable[i]);
                }
                Selector.current.Selected.Clear();
            }
            yield return null;
        }
    }
}
