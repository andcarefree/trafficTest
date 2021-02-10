using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Reflection;

public class DllReader 
{
    private static OrginCustom custom;
    /// <summary>
    /// 读取dll文件
    /// </summary>
    /// <param name="className"></param>类名（包括命名空间）
    /// <param name="fileName"></param>文件所在的地址
    /// <returns></returns> 类型
    public static Type ReadDll(string className,string fileName)
    {
        var a2=Assembly.LoadFile(@"Custom\UnityBehaviorTree.dll");
        FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
        byte[] b = new byte[fs.Length];
        fs.Read(b, 0, b.Length);
        fs.Dispose();
        fs.Close();

        Assembly assembly = Assembly.Load(b);

        Car c;
        

        Type type = assembly.GetType(className);
        Following.gm= (Following.GM)Delegate.CreateDelegate(type, type.GetMethod("CustomGM"));
        
        
        return type;
       
    }
    
    public static void TestDllRecover()
    {
        Type type = ReadDll(@"Recover", @"Custom\unityDllTest.dll");
        
    }

    
}
