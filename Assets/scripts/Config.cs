using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config ins;

    public Gradient white;
    public Gradient grey;

    public string displayMode;
    public float HXdist;
    public float HYdist;
    public float VXdist;
    public float VYdist;

    [HideInInspector]
    public Gradient lineColor;
    [HideInInspector]
    public Color backgroundColor;

    private void Start()
    {
        ins = this;
    }
    public void GetConfig()
    {
        displayMode = PlayerPrefs.GetString("displayMode", "Horizontal");
        HXdist = PlayerPrefs.GetFloat("HXdist", 0f);
        HYdist = PlayerPrefs.GetFloat("HYdist", 0f);
        Debug.Log(HYdist);
        VXdist = PlayerPrefs.GetFloat("VXdist", 0f);
        VYdist = PlayerPrefs.GetFloat("VYdist", 0f);
        //Debug.Log(PlayerPrefs.GetInt("darkMode", 0));
        lineColor = PlayerPrefs.GetInt("darkMode", 0) == 0 ? grey : white;
        backgroundColor = PlayerPrefs.GetInt("darkMode", 0) == 1 ? Color.black : Color.white;

    }
}
