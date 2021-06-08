using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Background : MonoBehaviour, IFile
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

        IFile file = this;
        var path = file.OpenFile();

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
        background.SetActive(true);

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

    // 实现IFile接口，用于选取文件
    string IFile.OpenFile()
    {
        FileDialog dialog = new FileDialog();
 
        dialog.structSize = Marshal.SizeOf(dialog);
        dialog.filter = "JPG Files (*.jpg)\0*.jpg\0JPEG Files (*.jpeg)\0*.jpeg\0PNG Files (*.png)\0*.png\0";
        dialog.file = new string(new char[256]);
        dialog.maxFile = dialog.file.Length;
        dialog.fileTitle = new string(new char[64]);
        dialog.maxFileTitle = dialog.fileTitle.Length;
        dialog.initialDir = Application.dataPath;  // 默认路径
        dialog.title = "读取文件";
        dialog.defExt = "JPG"; // 默认显示文件的类型
        dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        
        if (OpenFileDialog.GetOpenFileName(dialog))
        {
            Debug.Log(dialog.file);
            return dialog.file;
        }
        else
        {
            return string.Empty;
        }    
    }

    string IFile.SaveFile()
    {
        throw new System.NotImplementedException();
    }
}
