using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemFunctions : MonoBehaviour
{
    private Transform player;
    private GameObject canvas;

    public float distanceToTip = 5;
    public float distanceToFade = 7;

    void Awake(){
        player = Manager.Instance.player;
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
        if(Vector3.Distance(player.position, transform.position) < distanceToTip){
            Manager.Instance.messagetip.showText("\"Pressione E para pegar\"");
            if(Input.GetKeyDown(KeyCode.E)){
                insertOnInventory();
                Manager.Instance.messagetip.fadeOut();
                Destroy(this.gameObject);
            }
        }else if(Vector3.Distance(player.position, transform.position) < distanceToFade)
        {
            Manager.Instance.messagetip.fadeOut();
        }
    }
    void insertOnInventory(){//insere o item no inventario
        canvas.GetComponent<inventario>().addItem(this.gameObject);
    }

}
