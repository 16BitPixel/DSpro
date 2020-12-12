using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    public Transform targetTransform;
    public Transform cameraHolderTransform;


    Vector3 safeDis;


    private void Start()
    {
        safeDis = this.transform.position - targetTransform.position;
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        cameraHolderTransform.transform.position = targetTransform.position + safeDis;
    }

}
