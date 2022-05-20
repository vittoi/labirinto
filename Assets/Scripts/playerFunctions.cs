using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerFunctions : MonoBehaviour
{
    public GameObject itemTeste;
    public Vector3 goTo;
    public bool go;

    public GameObject aimCam;
    private Transform body;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        body = this.transform.GetChild(0);
        rb = body.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)){
            GameObject cipo = Instantiate(itemTeste);
            cipo.transform.position = new Vector3(body.position.x,body.position.y+2,body.position.z)+ body.forward*2;
            cipo.GetComponent<cipoFunctions>().dispara(body.forward, 20);
    
        }
        if(go){
            rb.useGravity = false;
            Vector3 dir = goTo - body.position;
            dir.Normalize();
            print(dir + " "+transform.forward);
            rb.AddForce((dir*0.3f), ForceMode.Impulse);
            //this.transform.Translate(goTo*Time.deltaTime * 0.2f); 
        
            if((Vector3.Distance(this.transform.position, goTo)>0 && Vector3.Distance(this.transform.position, goTo)<10)|| Input.GetKeyDown(KeyCode.E)){
                go = false;
                rb.useGravity = true;
            }
        }

        if(Input.GetMouseButtonDown(1)){
            aimCam.SetActive(true);
        }
        if(Input.GetMouseButtonUp(1)){
            aimCam.SetActive(false);
        }
    }

}
