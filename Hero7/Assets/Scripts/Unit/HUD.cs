using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour
{
    void LateUpdate()
    {
        this.transform.rotation = Camera.main.transform.rotation;
    }

}
