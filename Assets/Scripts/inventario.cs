using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventario : MonoBehaviour
{
    public GameObject emptyBt;
    public RectTransform barraFullTime;
    public RectTransform estoquePrincipal;
    public List<GameObject> itemTest = new List<GameObject>();

    private List<int> itemId = new List<int>();
    private List<GameObject> bts = new List<GameObject>();//Lista com os ids dos itens, e o index é onde ele está no inventario
    private tipAction tip;
    public struct transferencia {
        public int index;
        public Transform pai;
    };
    

    private int firstEmpty = 0;
    private transferencia itemAtual;
    private bool bolsa = false;
    // Start is called before the first frame update
    void Awake(){
        initializeLists();
        tip = GameObject.Find("tipAction").GetComponent<tipAction>();
    }
    void Start()
    {
        itemAtual.index = -1;
        desenhaMenu();
    }
    void initializeLists(){
        for(int i = 0; i < 40; i++){
            itemId.Add(-1);

            bts.Add(Instantiate(emptyBt));
            Transform parent = i<10 ? barraFullTime : estoquePrincipal;
            bts[i].transform.SetParent(parent);
            bts[i].name = "Slot"+ i;
            bts[i].transform.localScale = new Vector3(1,1,1);
        }   
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J)){
            
            addItem(Instantiate(itemTest[1]));
        }
        if(Input.GetKeyDown(KeyCode.Q)){
            
            addItem(Instantiate(itemTest[2]));
        }
        if(Input.GetKeyDown(KeyCode.I)){
            estoquePrincipal.transform.parent.parent.gameObject.SetActive(!bolsa);
            bolsa = !bolsa;
        }
    }

    void desenhaMenu()
    {
        while (barraFullTime.childCount<10) {
            
            GameObject item = Instantiate(bts[barraFullTime.childCount], barraFullTime.position, Quaternion.identity) as GameObject;
            item.transform.GetChild(0).GetComponentInChildren<Text>().text = "";

            bts[barraFullTime.childCount] = item;

            item.transform.SetParent(barraFullTime);
            

        }
        while (estoquePrincipal.childCount< 30) {
            GameObject item = Instantiate(bts[estoquePrincipal.childCount+10], estoquePrincipal.position, Quaternion.identity) as GameObject;
            
            item.transform.GetChild(0).GetComponentInChildren<Text>().text = "";
            

            bts[estoquePrincipal.childCount+10] = item;

            item.transform.SetParent(estoquePrincipal);
        }
    }
    
    public int getIndexByGameObject(Transform slot){
        if(slot.parent == barraFullTime){
            return slot.GetSiblingIndex();
        }
        return slot.GetSiblingIndex() + 10;
    
    }
    public void setBtnOnIndex(GameObject btn, int index){
        bts[index] = btn;
    }

    public void trocaItens(GameObject segurado, GameObject destino, GameObject origem) { //Slot que ocupara, slot antigo item segurado
        GameObject itemOnDest = destino.transform.childCount>1 ? destino.transform.GetChild(1).gameObject : null;
        print(destino.name);
        ItemEx itemSeg = segurado.GetComponent<ItemEx>();
        ItemEx itemDest = itemOnDest!= null? itemOnDest.GetComponent<ItemEx>(): null;
        int iOrig = getIndexByGameObject(origem.transform);
        int iDest = getIndexByGameObject(destino.transform);
             
        if(itemOnDest!= null){
            
            int qtd = itemSeg.qtd + itemDest.qtd;
            if(itemDest.id == itemSeg.id){
                if(qtd > itemSeg.qtdMax){
                    oculpaEspacoComItem(iOrig, qtd - itemSeg.qtdMax, segurado);
                    qtd = itemSeg.qtdMax;
                    
                }else{
                    oculpaEspacoComVazio(iOrig);
                }
                atualizaQtd(iDest, qtd);
                
            } else{
                oculpaEspacoComItem(iOrig, itemDest.qtd, itemOnDest);
                oculpaEspacoComItem(iDest, itemSeg.qtd, segurado);
            }
        }else{
            itemId[iOrig] = -1;
            oculpaEspacoComItem(iDest, itemSeg.qtd, segurado);
        }
        
    }

    public void addItem(GameObject item) {
        firstEmpty = -1;
        ItemEx entrando;
        ItemEx presente = null;

        entrando = item.GetComponent<ItemEx>();
        List<int> allPos = itemId.FindAll(i => entrando.id == i);

        firstEmpty = itemId.IndexOf(-1);
        int ultimoVisto = 0;

        if(allPos.Count > 0){
            for(int i=0; i< allPos.Count; i++){
                
                int position = itemId.IndexOf(entrando.id, ultimoVisto);
                ultimoVisto = position+1;

                presente = bts[position].transform.GetChild(1).GetComponent<ItemEx>();
                int qtd = presente.qtd<entrando.qtdMax ? entrando.qtd+ presente.qtd : entrando.qtd;

                if(presente.qtd < entrando.qtdMax){
                    if(qtd>= entrando.qtdMax){
                        entrando.qtd = qtd - entrando.qtdMax;
                        atualizaQtd(position, entrando.qtdMax);
                        
                    }else{
                        entrando.qtd = 0;
                        atualizaQtd(position, qtd);
                    }
                }
                

            }
            if(entrando.qtd>0 && firstEmpty>-1){

                oculpaEspacoComItem(firstEmpty, entrando.qtd, Instantiate(item));
            }
            
            if(firstEmpty == -1){
                tip.showText("\" Inventário Cheio\"");
                tip.fadeOut();
                
            }
            Destroy(item);
        }else{
            oculpaEspacoComItem(firstEmpty, entrando.qtd, item);
        }

    }
    //TODO trocar prints por Text
    //TODO trocar destroy por animacoes
    public bool remove(int id, int qtd){
        List<int> allPos = itemId.FindAll(i => id == i);
        List<int> toSetEmpty = new List<int>();
        int ultimoVisto = 0;
        
        if(allPos.Count > 0){
            for(int i=0; i< allPos.Count; i++){
                
                int position = itemId.IndexOf(id, ultimoVisto);
                ultimoVisto = position+1;
                ItemEx item = bts[position].transform.GetChild(1).GetComponent<ItemEx>();

                if(item.qtd- qtd > 0){
                    atualizaQtd(position, item.qtd- qtd);
                    qtd = item.qtd- qtd;
                }else if(qtd > 0 && item.qtd - qtd<= 0){//todos os slots q vao zerar
                    toSetEmpty.Add(position);
                    qtd = qtd - item.qtd;
                }else{
                    print("nao tem o item");
                }

            }
            if(qtd <= 0 ){//Se sumiu toda a qtd foi cobrado correto
                foreach(int position in toSetEmpty){
                    esvaziaSlot(bts[position]);
                }
                return true;
            }else{
                print("Nao tem a quantidade necessaria");
            }
        }
        
        return false;
        
    }
    public GameObject getEmptyBt(){
        return emptyBt;
    }
    private void atualizaQtd(int index, int qtd){
        bts[index].transform.GetChild(1).GetComponent<ItemEx>().qtd = qtd;
        bts[index].transform.GetChild(0).GetComponentInChildren<Text>().text = "." + qtd;
    }
    private void oculpaEspacoComItem(int index, int qtd, GameObject item){
        item.transform.SetParent(bts[index].transform);
        item.transform.SetAsLastSibling(); 
        item.GetComponent<itemFunctions>().enabled = false;
        item.transform.GetChild(0).gameObject.SetActive(false);
        ItemEx btsItem = item.GetComponent<ItemEx>();//Verificar com quantos components ficaram no slot

        if(qtd > 0){
            atualizaQtd(index, qtd);
            bts[index].GetComponent<Image>().sprite = btsItem.icone;
        }else{
            bts[index].transform.GetChild(0).GetComponentInChildren<Text>().text = "";
        }
        itemId[index] = btsItem.id;
        bts[index].GetComponent<seguraItem>().enabled = true;

    }
    public void esvaziaSlot(GameObject slot){
        int i = getIndexByGameObject(slot.transform);
        oculpaEspacoComVazio(i);
        if(slot.transform.childCount>1)
            Destroy(slot.transform.GetChild(1).gameObject);
    }

    private void oculpaEspacoComVazio(int index){
        itemId[index] = -1;
        bts[index].GetComponent<Image>().sprite = emptyBt.GetComponent<Image>().sprite;
        bts[index].GetComponent<seguraItem>().enabled = false;
        bts[index].transform.GetChild(0).GetComponentInChildren<Text>().text = "";

    }
    

}

 