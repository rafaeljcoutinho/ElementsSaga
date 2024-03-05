using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPlaceHolder : MonoBehaviour
{
    [SerializeField] public EquipmentObject equipment;

    private void Awake() {
        equipment = null;
    }
}
