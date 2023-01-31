using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dragger :MonoBehaviour, IDragHandler
{
	public GameObject cam;
	public float sensibility;
    public void OnDrag(PointerEventData eventData)
    {
		Vector2 pos = cam.transform.position;
		pos -= eventData.delta/sensibility;
		cam.transform.position = new Vector3(pos.x, pos.y, -10);
        
    }
}
