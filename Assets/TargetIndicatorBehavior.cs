using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicatorBehavior : MonoBehaviour
{
    [SerializeField] private float speed = 50;
    [SerializeField] private float timeToChange = 1;
    private float actualTime = 0;
    void Update()
    {
        actualTime += Time.deltaTime;
        if(actualTime >= timeToChange){
            speed = -speed;
            actualTime = 0;
        }
        transform.position += speed * Time.deltaTime * Vector3.up;

    }
}
