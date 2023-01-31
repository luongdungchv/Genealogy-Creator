using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using TMPro;
public class Node : MonoBehaviour {
    public static Node ins;
	public static event EventHandler OnNodePressed;

	public int gen;

	public float avg;

	public string name;
	public string dob;
	public string gender;
	public string wife;
	public string dod;
	public List<string> wifeList;

	public List<Node> children = new List<Node>();
    public XmlElement element;
	private void Start() {
		ins = this;

		InitProperties();
		if (gender == "Female") this.GetComponent<SpriteRenderer>().color = Color.magenta;
		GenerateWifeList();

		SearchHandler.OnOpenSearch += SearchHandler_OnOpenSearch;
        SearchHandler.OnCloseSearch += SearchHandler_OnCloseSearch;
        InfoHandler.OnInfoClose += InfoHandler_OnInfoClose;
        SettingHandler.OnSettingOpen += SettingHandler_OnSettingOpen;
        SettingHandler.OnSettingClose += SettingHandler_OnSettingClose;
        Node.OnNodePressed += Node_OnNodePressed;
		id.OnMouseScroll += DataRetriever_OnDrawFinished;
        //DataRetriever.OnDrawFinished += DataRetriever_OnDrawFinished;
	}

    public void DataRetriever_OnDrawFinished(object sender, EventArgs e)
    {
		//Debug.Log("called");
		DrawLine();
    }

    private void SettingHandler_OnSettingClose(object sender, EventArgs e)
    {
		this.gameObject.layer = 0;
	}

    private void SettingHandler_OnSettingOpen(object sender, EventArgs e)
    {
		this.gameObject.layer = 2;
	}

    private void Node_OnNodePressed(object sender, EventArgs e)
    {
		this.gameObject.layer = 2;
	}

    private void InfoHandler_OnInfoClose(object sender, EventArgs e)
    {
		this.gameObject.layer = 0;
	}

    private void SearchHandler_OnCloseSearch(object sender, EventArgs e)
    {
		this.gameObject.layer = 0;
	}

    private void SearchHandler_OnOpenSearch(object sender, EventArgs e)
    {
		this.gameObject.layer = 2;
    }
	public void UnsubscribeEvent()
    {
		SearchHandler.OnOpenSearch -= SearchHandler_OnOpenSearch;
		SearchHandler.OnCloseSearch -= SearchHandler_OnCloseSearch;
		InfoHandler.OnInfoClose -= InfoHandler_OnInfoClose;
		SettingHandler.OnSettingOpen -= SettingHandler_OnSettingOpen;
		SettingHandler.OnSettingClose -= SettingHandler_OnSettingClose;
		Node.OnNodePressed -= Node_OnNodePressed;
		DataRetriever.OnDrawFinished -= DataRetriever_OnDrawFinished;
		id.OnMouseScroll -= DataRetriever_OnDrawFinished;
	}
    private void Update() {
        //if (children.Count != 0)
        //{
        //    foreach (Node child in children)
        //    {
        //        LineRenderer line = child.GetComponent<LineRenderer>();
        //        if (PlayerPrefs.GetString("displayMode", "Horizontal") == "Horizontal")
        //        {
        //            line.SetPosition(0, child.GetStartPos());
        //            line.SetPosition(1, this.GetEndPos());
        //        }
        //        else
        //        {
        //            line.SetPosition(0, child.transform.position);
        //            line.SetPosition(1, this.transform.position);
        //        }
        //    }
        //}
        //DrawLine();
    }
	public void DrawLine()
    {
		Debug.Log("drawed");
		if (children.Count != 0)
		{
			float sum = 0;
			Vector2 avgPos = new Vector2();
			bool check = PlayerPrefs.GetString("displayMode", "Horizontal") == "Horizontal";
			foreach (Node child in children)
            {
				if (check)
				{
					sum += child.transform.position.y;
				}
				else sum += child.transform.position.x;
			}
			float average = sum / children.Count;
			if (check)
            {
				float avgX = (transform.position.x + children[0].transform.position.x) / 2;
				avgPos = new Vector2(avgX, average);
            }
            else
			{
				float avgY = (transform.position.y+ children[0].transform.position.y) / 2;
				avgPos = new Vector2(average, avgY);
			}
			
			foreach (Node child in children)
			{
				LineRenderer line = child.GetComponent<LineRenderer>();
				line.colorGradient = Config.ins.lineColor;

				Vector2 from = child.GetStartPos();
				Vector2 to = this.GetEndPos();
				if (check)
				{
					line.SetPosition(0, from);
					line.SetPosition(1, new Vector2(avgPos.x, child.transform.position.y));
					line.SetPosition(2, avgPos);
					line.SetPosition(3, to);
                }
                else
                {
					line.SetPosition(0, child.transform.position);
					line.SetPosition(1, new Vector2(child.transform.position.x, avgPos.y));
					line.SetPosition(2, avgPos);
					line.SetPosition(3, this.transform.position);
				}
			}
		}
	}
	
	void OnMouseDown()
	{
		DisPlayInfo();	
	}
	public void DisPlayInfo(){
		if (OnNodePressed != null)
			OnNodePressed(this, EventArgs.Empty) ;
	}
	public void InitProperties()
    {
		GameObject child = this.transform.GetChild(0).gameObject;
		TextMeshPro text = child.GetComponent<TextMeshPro>();

		this.name = element.GetAttribute("name");
		this.dob = element.GetAttribute("dob");
		this.gender = element.GetAttribute("gender");
		this.wife = element.GetAttribute("wife");
		this.dod = element.GetAttribute("dod").Length > 0 ? element.GetAttribute("dod") : "None";

		text.text = name;
	}
	public void GenerateWifeList()
    {
		if (wife.Length == 0) return;
		wifeList.Clear();
		string res = "";
		foreach (char i in wife)
        {
			if (i.ToString() == "/")
            {
				wifeList.Add(res);
				res = "";
				continue;
            }
			res += i.ToString();
        }
	}
	public Vector3 GetStartPos()
    {
		return this.transform.GetChild(1).position;
    }
	public Vector3 GetEndPos()
	{
		return this.transform.GetChild(2).position;
	}
}
