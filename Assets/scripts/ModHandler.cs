using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Linq;
using System;
using UnityEngine.Rendering;
using JetBrains.Annotations;

public class ModHandler : MonoBehaviour
{
    public static ModHandler ins;

    public GameObject modPanel;
    public GameObject infoPanel;
    public GameObject addWifePanel;

    public InputField nameField;
    public InputField DDfieldB, DDfieldD;
    public InputField MMfieldB, MMfieldD;
    public InputField YYfieldB, YYfieldD;

    public Button openAddWife;
    public InputField wifeField;
    public Button submitAddWife;

    public GameObject demised;
    public Toggle demiseToggle;
    

    public ToggleGroup gender;

    public Text wifeText;

    string wifeString;
    public List<string> wifeList;
    public List<Button> deleteWifeButtons;

    XmlDocument doc;

    private void Start()
    {
        ins = this;
    }

    public void OpenMod()
    {
        modPanel.SetActive(true);
        
        doc = DataRetriever.ins.doc;
        wifeString = InfoHandler.curNode.wife;

        InfoHandler.curNode.GenerateWifeList();
        wifeList = InfoHandler.curNode.wifeList;
        InitWifes(wifeList, wifeText);

        Debug.Log(InfoHandler.curNode.wifeList.Count);

        nameField.text = InfoHandler.curNode.name;
        try
        {
            List<string> dobs = Dobs(InfoHandler.curNode.dob);
            DDfieldB.text = dobs[0];
            MMfieldB.text = dobs[1];
            YYfieldB.text = dobs[2];
        }
        catch { }

        //wifeField.text = InfoHandler.curNode.wife;

        demiseToggle.isOn = false;

        demiseToggle.onValueChanged.AddListener(x =>
        {
            DDfieldD.text = "";
            MMfieldD.text = "";
            YYfieldD.text = "";
            demised.SetActive(x);
        });
    }
    public void CloseMod()
    {

        modPanel.SetActive(false);
    }
    public void OpenAddWife()
    {
        addWifePanel.SetActive(true);
    }
    public void SubmitAddWife()
    {
        addWifePanel.SetActive(false);

        if (wifeField.text.Length > 0)
        {
            //wifeString += (wifeField.text + "/");
            wifeList.Add(wifeField.text);
            InitWifes(wifeList, wifeText);
        }
    }
    public void Submit()
    {
        

        XmlElement element = InfoHandler.curNode.element;

        

        string genderText = gender.ActiveToggles().FirstOrDefault<Toggle>().transform.GetChild(1).GetComponent<Text>().text;
        element.SetAttribute("name", nameField.text);
        element.SetAttribute("dob", DDfieldB.text + "/" + MMfieldB.text + "/" + YYfieldB.text);
        element.SetAttribute("gender", genderText);
        element.SetAttribute("wife", GenerateWifeString(wifeList));
        element.SetAttribute("dod", DDfieldD.text + "/" + MMfieldD.text + "/" + YYfieldD.text);
        CloseMod();

        InfoHandler.curNode.InitProperties();
        InfoHandler.curNode.DisPlayInfo();

        doc.Save(Handler.dataPath);
        
    }
    List<string> Dobs(string dob)
    {
        List<string> res = new List<string>();
        string q = "";
        foreach (char i in dob)
        {
            if (i.ToString() == "/") { res.Add(q); q = "";continue; }
            q += i.ToString();
        }
        res.Add(q);
        return res;
    }
    public void InitWifes(List<string> input, Text write)
    {
       
        write.text = "";
        deleteWifeButtons.ForEach(x => x.gameObject.SetActive(false));
        int index = 0;
        foreach (string i in input)
        {
            write.text += (i + Environment.NewLine);
            Button delete = deleteWifeButtons[index];
            delete.gameObject.SetActive(true);
            delete.onClick.RemoveAllListeners();
            delete.onClick.AddListener(() => deleteWife(i));
            index++;
        }
        
        //write.text += (res + Environment.NewLine);
    }
    public string GenerateWifeString(List<string> list)
    {
        string res = "";
        foreach (string i in list)
        {
            res += (i + "/");
        }
        return res;
    }
    public void deleteWife(string wife)
    {
        wifeList.Remove(wife);
        InitWifes(wifeList, wifeText);
    }
}
