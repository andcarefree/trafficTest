using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveButton : MonoBehaviour, IFile
{
    [SerializeField]
    private GameObject warningPanel;

    [SerializeField]
    private GameObject warningText;

    public void OnSave()
    {
        IFile openFile = this;

        var saveFilePath = openFile.SaveFile();
        var isFileSaved = SerializationManager.Save(saveFilePath, SaveData.current);

        if (isFileSaved)
        {
            warningPanel.SetActive(true);
            warningText.GetComponent<TextMeshProUGUI>().SetText("保存成功");
        }
        else
        {
            warningPanel.SetActive(true);
            warningText.GetComponent<TextMeshProUGUI>().SetText("保存失败");
        }
    }
    
    public string OpenFile()
    {
        throw new System.NotImplementedException();
    }

    public string SaveFile()
    {
        FileDialog dialog = new FileDialog();

        dialog.structSize = System.Runtime.InteropServices.Marshal.SizeOf(dialog);
        dialog.filter = "Save File (*.save)\0*.save\0";
        dialog.file = new string(new char[256]);
        dialog.maxFile = dialog.file.Length;
        dialog.fileTitle = new string(new char[64]);
        dialog.maxFileTitle = dialog.fileTitle.Length;
        dialog.initialDir = Application.dataPath;  
        dialog.title = "保存文件";
        dialog.defExt = "save";
        dialog.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;

        if (SaveFileDialog.GetSaveFileName(dialog))
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
