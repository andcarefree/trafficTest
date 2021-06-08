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
    private RectTransform canvas;

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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            RectangleSelection();
        }
    }

    // 框选游戏对象
    private void RectangleSelection()
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
            
            var width = (endPosition.x - startPosition.x) *  canvas.rect.width / Screen.width;
            var height = (endPosition.y - startPosition.y) * canvas.rect.height / Screen.height;

            var positionX = startPosition.x * canvas.rect.width / Screen.width + width / 2;
            var positionY = startPosition.y * canvas.rect.height / Screen.height + height / 2;

            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            selectionBox.anchoredPosition = new Vector2(positionX, positionY);

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
    
    // 返回点选的单一GameObject
    public static GameObject SelectObjectOnClick()
    {
        GameObject selectedObject = null;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;

        if (Physics.Raycast(ray, out rayHit))
        {
            selectedObject = rayHit.collider.gameObject;
        }

        return selectedObject;
    }
}
