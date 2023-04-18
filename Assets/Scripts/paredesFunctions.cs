using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class paredesFunctions : MonoBehaviour
{
    public GameObject database;
    private itensList itens;

    private int id;
    private int qtd;
    private Transform outside;
    private Transform inside;
    private Text txt;
    private Transform player;
    private GameObject canvas;
    private inventario inv;
    void Awake()
    {
        player = GameObject.Find("Player").transform;
        canvas = GameObject.Find("menuCanvas");
        inv = canvas.GetComponent<inventario>();


        itens = database.GetComponent<itensList>();
        //txt =  this.transform.GetChild(2).GetComponent<Text>();
        Transform canv = this.transform.GetChild(0);
        outside =  canv.GetChild(0);
        inside  =  canv.GetChild(1);
        id = 0;
        qtd =8;
        
    }   

    void Update(){
        tipByProximity();
    }

    public void valor(int id){
        outside.GetComponent<Image>().sprite = itens.getItemByIndex(id).GetComponent<ItemEx>().icone;
        inside.GetComponent<Image>().sprite = itens.getItemByIndex(id).GetComponent<ItemEx>().icone;
        qtd = (int)Random.Range(6, 25);
        this.id = id;
    }

    private void cobrar(){
        bool tem = false;
        tem = inv.remove(this.id, this.qtd);
        print(tem);
        if(tem){
            Destroy(this.gameObject);
        }
    }

    void tipByProximity(){
        if(Vector3.Distance(player.position, transform.position) < 6f){
            Manager.Instance.messagetip.showText("\"Pressione E para abrir\"");

            if(Input.GetKeyDown(KeyCode.E)){
                cobrar();
                Manager.Instance.messagetip.fadeOut();
            }
        }else if(Vector3.Distance(player.position, transform.position) < 8f){
            Manager.Instance.messagetip.fadeOut();
           
        }
    }

}
