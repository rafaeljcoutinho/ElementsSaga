using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{

    public static PlayerStatsController instance;
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);

        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    public static PlayerStatsController GetInstance()
    {
        return instance;
    }
}
