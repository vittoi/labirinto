using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventario : MonoBehaviour
{
    public GameObject emptyBt;
    public RectTransform barraFullTime;
    public RectTransform estoquePrincipal;
    public GameObject[] itens = new GameObject[40];

    public struct transferencia {
        public int index;
        public Transform pai;
    };
    

    private int firstEmpty = 0;
    private transferencia itemAtual;
    private bool wClicked = false;
    // Start is called before the first frame update
    void Start()
    {
        itemAtual.index = -1;
        desenhaMenu();
        wClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void desenhaMenu()
    {
        while (barraFullTime.childCount<10) {
            
            GameObject item = Instantiate(itens[barraFullTime.childCount], barraFullTime.position, Quaternion.identity) as GameObject;
            item.GetComponentInChildren<Text>().text = "";

            itens[barraFullTime.childCount] = item;

            item.transform.SetParent(barraFullTime);
            

        }
        while (estoquePrincipal.childCount< 30) {
            GameObject item = Instantiate(itens[estoquePrincipal.childCount+10], estoquePrincipal.position, Quaternion.identity) as GameObject;
            
            item.GetComponentInChildren<Text>().text = "";
            

            itens[estoquePrincipal.childCount+10] = item;

            item.transform.SetParent(estoquePrincipal);
        }
    }
    public void setAtual(transferencia atual) {
        itemAtual = atual;
    }
    public transferencia getAtual()
    {
        return itemAtual;
    }

    public void trocaItens(Transform segurado, Transform destino) {
        Transform aux;
        Item presente = destino.GetComponent<Item>();
        Item entrando = segurado.GetComponent<Item>();

        if (presente != null && presente.id == entrando.id)
        {


            presente.qtd += entrando.qtd;
            if (presente.qtd > entrando.qtdMax)
            {

                //altero a qtd do presente e salvo ele no slot dele msmo, e manter o atual no index anterior e colocar a qtd subitraida
                presente.qtd = entrando.qtdMax;
                //atualizar texto do presente

                //coloca o gameobject no vetor
                itens[presente.index] = presente.gameObject;

                //atualiza qtd do entrando, atualiza texto do entrando
                //volta o gameobject para a posicao anterior 
                entrando.qtd = presente.qtd - entrando.qtdMax;
                itens[entrando.index] = entrando.gameObject;

                if (entrando.index < 10 && entrando.index > -1)
                {
                    segurado.SetParent(barraFullTime);
                    aux = barraFullTime.GetChild(entrando.index);
                    aux.SetParent(null);
                    Destroy(aux.gameObject);
                    segurado.SetSiblingIndex(entrando.index);
                }
                else if(entrando.index >-1){
                    segurado.SetParent(estoquePrincipal);
                    aux = estoquePrincipal.GetChild(entrando.index);
                    aux.SetParent(null);
                    Destroy(aux.gameObject);
                    segurado.SetSiblingIndex(entrando.index);
                }

                if (entrando.qtd > 0)
                {
                    entrando.gameObject.GetComponentInChildren<Text>().text = "." + entrando.qtd;
                }
                else
                {
                    entrando.gameObject.GetComponentInChildren<Text>().text = "";
                }
            }
            presente.qtd = entrando.qtdMax;
            itens[presente.index] = presente.gameObject;
       
            if (presente.qtd > 0)
            {
                presente.gameObject.GetComponentInChildren<Text>().text = "." + presente.qtd;
            }
            else
            {
                presente.gameObject.GetComponentInChildren<Text>().text = "";
            }

        }
        else
        {
            //So troca os dois de posicao
            aux = segurado;
            itens[entrando.index] = destino.gameObject;
            itens[presente.index] = aux.gameObject;
            
        }
    }

    public void addItem(GameObject item) {

        Item entrando;
        Item presente = null;

        GameObject itemAux ;
        entrando = item.GetComponent<Item>();

        if ((firstEmpty >= 10 && firstEmpty < 40) || (entrando.index >= 10 && firstEmpty == -1)) 
        {
            
            if (entrando.index >= 0)
            {
                presente = itens[entrando.index].GetComponent<Item>();
            }

            if (presente != null && presente.id == entrando.id && entrando.index >= 0)
            {


                presente.qtd += entrando.qtd;
                if (presente.qtd > entrando.qtdMax)
                {
                    if (firstEmpty >= 10 && firstEmpty < 40)
                    {
                        itemAux = Instantiate(item, estoquePrincipal.position, Quaternion.identity) as GameObject;

                        itemAux.transform.SetParent(estoquePrincipal);
                        Transform retirando = estoquePrincipal.GetChild(firstEmpty - 10);

                        
                        retirando.SetParent(null);
                        Destroy(retirando.gameObject);
                        itemAux.transform.SetSiblingIndex(firstEmpty - 10);

                        itemAux.GetComponent<Item>().qtd = presente.qtd - entrando.qtdMax;




                        itens[firstEmpty] = null;

                        itens[firstEmpty] = itemAux;

                        entrando.index = firstEmpty;
                        item.GetComponent<Item>().index = firstEmpty;

                        if (itemAux.GetComponent<Item>().qtd > 0)
                        {
                            itemAux.GetComponentInChildren<Text>().text = "." + item.GetComponent<Item>().qtd;
                        }
                        else
                        {
                            itemAux.GetComponentInChildren<Text>().text = "";
                        }
                        firstEmpty++;
                    }
                    else {
                        item.GetComponent<Item>().index = -1;
                    }
                    presente.qtd = entrando.qtdMax;
                }
                if (presente.qtd > 0)
                {
                    presente.gameObject.GetComponentInChildren<Text>().text = "." + presente.qtd;
                }
                else
                {
                    presente.gameObject.GetComponentInChildren<Text>().text = "";
                }
               
            }
            else
            {
                itemAux = Instantiate(item, estoquePrincipal.position, Quaternion.identity) as GameObject;
                
                
                if (item.GetComponent<Item>().qtd > 0)
                {
                    itemAux.GetComponentInChildren<Text>().text = "." + item.GetComponent<Item>().qtd;
                }
                else
                {
                    itemAux.GetComponentInChildren<Text>().text = "";
                }

                itemAux.transform.SetParent(estoquePrincipal);
                Transform retirando = estoquePrincipal.GetChild(firstEmpty - 10);
                retirando.SetParent(null);
                Destroy(retirando.gameObject);
                itemAux.transform.SetSiblingIndex(firstEmpty - 10);

                itens[firstEmpty] = null;

                itens[firstEmpty] = itemAux;

                entrando.index = firstEmpty;
                item.GetComponent<Item>().index = firstEmpty;

                firstEmpty++;
            }
            if (firstEmpty == 40) {
                firstEmpty = -1;
            }
        }
        else if ((firstEmpty>=0 && firstEmpty < 10) || (entrando.index >= 0 && firstEmpty ==-1) )
        {
            
            entrando = item.GetComponent<Item>();
            if (entrando.index >= 0)
            {
                presente = itens[entrando.index].GetComponent<Item>();
            }

            if (presente != null && presente.id == entrando.id && entrando.index >= 0)
            {
                

                presente.qtd += entrando.qtd;
                if (presente.qtd > entrando.qtdMax)
                {
                    if (firstEmpty >= 0 && firstEmpty < 40)
                    {
                        itemAux = Instantiate(item, barraFullTime.position, Quaternion.identity) as GameObject;

                        itemAux.transform.SetParent(barraFullTime);
                        Transform retirando = barraFullTime.GetChild(firstEmpty);
                        retirando.SetParent(null);
                        Destroy(retirando.gameObject);
                        itemAux.transform.SetSiblingIndex(firstEmpty);

                        itemAux.GetComponent<Item>().qtd = presente.qtd - entrando.qtdMax;




                        itens[firstEmpty] = null;

                        itens[firstEmpty] = itemAux;
                        entrando.index = firstEmpty;
                        item.GetComponent<Item>().index = firstEmpty;

                        if (itemAux.GetComponent<Item>().qtd > 0)
                        {
                            itemAux.GetComponentInChildren<Text>().text = "." + item.GetComponent<Item>().qtd;
                        }
                        else
                        {
                            itemAux.GetComponentInChildren<Text>().text = "";
                        }

                        firstEmpty++;
                    }
                    else {
                        item.GetComponent<Item>().index = -1;
                    }
                    presente.qtd = entrando.qtdMax;
                }
                presente.gameObject.GetComponentInChildren<Text>().text = "." + presente.qtd;
            }
            else
            {
                itemAux = Instantiate(item, barraFullTime.position, Quaternion.identity) as GameObject;

                if (item.GetComponent<Item>().qtd > 0)
                {
                    itemAux.GetComponentInChildren<Text>().text = "." + item.GetComponent<Item>().qtd;
                }
                else
                {
                    itemAux.GetComponentInChildren<Text>().text = "";
                }
                

                itemAux.transform.SetParent(barraFullTime);

                Transform retirando = barraFullTime.GetChild(firstEmpty);
                retirando.SetParent(null);
                Destroy(retirando.gameObject);
                itemAux.transform.SetSiblingIndex(firstEmpty);

                itens[firstEmpty] = null;

                itens[firstEmpty] = itemAux;
                entrando.index = firstEmpty;
                item.GetComponent<Item>().index = firstEmpty;
                firstEmpty++;
            }
        }
        else {
            print("Inventario cheio");
        }
    }


    

}
