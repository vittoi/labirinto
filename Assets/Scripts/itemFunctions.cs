using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemFunctions : MonoBehaviour
{
    private Transform player;
    private tipAction tip;
    private GameObject canvas;
    void Awake(){
        player = GameObject.Find("Player").transform;
        tip = GameObject.Find("tipAction").GetComponent<tipAction>();
        canvas = GameObject.Find("menuCanvas");
    }
    // Update is called once per frame
    void Update()
    {
        rotateObj();
        tipByProximity();
    }

    void rotateObj(){
        transform.Rotate(new Vector3(0f, 80f, 0f) * Time.deltaTime);
    }

    void tipByProximity(){
        if(Vector3.Distance(player.position, transform.position) < 5f){
            tip.showText("\"Pressione E para pegar\"");
            if(Input.GetKeyDown(KeyCode.E)){
                insertOnInventory();
                tip.fadeOut();
                Destroy(this.gameObject);
            }
        }else if(Vector3.Distance(player.position, transform.position) < 7f){
           tip.fadeOut();
        }
    }
    void insertOnInventory(){//insere o item no inventario
        canvas.GetComponent<inventario>().addItem(this.gameObject);
    }
}
