using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectangleSelector : MonoBehaviour
{
    public static RectangleSelector current;
    private Texture2D texture;
    public bool isSelecting;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public List<GameObject> selectable;
    public List<GameObject> selected;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        texture = new Texture2D(1, 1);
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

            var selectionBox = Rect.MinMaxRect(Mathf.Min(startPosition.x, endPosition.x), 
                                                Screen.height - Mathf.Min(startPosition.y, endPosition.y),
                                                Mathf.Max(startPosition.x, endPosition.x), 
                                                Screen.height - Mathf.Max(startPosition.y, endPosition.y));
            foreach(var go in selectable)
            {
                var position = Camera.main.WorldToScreenPoint(go.transform.position);
                if(selectionBox.Contains(position, true) & !selected.Contains(go))
                {
                    selected.Add(go);
                }
                else if(!selectionBox.Contains(position, true) & selected.Contains(go))
                {
                    selected.Remove(go);
                }
            }
        }
        else
        {
            isSelecting = false;
        }
    }

    void OnGUI()
    {
        if (isSelecting) 
        {
            var box = Rect.MinMaxRect(Mathf.Min(startPosition.x, endPosition.x), 
                                        Screen.height - Mathf.Min(startPosition.y, endPosition.y),
                                        Mathf.Max(startPosition.x, endPosition.x), 
                                        Screen.height - Mathf.Max(startPosition.y, endPosition.y));

            DrawRectangle(box, new Color(0.8f, 0.8f, 0.8f, 0.25f));

            DrawRectangleBorder(box, 2f, new Color(0.8f, 0.8f, 0.8f, 1f));
        }
    }

    private void DrawRectangle(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, texture);
        GUI.color = Color.white;
    }

    private void DrawRectangleBorder(Rect rect, float thickness, Color color)
    {
        // Top
        DrawRectangle(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        DrawRectangle(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        DrawRectangle(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        DrawRectangle(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }
}
