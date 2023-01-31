using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System;

public class DataRetriever : MonoBehaviour
{
    public static DataRetriever ins;
    public static event EventHandler OnDrawFinished;

    public Config config;

    public GameObject container;

    public GameObject navigator;
    public GameObject navContainer;
    public  float originalYPos;
    public  float originalXPos;

    public GameObject originalPos;

    public  float Ydist;
	public  float Xdist;
    public float genIndex;

    public  GameObject node;

    public List<GameObject> allNodes = new List<GameObject>();
    public GameObject firstTimePanel;

    //public GameObject nodePoolObj;
    public ObjectPool nodePool;

    public id zoomer;

    public XmlDocument doc = new XmlDocument();
    private void Start()
    {
        ins = this;
        //nodePool = nodePoolObj.GetComponent<ObjectPool>();
        config.GetConfig();


        if (!File.Exists(Handler.dataPath))
        {
            doc.LoadXml("<!DOCTYPE root[<!ELEMENT root ANY ><!ELEMENT node ANY ><!ATTLIST node name ID #REQUIRED>]><root></root>");
            doc.Save(Handler.dataPath);
        }
        else doc.Load(Handler.dataPath);



        if (!doc.DocumentElement.HasChildNodes)
        {
            firstTimePanel.SetActive(true);
        }
        else
            DrawGraph();
        
        SearchField.OnFieldChange += Search_OnFieldChange;
    }
    void test(object sender, EventArgs e)
    {
        Debug.Log("sdfsdf");
    }
    void Search_OnFieldChange(object sender, SearchField.ChangeEventArgs e){
        clearNavContainer();
        RectTransform rt = navContainer.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 0);
        List<GameObject> filtered = allNodes.FindAll(x => {
            return x.GetComponent<Node>().name.Contains(e.value);
        });
        filtered.ForEach(x => {
            var a = Instantiate(navigator);
            
            a.SetActive(true);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + 42);
            a.transform.SetParent(navContainer.transform);
            a.GetComponent<RectTransform>().localScale = new Vector3( 1, 1, 0);
            
            Button b = a.GetComponent<Button>();
            Node curNode = x.GetComponent<Node>();

            Text display = a.transform.GetChild(0).GetComponent<Text>();
            display.text = curNode.name;

            b.onClick.AddListener(() =>{
                //Node.ins.DisPlayInfo(curNode.name, curNode.dob, curNode.children);
                Camera.main.transform.position = new Vector3(
                    curNode.transform.position.x, 
                    curNode.transform.position.y, 
                    Camera.main.transform.position.z);
                SearchHandler.ins.CloseSearch();
            });
        });      
    }  
    public void clearNavContainer(){
        for (int i = 0;i < navContainer.transform.childCount; i++){
            Destroy(navContainer.transform.GetChild(i).gameObject);
        }
    }
    public void clearContainer()
    {
        for (int i = 0; i < container.transform.childCount; i++)
        {
            GameObject objectToRemove = container.transform.GetChild(i).gameObject;
            Node node = objectToRemove.GetComponent<Node>();
            node.UnsubscribeEvent();

            allNodes.Remove(objectToRemove);

            //nodePool.Attach(objectToRemove);
            //objectToRemove.SetActive(false); 
            Destroy(objectToRemove);
        }
    }
    public void DrawGraph(){
        clearContainer();

        Camera.main.backgroundColor = config.backgroundColor;

        XmlElement rootNode = doc.DocumentElement.FirstChild as XmlElement;
        container.transform.localScale = new Vector3(1, 1, 1);

        originalXPos = -5;
        originalYPos = 0;

        string displayMode = PlayerPrefs.GetString("displayMode", "Horizontal");

        if (displayMode == "Horizontal") 
        {
            Xdist = config.HXdist;
            Ydist = config.HYdist;
            GetData2(rootNode, originalXPos, 1);
        }
        else
        {
            Xdist = config.VXdist;
            Ydist = config.VYdist;
            GetData(rootNode, originalYPos, 1);
        }
        Vector3 scale = new Vector3(PlayerPrefs.GetFloat("X", 1), PlayerPrefs.GetFloat("Y", 1), 0);
        container.transform.localScale = scale;
        zoomer.transform.localScale = scale;
        OnDrawFinished?.Invoke(this, EventArgs.Empty);
    }
    public GameObject GetData(XmlElement parent, float YPos, int gen)
    {
        Vector2 spawnPos = new Vector2();
        List<GameObject> childNode = new List<GameObject>();
        float avgX = 0;
        if (parent.HasChildNodes)
        {
            foreach (XmlElement child in parent)
            {
                childNode.Add(GetData(child, YPos - Ydist, gen + 1));
            }
            float sumX = 0;
            foreach (GameObject pos in childNode)
            {
                sumX += pos.transform.position.x;
            }
            avgX = sumX / childNode.Count;
            spawnPos = new Vector2(avgX, YPos);
        }
        else
        {
            Vector2 pos = new Vector2(originalXPos, YPos);

            spawnPos = pos;
            originalXPos += Xdist;
        }
        var a = Instantiate(node, spawnPos, Quaternion.identity);
        //var a = nodePool.count() == 0? Instantiate(node):nodePool.Detach();
        a.transform.position = spawnPos;
        
        a.transform.SetParent(container.transform);
        if (gen == 1) originalPos = a;

        Node _node = a.GetComponent<Node>();
        _node.element = parent;
        _node.gen = gen;
        _node.avg = avgX;
        
        allNodes.Add(a);
        if (parent.HasChildNodes)
        {
            foreach (GameObject child in childNode)
            {
				_node.children.Add(child.GetComponent<Node>());
            }
        }
        OnDrawFinished += _node.DataRetriever_OnDrawFinished;
        return a;
    }
    public GameObject GetData2(XmlElement parent, float Xpos, int gen)
    {
        Vector2 spawnPos = new Vector2();
        List<GameObject> childNode = new List<GameObject>();
        float avgY = 0;
        if (parent.HasChildNodes)
        {
            foreach (XmlElement child in parent)
            {
                childNode.Add(GetData2(child, Xpos + Xdist, gen + 1));
            }
           
            float sumY = 0;
            foreach (GameObject pos in childNode)
            {
                sumY += pos.transform.position.y;
            }
            avgY = sumY / childNode.Count;
            spawnPos = new Vector2(Xpos, avgY);
        }
        else
        {
            Vector2 pos = new Vector2(Xpos, originalYPos);

            spawnPos = pos;
            originalYPos -= Ydist;
        }
        var a = Instantiate(node, spawnPos, Quaternion.identity);
        
        //var a = nodePool.count() == 0? Instantiate(node):nodePool.Detach();
        a.transform.position = spawnPos;

        a.transform.SetParent(container.transform);
        if (gen == 1) originalPos = a;

        Node _node = a.GetComponent<Node>();
        
        _node.element = parent;
        _node.gen = gen;
        _node.avg = avgY;

        _node.children.RemoveAll((x) => { return true; });
        allNodes.Add(a);
        if (parent.HasChildNodes)
        {
            foreach (GameObject child in childNode)
            {
                _node.children.Add(child.GetComponent<Node>());
            }
        }
        OnDrawFinished += _node.DataRetriever_OnDrawFinished;
        if (gen == 1) 
        {
            //Debug.Log("fsfd");
           
        }
        return a;
    }
}
