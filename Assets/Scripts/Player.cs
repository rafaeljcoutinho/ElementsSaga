using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InventoryObject inventory;
    [SerializeField] private PlayerStats target;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private battleController battleController;
    [SerializeField] private GameObject playerVisual;
    [SerializeField] private Ability ability1;
    [SerializeField] private Ability ability2;
    [SerializeField] private Ability normalAtack;
    public Vector3 originalPosition;
    public int damage;
    public float animationTime;

}
