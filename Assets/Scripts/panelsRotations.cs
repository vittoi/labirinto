using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panelsRotations : MonoBehaviour
{
    public GameObject panelsOrigin;
    private Transform[] vetorPanels;
    private int currentPanel = 0;
    // Start is called before the first frame update
    void Start()
    {
        vetorPanels = new Transform[panelsOrigin.transform.childCount];
        for (int i = 0; i < panelsOrigin.transform.childCount; i++)
        {
            
            vetorPanels[i] = panelsOrigin.transform.GetChild(i);
            if (i > 2)
            {
                vetorPanels[i].gameObject.SetActive(false);
            }
        }

        vetorPanels[0].localPosition = new Vector2(0f, 0);
        vetorPanels[1].localPosition = new Vector2(500f, 0);
        vetorPanels[2].localPosition = new Vector2(1000f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void passaPainel(int direcao) {
        //rotaciona os paineis da loja conforme a direcao
        //O botao da direita(>-1) e o da esquerda(-1)
        float scaleVariation = -40;

        if (direcao == -1)
        {
            if (currentPanel > 0)
            {
                int lastOnScreen = currentPanel + 1;
                if (currentPanel + 2 < vetorPanels.Length)
                    vetorPanels[currentPanel + 2].gameObject.SetActive(false);
                float lastPosition = 1000;
                float variation = 0.8f;

                for (int i = 0; i < 5; i++)
                {
                    if (lastOnScreen > -1 && lastOnScreen < vetorPanels.Length)
                    {
                        vetorPanels[lastOnScreen].localPosition = new Vector2(lastPosition, 0);
                        vetorPanels[lastOnScreen].transform.localScale = new Vector2(variation, variation+0.4f);

                    }
                    lastPosition -= 500;
                    
                    variation = i<2 ? variation += 0.1f : variation -= 0.1f; ;
                    
                    scaleVariation += 20;
                    lastOnScreen--;
                }
                currentPanel--;
                if (currentPanel - 2 >-1)
                {
                    vetorPanels[currentPanel - 2].gameObject.SetActive(true);
                    vetorPanels[currentPanel - 2].localPosition = new Vector2(-1000, 0);
                }
            }
        }
        else {
            if (currentPanel < vetorPanels.Length-1)
            {

                int firstOnScreen = currentPanel - 1;
                if(currentPanel >= 2)
                    vetorPanels[currentPanel - 2].gameObject.SetActive(false);
                float firstPosition = -1000;
                float variation = 0.8f;

                for (int i = 0; i < 5; i++) {
                    
                    if (firstOnScreen >-1 && firstOnScreen < vetorPanels.Length) {
                        
                        vetorPanels[firstOnScreen].localPosition = new Vector2(firstPosition, 0);
                        vetorPanels[firstOnScreen].transform.localScale = new Vector2(variation, variation + 0.4f);

                    }
                    firstPosition += 500;
                    variation = i < 2 ? variation += 0.1f : variation -= 0.1f; ;
                    firstOnScreen++;
                    
                }
                currentPanel++;
                if (currentPanel + 2 < vetorPanels.Length)
                {
                    vetorPanels[currentPanel + 2].gameObject.SetActive(true);
                    vetorPanels[currentPanel + 2].localPosition = new Vector2(1000, 0);
                }
                

            }
        }
    }
}
