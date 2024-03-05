using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NotifySystem : MonoBehaviour
{
    [SerializeField] private GameObject textModel;
    [SerializeField] private float timer; 

    public void Notify(string text){
        var go = Instantiate(textModel);
        go.GetComponentInChildren<TextMeshProUGUI>().text = text;
        Destroy(go, timer);
    }

    private void Awake() {
        DontDestroyOnLoad(this);
    }
}
