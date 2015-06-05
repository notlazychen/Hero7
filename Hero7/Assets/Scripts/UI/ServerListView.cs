using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ServerListView : MonoBehaviour
{
    public GameObject ServerItemPrefab;
    
    void Start()
    {
        for (int i = 0; i < GameManager.Instance.ServerList.Count; i++)
        {
            var server = GameManager.Instance.ServerList[i];
            var serverObj = GameObject.Instantiate(ServerItemPrefab);
            serverObj.transform.SetParent(this.transform);
            var btnText = serverObj.transform.GetChild(0).GetComponent<Text>();
            btnText.text = server.Name;
            serverObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("进入"+server.Name);
                GameManager.Instance.Enter(server);
            });
        }
    }
}
