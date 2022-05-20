using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraOffset : MonoBehaviour
{
    private Transform body;
    // Start is called before the first frame update
    void Awake()
    {
        body =  GameObject.FindGameObjectWithTag("Player").transform;;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(body);
        if(Vector3.Distance(body.position, transform.position) > 5){
            
            Vector3 newPosi = new Vector3(body.position.x,  body.position.y+2,  body.position.z);
            print(body.position);
            transform.Translate(newPosi*Time.deltaTime*0.1f);
            
        }

    }
}
