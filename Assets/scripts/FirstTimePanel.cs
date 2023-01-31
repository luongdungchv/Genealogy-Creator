using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.Linq;

public class FirstTimePanel : MonoBehaviour
{
    public Text dname;
    public Text day, month, year;

    public ToggleGroup gender;


    public void Submit()
    {
        XmlDocument doc = DataRetriever.ins.doc;
        XmlElement rootElement = doc.DocumentElement;
        XmlElement firstNode = doc.CreateElement("node");

        firstNode.SetAttribute("name", dname.text);
        string dob = day.text + "/" + month.text + "/" + year.text;
        firstNode.SetAttribute("dob", dob);

        string genderText = gender.ActiveToggles()
            .FirstOrDefault<Toggle>()
            .transform.GetChild(1)
            .GetComponent<Text>().text;
        firstNode.SetAttribute("gender", genderText);

        rootElement.AppendChild(firstNode);
        doc.Save(Handler.dataPath);

        DataRetriever.ins.firstTimePanel.SetActive(false);
        DataRetriever.ins.DrawGraph();
    }
}
