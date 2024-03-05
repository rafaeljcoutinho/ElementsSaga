using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupBehaviour : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    [SerializeField] private float popupSpeed;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(Vector3.down, 180);
        Destroy(this.gameObject, timeToDestroy);
    }

    private void Update() {
        transform.Translate(0f, popupSpeed * Time.deltaTime, 0f);
    }
}
