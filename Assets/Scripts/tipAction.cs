using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tipAction : MonoBehaviour
{
    private bool visivel = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void showText(string text){
        if(!visivel){
            fadeIn();
            visivel = true;
        }
        GetComponent<Text>().text = text;
    }
    private IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public void fadeOut(){
        if(visivel){
            StartCoroutine(FadeTextToZeroAlpha(2f, GetComponent<Text>()));
            visivel = false;
        }
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public void fadeIn(){
        StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<Text>()));
    }
}
