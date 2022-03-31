using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mantemEscala : MonoBehaviour
{
    //TODO trocar os itens efetivamente no vetor de itens e Somar as qtd caso nao de arrastar para o proximo vazio 
    private GameObject itemAtual;
    public GameObject emptyBt;
    private bool wClicked = false;
    private bool comecoDoDrag = false;
    private bool intoBt;
    static GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one;
        intoBt = false;
        canvas = GameObject.FindGameObjectsWithTag("Inventario")[0];
    }

    // Update is called once per frame
    
    public void arrastaItem(GameObject item)
    {
        comecoDoDrag = true;
        itemAtual = item;

        if (wClicked == false)
        {
            //Pega index e o pai do item q vai ser arrastado
            inventario.transferencia segurado;
            segurado.index = itemAtual.transform.GetSiblingIndex();
            segurado.pai = itemAtual.transform.parent;
            //Coloca o item arrastado para o layer mais alto
            itemAtual.transform.SetParent(canvas.transform);
            itemAtual.transform.SetSiblingIndex(0);

            //Coloca um campo vazio de onde saiu o item sendo arrastado;
            GameObject aux = Instantiate(emptyBt, segurado.pai.position, Quaternion.identity) as GameObject;
            aux.transform.SetParent(segurado.pai);
            aux.transform.SetSiblingIndex(segurado.index);
            aux.GetComponentInChildren<Text>().text = "";

            canvas.GetComponent<inventario>().setAtual(segurado);

            wClicked = true;
        }

        itemAtual.transform.position = Input.mousePosition;

    }
    public void clickedFalse() {
        print(wClicked + "");
        wClicked = false;
    }
    public IEnumerator InBt(GameObject item)
    {
        while (Input.GetMouseButton(0) && intoBt)
        {
            yield return null;
        }
        if (Input.GetMouseButtonUp(0) && intoBt)
        {
            
            inventario inv = canvas.GetComponent<inventario>();
            inventario.transferencia segurado = inv.getAtual();

            if (segurado.index != -1) {

                int indexDestino = item.transform.GetSiblingIndex();
                Transform paiDestino = item.transform.parent;

                Transform aux  = paiDestino.GetChild(indexDestino);
                Transform aux2 = segurado.pai.GetChild(segurado.index);
                Destroy(aux2.gameObject);

                aux.parent = segurado.pai;
                aux.SetSiblingIndex(segurado.index);
      

                Transform seguradoObj = canvas.transform.GetChild(0);
                seguradoObj.SetParent(paiDestino);
                seguradoObj.SetSiblingIndex(indexDestino);

                comecoDoDrag = false;

                inv.trocaItens(seguradoObj.transform, item.transform); ;
                segurado.index = -1;
                inv.setAtual(segurado);
            }
            yield return null;
        }
        yield return null;
    }
    public void wIn(GameObject item)
    {
        intoBt = true;
        if (!comecoDoDrag)
        {
            StartCoroutine(InBt(item));

        }
    }
    public void setIntoBt()
    {
        intoBt = false;


    }


}
