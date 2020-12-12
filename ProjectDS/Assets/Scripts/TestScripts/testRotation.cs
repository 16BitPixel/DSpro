using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testRotation : MonoBehaviour
{

    public Transform player;
    Vector3 safeDis;

    // Start is called before the first frame update
    void Start()
    {
        safeDis = this.transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        boohaaa();
    }


    void boohaaa()
    {
        this.transform.position = player.transform.position + safeDis;

        Quaternion rot = Quaternion.AngleAxis(10f, player.transform.up);
        this.transform.rotation = rot;

        safeDis = rot * safeDis;
    }


}
