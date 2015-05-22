//Created by PoqXert (poqxert@gmail.com)

/* Example script */
using UnityEngine;
using System.Collections;
using PoqXert.MessageBox;  

public class PXMSGExample : MonoBehaviour {
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.I))
		{
			MsgBox.Show(0, "Your platform: " + Application.platform.ToString(), "Platform", Method);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Q))
		{
			MsgBox.Show(1, "Are you sure you want to quit?", "Quit", MsgBoxButtons.YES_NO, MsgBoxStyle.Question, Method);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.W))
		{
			MsgBox.Show(2, "Changes will take effect after a reboot.", "Changes", MsgBoxButtons.YES_NO, MsgBoxStyle.Warning, Method, false, "Reboot", "Cancel");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.E))
		{
			MsgBox.Show(3, "File not found", "Error", MsgBoxButtons.OK, MsgBoxStyle.Error, Method, btnText0:"Close");
		}
		else if(Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.C))
		{
			MsgBox.Show(4, "Any unsaved changes will be lost.\nSave changes?", "Save Project", MsgBoxButtons.YES_NO_CANCEL, MsgBoxStyle.Custom, Method, true, "Save", "Don't Save", "Cancel");
		}
		else if(Input.GetKeyDown(KeyCode.M))
		{
			MsgBox.Show(0,								//ID
			            "Message",						//Message Text
			            "Caption",						//Caption Text
			            MsgBoxButtons.YES_NO_CANCEL,	//Buttons: OK / OK_CANCEL / YES_NO / YES_NO_CANCEL
			            MsgBoxStyle.Error,				//Style: Info / Question / Warning / Error / Custom
			            Method,							//The methon that will be called when user click on the button
			            false,							//Block other GUI elements?
			            "Yes",							//Text Button "YES/OK"
			            "No",							//Text Button "NO"
			            "Close"							//Text Button "CANCEL"
			            );
		}
	}

	public void Method(int id, DialogResult btn)
	{
		Debug.Log("Message ID: " + id + " Button: " + btn);
	}
}
