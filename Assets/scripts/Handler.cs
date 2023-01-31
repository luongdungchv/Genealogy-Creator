using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handler : MonoBehaviour
{
    public static string dataPath;

    private void Start()
    {
        dataPath = Application.dataPath + "/data.xml";
    }
    public void Quit()
    {
        Application.Quit();
    }
}
