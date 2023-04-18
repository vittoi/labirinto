using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemEx : MonoBehaviour
{
    public string nome;
    public int id = -1;
    public int index;//posicao dele no inventario
    public int resistencia;
    public Sprite icone;
    public int qtd = 0, qtdMax;
    public float cd;

    public float timeToDesapear = 15;
    
    protected itensController iFunctions;
    protected Transform player;
    protected MakeLevel map;
    protected inventario inv;



    private void Awake()
    {
        iFunctions = GameObject.Find("menuCanvas").GetComponent<itensController>();
        player = Manager.Instance.player;
        map = GameObject.Find("InitializeGame").GetComponent<MakeLevel>();

        GameObject canvas = GameObject.Find("menuCanvas");

        inv = canvas.GetComponent<inventario>();
        
    }

    public virtual void handle(GameObject item) {
        
    }

    public virtual void runtime() { 
    }
}


