using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itensList : MonoBehaviour
{
    public GameObject[] itens;

    public GameObject getItemByIndex(int i){
        return itens[i];
    }
}