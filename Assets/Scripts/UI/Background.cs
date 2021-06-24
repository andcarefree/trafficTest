using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private GameObject background;

    [SerializeField]
    private Vector3[] mousePosition;

    private float textureWidth;
    private float textureHeight;

    void Start()
    {
        mousePosition = new Vector3[2];
    }

    // 打开文件对话框，选择图片
    public void SetImage()
    {
        ResetImage();

        var path = FileDialog.OpenFileDialog("选择背景图", "jpg", "png");

        if (path != string.Empty)
        {
            var byteArray = System.IO.File.ReadAllBytes(path);
            var texture = new Texture2D(1,1);
            var isLoaded = texture.LoadImage(byteArray);

            if (isLoaded)
            {
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 100.0f);
                
                textureWidth = texture.width;
                textureHeight = texture.height;

                Debug.Log($"{textureWidth} * {textureHeight}");

                background.GetComponent<SpriteRenderer>().sprite = sprite;

                Selector.current.enabled = false;

                StartCoroutine(SetSizeOnDrag());
            }
        }
    }

    // 重置背景图
    public void ResetImage()
    {
        background.SetActive(false);
        background.GetComponent<SpriteRenderer>().sprite = null;
        background.transform.position = Vector3.zero;
        background.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    // 拖拽预览背景图缩放
    private IEnumerator SetSizeOnDrag()
    {
        var isFinished = false;
        var color = background.GetComponent<SpriteRenderer>().color;

        color.a = 0.5f;

        background.GetComponent<SpriteRenderer>().color = color;

        while (true)
        {
            if (!isFinished)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    mousePosition[0] = Util.GetPointOnXZPlane(Input.mousePosition);
                }
                if (Input.GetMouseButton(0))
                {
                    if (!background.activeInHierarchy)
                    {
                        background.SetActive(true);
                    }

                    mousePosition[1] = Util.GetPointOnXZPlane(Input.mousePosition);
                    
                    var widthScale = Mathf.Abs(mousePosition[1].x - mousePosition[0].x) / (textureWidth / 100.0f) ;
                    var heightScale = Mathf.Abs(mousePosition[1].z - mousePosition[0].z) / (textureHeight / 100.0f);

                    // use different calculating method for four different mouse dragging ways
                    if (mousePosition[1].x > mousePosition[0].x && mousePosition[1].z > mousePosition[0].z)
                    {
                        background.transform.position = mousePosition[0] - new Vector3(0f, 0.01f, 0f);
                        background.transform.localScale = new Vector3(widthScale, heightScale, 1.0f);
                    }
                    if (mousePosition[1].x < mousePosition[0].x && mousePosition[1].z < mousePosition[0].z)
                    {
                        background.transform.position = mousePosition[1] - new Vector3(0f, 0.01f, 0f);
                        background.transform.localScale = new Vector3(widthScale, heightScale, 1.0f);
                    }
                    if (mousePosition[1].x > mousePosition[0].x && mousePosition[1].z < mousePosition[0].z)
                    {
                        var pivot = new Vector3(mousePosition[0].x, 0f, mousePosition[1].z);

                        background.transform.position = pivot - new Vector3(0f, 0.01f, 0f);
                        background.transform.localScale = new Vector3(widthScale, heightScale, 1.0f);
                    }
                    if (mousePosition[1].x < mousePosition[0].x && mousePosition[1].z > mousePosition[0].z)
                    {
                        var pivot = new Vector3(mousePosition[1].x, 0f, mousePosition[0].z);

                        background.transform.position = pivot - new Vector3(0f, 0.01f, 0f);
                        background.transform.localScale = new Vector3(widthScale, heightScale, 1.0f);
                    }

                }
                if (Input.GetMouseButtonUp(0))
                {
                    isFinished = true;
                }
            }
            else 
            {
                color.a = 1.0f;

                background.GetComponent<SpriteRenderer>().color = color;

                Selector.current.enabled = true;
                
                yield break;
            }

            yield return null;
        }
    }

}
