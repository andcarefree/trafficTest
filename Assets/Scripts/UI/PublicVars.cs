using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicVars
{
    private static PublicVars _current;
    private static readonly object padlock = new object();
    public static PublicVars current
    {
        get
        {
            lock (padlock)
            {
                if(_current == null)
                    _current = new PublicVars();
                return _current;
            }
        }
    }
    public bool isGUIActive = false;
}
