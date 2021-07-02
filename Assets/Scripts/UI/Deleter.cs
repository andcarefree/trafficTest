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
                var deleteList = Selector.Instance.Selected;
                
                for (int i = 0; i < deleteList.Count; i++)
                {
                    GameEvents.Instance.OnDelete(deleteList[i].GetInstanceID());
                }

                Selector.Instance.Selected.Clear();
            }
            yield return null;
        }
    }
}
