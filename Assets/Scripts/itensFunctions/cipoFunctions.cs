using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cipoFunctions : MonoBehaviour
{
    private Rigidbody rb;
    private Transform player;
    private Transform mCamera;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        rb = this.GetComponent<Rigidbody>();
        mCamera = Camera.main.transform;
    }

    public void dispara(Vector3 direction, float rotation)
    {
        rb.AddForce(mCamera.forward*40, ForceMode.Impulse);
            
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == 7){
            playerFunctions p = player.GetComponent<playerFunctions>();
            p.go =true;
            p.goTo = transform.position;
            Debug.Log("acertou");
        }
        Destroy(this.gameObject);
    }
}