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
}


