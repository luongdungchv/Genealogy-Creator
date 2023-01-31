using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using System;

public class SettingHandler : MonoBehaviour
{
    public static event EventHandler OnSettingOpen;
    public static event EventHandler OnSettingClose;

    public GameObject settingPanel;
    public ToggleGroup displayModes;

    public Toggle hori;
    public Toggle vert;
    public Toggle darkMode;

    public GameObject horiConfig;
    public GameObject vertConfig;

    public InputField HXdist;
    public InputField HYdist;
    public InputField VXdist;
    public InputField VYdist;

    public Text HXdistHolder;
    public Text HYdistHolder;
    public Text VXdistHolder;
    public Text VYdistHolder;

    private void Start()
    {
       
        hori.onValueChanged.AddListener(x =>
        {
            horiConfig.SetActive(x);
           
            vertConfig.SetActive(!x);
        });
        vert.onValueChanged.AddListener(x =>
        {
            horiConfig.SetActive(!x);
            vertConfig.SetActive(x);
        });
    }
    public void OpenSetting()
    {
        HXdist.text = PlayerPrefs.GetFloat("HXdist", 0f).ToString();
       
        HYdist.text = PlayerPrefs.GetFloat("HYdist", 0f).ToString();
        VXdist.text = PlayerPrefs.GetFloat("VXdist", 0f).ToString();
        VYdist.text = PlayerPrefs.GetFloat("VYdist", 0f).ToString();
        OnSettingOpen?.Invoke(this, EventArgs.Empty);
        settingPanel.SetActive(true);
        if (PlayerPrefs.GetString("displayMode") == "Horizontal") hori.isOn = true;
        else vert.isOn = true;
    }
    public void CLoseSetting()
    {
        OnSettingClose?.Invoke(this, EventArgs.Empty);
        settingPanel.SetActive(false);
    }
    public void Submit()
    {
        if (darkMode.isOn) PlayerPrefs.SetInt("darkMode", 1);
        else PlayerPrefs.SetInt("darkMode", 0);
        string mode = displayModes
            .ActiveToggles()
            .FirstOrDefault()
            .transform.GetChild(1)
            .GetComponent<Text>().text;
        PlayerPrefs.SetString("displayMode", mode);

        if (horiConfig.activeSelf && HXdist.text.Length > 0 && HYdist.text.Length > 0)
        {
            PlayerPrefs.SetFloat("HXdist", float.Parse(HXdist.text));
            PlayerPrefs.SetFloat("HYdist", float.Parse(HYdist.text));
            Debug.Log(PlayerPrefs.GetFloat("HYdist", 0f));
        }
        if (vertConfig.activeSelf && VXdist.text.Length > 0 && VYdist.text.Length > 0)
        {
            PlayerPrefs.SetFloat("VXdist", float.Parse(VXdist.text));
            PlayerPrefs.SetFloat("VYdist", float.Parse(VYdist.text));
        }
       
        DataRetriever.ins.config.GetConfig();
        


        CLoseSetting();

        DataRetriever.ins.DrawGraph();
        Vector2 camPos = DataRetriever.ins.originalPos.transform.position;

        Camera.main.transform.position = new Vector3(camPos.x, camPos.y, Camera.main.transform.position.z);
        
    }
}
