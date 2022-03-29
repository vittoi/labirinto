using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject canvas;
    // Start is called before the first frame update
    public void ativarDestino(GameObject destino)
    {
        destino.SetActive(true);
    }

    public void desativarAtual(GameObject a) {
        a.SetActive(false);
    }
    
}
