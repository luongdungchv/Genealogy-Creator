using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class Test : MonoBehaviour
{
    LineRenderer line;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        Vector2[] vects = {Vector2.zero, transform.position, Vector2.up };
        line.SetPosition(0, Vector2.zero);
        line.SetPosition(1, transform.position);
        line.SetPosition(2, Vector2.up);
    }

    // Update is called once per frame
    public void DoST()
    {
        
    }
}
