using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShooter : MonoBehaviour
{

    public GameObject GB;
    public Transform shooterTransform;
    Transform player;
    public float speed; 
    [SerializeField] float time;
    bool playerPresent, ready;

    // Start is called before the first frame update
    void Start()
    {
        time = 2.15f;
        playerPresent = false;
        ready = false;
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (playerPresent)
        {
            shooterTransform.LookAt(player);
            StartCoroutine(shoot(time));
        }
    }


    public IEnumerator shoot(float time)
    {
        if (ready)
        {
            ready = false;
            GameObject FB = Instantiate(GB, this.transform.position, this.transform.rotation) as GameObject;
            FB.GetComponent<Rigidbody>().velocity = this.transform.forward * speed;
             yield return new WaitForSeconds(time);
          //  yield return new WaitForSeconds(2.15f);
            ready = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerPresent = true;
            ready = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            playerPresent = false;
            ready = false;
        }
       
    }



}
