using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using UnityEngine.SceneManagement;
using System.Linq;

public class AddHandler : MonoBehaviour {
	public GameObject addPanel;
	public GameObject infoPanel;

	public InputField name;
	public InputField DDfield;
	public InputField MMfield;
	public InputField YYfield;
	//public InputField wifeField;
	public List<InputField> wifeFields;

	public ToggleGroup gender;

	public void OpenAddPanel(){
		addPanel.SetActive(true);
	}
	public void CloseAddPanel(){
		addPanel.SetActive(false);
	}
	public void Submit(){
		XmlDocument doc = DataRetriever.ins.doc;

		XmlElement element = InfoHandler.curNode.element;
		XmlElement newelem = doc.CreateElement("node");

		newelem.SetAttribute("name", name.text);

		string dob = DDfield.text + "/" + MMfield.text + "/" + YYfield.text;
		newelem.SetAttribute("dob", dob);

		string genderText = gender.ActiveToggles().FirstOrDefault<Toggle>().transform.GetChild(1).GetComponent<Text>().text;

		newelem.SetAttribute("gender", genderText);
		//newelem.SetAttribute("wife", wifeField.text);

		element.AppendChild(newelem);
		doc.Save(Handler.dataPath);
		//Application.LoadLevel("familygraph");
		GameObject con = DataRetriever.ins.container;

		//DataRetriever.ins.clearContainer();
		DataRetriever.ins.clearNavContainer();

		DataRetriever.ins.DrawGraph();
		
		addPanel.SetActive(false);
		infoPanel.SetActive(false);				
	}
	string GetWifeString()
    {
		string res = "";

		foreach (InputField i in wifeFields)
        {

        }

		return res;
    }
}
