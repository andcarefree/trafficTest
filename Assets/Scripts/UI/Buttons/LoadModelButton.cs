using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LoadModelButton : MonoBehaviour, IFile
{
    public void LoadModel()
    {
        IFile openFile = this;
        DllReader.LoadDll(openFile.OpenFile());
    }

    string IFile.OpenFile()
    {
        FileDialog dialog = new FileDialog();
 
        dialog.structSize = Marshal.SizeOf(dialog);
        dialog.filter = "C# Source file\0*.cs";
        dialog.file = new string(new char[256]);
        dialog.maxFile = dialog.file.Length;
        dialog.fileTitle = new string(new char[64]);
        dialog.maxFileTitle = dialog.fileTitle.Length;
        dialog.initialDir = Application.dataPath;  //默认路径
        dialog.title = "读取文件";
        dialog.defExt = "cs"; //显示文件的类型
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

    string IFile.SaveFile()
    {
        throw new System.NotImplementedException();
    }
}
