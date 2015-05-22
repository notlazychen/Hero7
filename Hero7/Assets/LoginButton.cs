using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using LitJson;
using UnityEngine.UI;

public class LoginButton : MonoBehaviour
{
    public InputField InputUserName;
    public InputField InputPassword;
    private Button _button;

    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }
    
    private void OnClick()
    {
        string username = InputUserName.text;
        string password = InputPassword.text;

        WebRequest request = WebRequest.Create(new Uri(string.Format("http://123.56.119.97:8080/login/{0}/{1}", username, password)));
        using (WebResponse response = request.GetResponse())
        {
            using (Stream stream = response.GetResponseStream())
            {
                byte[] buffer = new byte[1024];
                if (stream != null && stream.CanRead)
                {
                    int ss = stream.Read(buffer, 0, buffer.Length);
                    string responseMsg = Encoding.UTF8.GetString(buffer, 0, ss);
                    JsonData jobject = JsonMapper.ToObject(responseMsg);
                    if (jobject["result"].ToString() == "OK")
                    {
                        GameManager.Instance.Load(jobject);

                        Application.LoadLevelAsync("servScene");
                    }
                    else
                    {
                        //todo: 登录失败
                    }
                }
            }
        }
    }
}
