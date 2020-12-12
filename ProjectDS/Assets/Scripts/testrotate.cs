using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testrotate : MonoBehaviour
{
    public Transform toLook;
    public float offset = 2f;
    public float rotationSpeed = 10f;
    private float finalAngle = 0;
    // Start is called before the first frame update
    void FixedUpdate()
    {
       faceTarget(toLook.position);
    }    
    private void faceTarget(Vector3 target)
    {        
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;        
        if (finalAngle == 0)
            finalAngle = angle;                    
        Quaternion rot = Quaternion.Euler(new Vector3(0, angle + offset, 0));
        Debug.Log(finalAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed *  Time.deltaTime);
        if (transform.rotation.y <= finalAngle)
        {                      
            // faceTarget(target);
        }
    }    
}
