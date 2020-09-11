using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serial : MonoBehaviour
{

    public SerialHandler serialHandler;
    void Start()
    {
        serialHandler.OnDataReceived += OnDataReceived;
    }

    void OnDataReceived(string message)
    {
        var data = message.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        if (data.Length != 1) return;
    }
}
