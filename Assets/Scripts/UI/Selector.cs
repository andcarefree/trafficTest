using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    public static Selector current;

    [SerializeField]
    private RectTransform canvas;

    [SerializeField] 
    private RectTransform selectionBox;

    [SerializeField]
    private List<Vector3> mousePosition;

    public List<GameObject> Selectable { get; set; }
    public List<GameObject> Selected { get; set; }
    
    void Start()
    {
        current = this;
        
        this.Selectable = new List<GameObject>();
        this.Selected = new List<GameObject>();
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
            if (mousePosition.Count == 0)
            {
                mousePosition.Add(Input.mousePosition);
            }
        }
        else if(Input.GetMouseButton(0))
        {
            if (mousePosition.Count == 1)
            {
                mousePosition.Add(Input.mousePosition);
            }
            if (mousePosition.Count == 2)
            {
                mousePosition[1] = Input.mousePosition;

                var xMax = Mathf.Max(mousePosition[1].x, mousePosition[0].x);
                var yMax = Mathf.Max(mousePosition[1].y, mousePosition[0].y);
                var xMin = Mathf.Min(mousePosition[1].x, mousePosition[0].x);
                var yMin = Mathf.Min(mousePosition[1].y, mousePosition[0].y);
                
                var width = (mousePosition[1].x - mousePosition[0].x) *  canvas.rect.width / Screen.width;
                var height = (mousePosition[1].y - mousePosition[0].y) * canvas.rect.height / Screen.height;

                var positionX = mousePosition[0].x * canvas.rect.width / Screen.width + width / 2;
                var positionY = mousePosition[0].y * canvas.rect.height / Screen.height + height / 2;

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
                mousePosition.Clear();
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

    #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(Camera.main.transform.position, ray.direction * 100.0f);
        }
    #endif
}
