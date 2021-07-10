using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;

public static class FileDialog
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class FileDialogConfig
    {
        public int structSize = 0;
        public IntPtr dlgOwner = IntPtr.Zero;
        public IntPtr instance = IntPtr.Zero;
        public String filter = null;
        public String customFilter = null;
        public int maxCustFilter = 0;
        public int filterIndex = 0;
        public String file = null;
        public int maxFile = 0;
        public String fileTitle = null;
        public int maxFileTitle = 0;
        public String initialDir = null; // default path
        public String title = null;
        public int flags = 0; 
        public short fileOffset = 0;
        public short fileExtension = 0;
        public String defExt = null; // default file extension
        public IntPtr custData = IntPtr.Zero;
        public IntPtr hook = IntPtr.Zero;
        public String templateName = null;
        public IntPtr reservedPtr = IntPtr.Zero;
        public int reservedInt = 0;
        public int flagsEx = 0;
    }

    [DllImport("user32.dll")]
    static extern IntPtr GetForegroundWindow();

    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out]FileDialogConfig dialog);

    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetSaveFileName([In, Out]FileDialogConfig dialog);

    static string Filter(params string[] filters)
    {
        return string.Join("\0", filters) + "\0";
    }

    public static string OpenFileDialog(string title, params string[] extensions)
    {
        FileDialogConfig ofn = new FileDialogConfig();
        ofn.structSize = Marshal.SizeOf(ofn);

        var filters = new List<string>();
        filters.Add("All Files (*.*)");
        filters.Add("*.*");
        foreach (var ext in extensions)
        {
            filters.Add(ext);
        }

        ofn.filter = Filter(filters.ToArray());
        ofn.filterIndex = 2;
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = Application.dataPath;
        ofn.defExt = extensions[1];
        ofn.title = title;
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;// OFN_EXPLORER | OFN_FILEMUSTEXIST | OFN_PATHMUSTEXIST | OFN_ALLOWMULTISELECT | OFN_NOCHANGEDIR
        ofn.dlgOwner = GetForegroundWindow(); // 这一步将文件选择窗口置顶。
        if (!GetOpenFileName(ofn))
        {
            return null;
        }
        return ofn.file;
    }

    public static string SaveFileDialog(string title, params string[] extensions)
    {
        FileDialogConfig ofn = new FileDialogConfig();
        ofn.structSize = Marshal.SizeOf(ofn);

        var filters = new List<string>();

        foreach (var ext in extensions)
        {
            filters.Add(ext);
        }

        ofn.filter = Filter(filters.ToArray());
        ofn.filterIndex = 2;
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = Application.dataPath;
        ofn.title = title;
        ofn.flags = 0x00000002 | 0x00000004; // OFN_OVERWRITEPROMPT | OFN_HIDEREADONLY
        ofn.dlgOwner= GetForegroundWindow(); // 这一步将文件选择窗口置顶。
        if (!GetSaveFileName(ofn))
        {
            return null;
        }
        return ofn.file;
    }
} 
