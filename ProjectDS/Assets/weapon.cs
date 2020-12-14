using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public float damagingValue;
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log("Hit " + other.collider.name);
        if (other.collider.name == "Player")
        {            
            other.transform.GetComponent<DS.PlayerManager>().takeDamage(damagingValue);
        }
    }
}
