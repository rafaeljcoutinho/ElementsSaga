using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class listMissions : MonoBehaviour
{
    [SerializeField] private List<Mission> gerenciadorMissao = new List<Mission>();

    void Update()
    {

        foreach (Mission mission in gerenciadorMissao)
        {
            
        }
    }
}


[System.Serializable]
public class Mission
{
    public string name;
    public string description;
    public int reward;
    public int completed;
}

