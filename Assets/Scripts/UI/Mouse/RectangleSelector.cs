using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 该脚本实现框选功能、按住鼠标左键在画布上框选对象
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
                
                var width = (endPosition.x - startPosition.x) * 1920f / Screen.width;
                var height = (endPosition.y - startPosition.y) * 1080f / Screen.height;

                selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
                selectionBox.anchoredPosition = new Vector2(startPosition.x * 1920f / Screen.width + width / 2, startPosition.y * 1080f / Screen.height + height / 2);

                if(!selectionBox.gameObject.activeInHierarchy)
                    selectionBox.gameObject.SetActive(true);

                var selection = Rect.MinMaxRect(Mathf.Min(startPosition.x, endPosition.x), 
                                                Screen.height - Mathf.Max(startPosition.y, endPosition.y),
                                                Mathf.Max(startPosition.x, endPosition.x), 
                                                Screen.height - Mathf.Min(startPosition.y, endPosition.y));
                
                foreach(var go in selectable)
                {
                    var position = Camera.main.WorldToScreenPoint(go.transform.position);
                    var positionInScreen = new Vector2(position.x, Camera.main.pixelHeight - position.y);

                    if(selection.Contains(positionInScreen, true))
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
