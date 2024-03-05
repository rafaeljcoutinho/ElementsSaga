using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconBehaviour : MonoBehaviour
{
    private void OnDisable() {
        Destroy(this.gameObject);
    }
}
