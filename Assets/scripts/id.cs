using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEditor;
using System;

public class id : MonoBehaviour {

	public static event EventHandler OnMouseScroll;

	float t;
	public float sensitivity;
	Transform mainCam;
	public Transform graphContainer;

	private void Start() {
		mainCam = Camera.main.transform;
	}
	void Update()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = mousePos;
		if (Input.mouseScrollDelta.y != 0)
		{
			
			graphContainer.SetParent(this.transform);
			Vector2 scale = transform.localScale;
			if (Input.mouseScrollDelta.y > 0) t = 1;
			else t = -1;
			scale += new Vector2(1, 1) * Input.mouseScrollDelta.magnitude * t * sensitivity;
			if (scale.x < .05f && scale.y < .05f) scale = new Vector2(.05f, .05f);
			transform.localScale = scale;
			OnMouseScroll?.Invoke(this, EventArgs.Empty);
			SaveZoom(transform.localScale.x, transform.localScale.y);
		}
        else
        {
			this.transform.DetachChildren();
        }
	}
	public void SaveZoom(float x, float y)
    {
		PlayerPrefs.SetFloat("X", x);
		PlayerPrefs.SetFloat("Y", y);
	}
}
