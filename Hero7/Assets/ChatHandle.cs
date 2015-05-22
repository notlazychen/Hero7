using System;
using UnityEngine;
using System.Collections;
using Logic;
using RoyNet.GameServer.Entity;
using UnityEngine.UI;

public class ChatHandle : MonoBehaviour
{
    public Button ButtonSendChat;
    public InputField InputFieldChat;
    public Text OutputText;

    void Start()
    {
        ButtonSendChat.onClick.AddListener(SendChat);

        GameManager.Instance.RegisterCommand(new ChatCommand(e =>
        {
            OutputText.text += Environment.NewLine + e.Text;
        }));
    }

    void SendChat()
    {
        GameManager.Instance.Send((int)CMD_Chat.Send, new Chat_Send() { Text = InputFieldChat.text });
        InputFieldChat.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            SendChat();
        }
    }
}
