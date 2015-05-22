using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// 报文接收处理器
/// </summary>
public class ReceiveConsumer : MonoBehaviour
{
    void Start()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
    }

	void Update ()
	{
	    Action act;
	    if (GameManager.Instance.ActionQueue.TryDequeue(out act))
	    {
	        act.Invoke();
	    }
	}
}
