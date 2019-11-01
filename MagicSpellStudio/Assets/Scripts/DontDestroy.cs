using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private static DontDestroy s;
    void Awake()
    {
        if( s == null)
        {
            DontDestroyOnLoad(this.gameObject);
            s = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }
}
