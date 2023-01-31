using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Xml;

public class InfoHandler : MonoBehaviour {
	public static event EventHandler OnInfoClose;

	public GameObject infoPanel;
	public static Node curNode;
	public Text name,dob,gender, wife, dod, children;
	public Button delete, close;
    // Use this for initialization

    void Start() {
		Node.OnNodePressed += OpenPanel_OnNodePressed;
		
	}
	void OpenPanel_OnNodePressed(object sender, EventArgs e){
		//Debug.Log (e.name);
		//Node.OnNodePressed -= OpenPanel_OnNodePressed;
		
		infoPanel.SetActive(true);
		curNode = sender as Node;
		Debug.Log(curNode.wifeList.Count);
		
		Debug.Log (curNode.wife);

		delete.onClick.RemoveAllListeners();
		delete.onClick.AddListener(() =>
		{
			XmlNode nodeToDelete = curNode.element as XmlNode;
			XmlNode parent = nodeToDelete.ParentNode;
			parent.RemoveChild(nodeToDelete);

			infoPanel.SetActive(false);

			DataRetriever.ins.doc.Save(Handler.dataPath);			

			DataRetriever.ins.clearContainer();
			DataRetriever.ins.clearNavContainer();
			//DataRetriever.ins.allNodes.Remove(curNode.gameObject);

			DataRetriever.ins.DrawGraph();
		});
	

		name.text = "Name: ";
		dob.text = "Date of birth: ";
		gender.text = "Gender: ";
		wife.text = "";
		dod.text = "Date of death: ";
		children.text = "";
		
		close.onClick.AddListener(() =>{
			infoPanel.SetActive(false);
			OnInfoClose?.Invoke(this, EventArgs.Empty);
		});

		name.text += curNode.name;
		dob.text += curNode.dob;
		gender.text += curNode.gender;
		
		dod.text += curNode.dod;
		foreach (string i in curNode.wifeList)
		{
			wife.text += (i + Environment.NewLine);
		}
        foreach (Node child in curNode.children)
        {
            this.children.text += (child.name + Environment.NewLine);
        }
    }
}
