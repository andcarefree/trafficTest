using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSelector : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    public static RectangleSelector current;
    public bool isSelecting;
    public RectTransform selectionBox;
    public List<GameObject> selectable;
    public List<GameObject> selected;
    
    void Awake()
    {
        current = this;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            startPosition = Input.mousePosition;
        }
        else if(Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
            
            var width = (endPosition.x - startPosition.x) * 1920f / Screen.width;
            var height = (endPosition.y - startPosition.y) * 1080f / Screen.height;

            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            selectionBox.anchoredPosition = new Vector2(startPosition.x * 1920f / Screen.width + width / 2, startPosition.y * 1080f / Screen.height + height / 2);

            if(!selectionBox.gameObject.activeInHierarchy)
                selectionBox.gameObject.SetActive(true);

            var selection = Rect.MinMaxRect(Mathf.Min(startPosition.x, endPosition.x), 
                                            Screen.height - Mathf.Min(startPosition.y, endPosition.y),
                                            Mathf.Max(startPosition.x, endPosition.x), 
                                            Screen.height - Mathf.Max(startPosition.y, endPosition.y));
            
            foreach(var go in selectable)
            {
                var position = Camera.main.WorldToScreenPoint(go.transform.position);
                
                if(selection.Contains(position, true) & !selected.Contains(go))
                {
                    selected.Add(go);
                    go.GetComponent<Outline>().enabled = true;
                }
                else if(!selection.Contains(position, true) & selected.Contains(go))
                {
                    selected.Remove(go);
                    go.GetComponent<Outline>().enabled = false;
                }
            }
        }
        else
        {
            isSelecting = false;

            if(selectionBox.gameObject.activeInHierarchy)
                selectionBox.gameObject.SetActive(false);
        }
    }
}
