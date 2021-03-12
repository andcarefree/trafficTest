using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RectangleSelector : MonoBehaviour
{
    public static RectangleSelector current;
    private bool isSelecting;
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private List<GameObject> selectable = new List<GameObject>();
    public List<GameObject> Selectable { get => selectable; set => selectable = value; }
    [SerializeField] private List<GameObject> selected = new List<GameObject>();
    public List<GameObject> Selected { get => selected; set => selected = value; }
    
    void Awake()
    {
        current = this;
    }

    void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject() & !ScrollBar.isGUIActive)
        {
            if(Input.GetMouseButtonDown(0))
            {
                isSelecting = true;
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
                
                foreach(var go in selectable)
                {
                    var position = Camera.main.WorldToScreenPoint(go.transform.position);

                    if(position.x > xMin && position.x < xMax && position.y > yMin && position.y < yMax)
                    {
                        if(!selected.Contains(go))
                        {
                            selected.Add(go);
                            go.GetComponent<Outline>().enabled = true;
                        }              
                    }
                }
            }
            else if(Input.GetKey(KeyCode.Escape))
            {
                foreach(GameObject gameObject in selectable)
                {
                    if(selected.Contains(gameObject))
                    {    
                        gameObject.GetComponent<Outline>().enabled = false;
                        selected.Remove(gameObject);
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
}
