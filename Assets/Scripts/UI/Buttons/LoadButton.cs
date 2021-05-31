using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour, IFile
{
    [SerializeField]
    private GameObject carPrefab;

    [SerializeField]
    private GameObject loadObject;

    [SerializeField]
    private GameObject warningPanel;
    
    [SerializeField]
    private GameObject warningText;

    public void OnLoad()
    {
        IFile OpenFile = this;
        var loadFile = OpenFile.OpenFile();

        if (loadFile != string.Empty)
        {
            SaveData.current = (SaveData)SerializationManager.Load(loadFile);

            GameEvents.current.DispatchOnLoad();
            
            foreach(var newObject in SaveData.current.objects)
            {
                var gameObject = Instantiate(loadObject, newObject.position, newObject.rotation);
                gameObject.GetComponent<Road>().Id = newObject.id;

                if (newObject.roadType == RoadTypeEnum.SOURCE)
                {
                    var originRoad = gameObject.AddComponent<OriginRoad>();
                    originRoad.Car = carPrefab;
                }
            }

            warningPanel.SetActive(true);
            warningText.GetComponent<TextMeshProUGUI>().SetText("读取成功！");
        }
    }

    public string SaveFile()
    {
        throw new System.NotImplementedException();
    }

    string IFile.OpenFile()
    {
        FileDialog dialog = new FileDialog();
 
        dialog.structSize = Marshal.SizeOf(dialog);
        dialog.filter = "Save files (*.save)\0*.save\0";
        dialog.file = new string(new char[256]);
        dialog.maxFile = dialog.file.Length;
        dialog.fileTitle = new string(new char[64]);
        dialog.maxFileTitle = dialog.fileTitle.Length;
        dialog.initialDir = Application.dataPath;  //默认路径
        dialog.title = "读取文件";
        dialog.defExt = "save"; //显示文件的类型
        dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;  //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        
        if (DialogShow.GetOpenFileName(dialog))
        {
            Debug.Log(dialog.file);
            return dialog.file;
        }
        else
        {
            return string.Empty;
        }
    }
    
    
}
