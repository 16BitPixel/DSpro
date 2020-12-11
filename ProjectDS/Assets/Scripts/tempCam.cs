using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempCam : MonoBehaviour
{

    public Transform playerTransform;
    public float speed;
    float mouseX;

    //rotate => camRotation(Input.GetAxis("MouseX"));

    // Start is called before the first frame update
    void Start()
    {
        // playerTransform = GameObject.Find("Player").transform;
        //speed = 1.25f;
        mouseX = Input.GetAxis("Mouse X");
    }

    // Update is called once per frame
    void Update()
    {
        camRotation(mouseX * Time.deltaTime * speed);
    }

    void camRotation(float mouse)
    {
        //Quaternion rot = Quaternion.AngleAxis(mouse, playerTransform.up);

        Quaternion rot = Quaternion.Euler(mouse, this.transform.rotation.y, this.transform.rotation.z);
        this.transform.rotation = rot;
    }

}
