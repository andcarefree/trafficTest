using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class LoadModelButton : MonoBehaviour
{
    public void LoadModel()
    {
        var file = FileDialog.OpenFileDialog("打开源文件", "C# 源文件(*.cs)" ,"*.cs");
        
        if (file != string.Empty)
        {
            DllReader.LoadDll(file);
        }
    }
}
