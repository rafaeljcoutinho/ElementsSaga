using UnityEngine;
using UnityEngine.UI;

public class healthBarController : MonoBehaviour
{
    [SerializeField] public Image healthBar;



    public void setHealthbar(float amount){
        healthBar.fillAmount = amount;
    }

    private void Awake() {
        healthBar.fillAmount = 1;
        transform.LookAt(Camera.main.transform.position);
        transform.Rotate(Vector3.down, 180);
    }


}
