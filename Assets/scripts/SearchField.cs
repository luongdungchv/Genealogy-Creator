using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SearchField : MonoBehaviour {

	public static event EventHandler<ChangeEventArgs> OnFieldChange;
	public class ChangeEventArgs:EventArgs{
		public string value;
	}
	
	string text = "";
	
	void Update () {
		//Check if field is changed
		if (this.GetComponent<InputField>().text != text){
			
			if (OnFieldChange != null)
				OnFieldChange(this, new ChangeEventArgs{
					value = this.GetComponent<InputField>().text
				});
		}
		text = this.GetComponent<InputField>().text;

	}
}
