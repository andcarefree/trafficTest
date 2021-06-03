using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    public static Selector current;
    private Vector3 startPosition;
    private Vector3 endPosition;
    
    [SerializeField] 
    private RectTransform selectionBox;

    [field : SerializeField]
    public List<GameObject> Selectable { get; set; }

    [field : SerializeField]
    public List<GameObject> Selected { get; set; }
    
    void Awake()
    {
        current = this;
    }

    void Update()
    {
        RectangleSelection();
    }

    // 框选游戏对象
    private void RectangleSelection()
    {
        if(!EventSystem.current.IsPointerOverGameObject() & !ScrollBar.isGUIActive)
        {
            if(Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
            }
            else if(Input.GetMouseButton(0))
            {
                endPosition = Input.mousePosition;

                var xMax = Mathf.Max(endPosition.x, startPosition.x);
                var yMax = Mathf.Max(endPosition.y, startPosition.y);
                var xMin = Mathf.Min(endPosition.x, startPosition.x);
                var yMin = Mathf.Min(endPosition.y, startPosition.y);
                
                var width = (endPosition.x - startPosition.x) * 1920f / Screen.width;
                var height = (endPosition.y - startPosition.y) * 1080f / Screen.height;

                selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
                selectionBox.anchoredPosition = new Vector2(startPosition.x * 1920f / Screen.width + width / 2, startPosition.y * 1080f / Screen.height + height / 2);

                if(!selectionBox.gameObject.activeInHierarchy)
                    selectionBox.gameObject.SetActive(true);
                
                foreach(var go in Selectable)
                {
                    var position = Camera.main.WorldToScreenPoint(go.transform.position);

                    if(position.x > xMin && position.x < xMax && position.y > yMin && position.y < yMax)
                    {
                        if(!Selected.Contains(go))
                        {
                            Selected.Add(go);
                            go.GetComponent<Outline>().enabled = true;
                        }              
                    }
                }
            }
            else if(Input.GetKey(KeyCode.Escape))
            {
                foreach(GameObject gameObject in Selectable)
                {
                    if(Selected.Contains(gameObject))
                    {    
                        gameObject.GetComponent<Outline>().enabled = false;
                        Selected.Remove(gameObject);
                    }
                }

            }
            else
            {
                if(selectionBox.gameObject.activeInHierarchy)
                {
                    selectionBox.gameObject.SetActive(false);
                }
            }
        }
    }
}
