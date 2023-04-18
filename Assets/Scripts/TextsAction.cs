using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextsAction : MonoBehaviour
{
    private bool visivel = true;
    private Transform objText;

    public void setObjText(Transform objText)
    {
        this.objText = objText;
    }

    public void showText(string text){
        
        objText.GetComponent<Text>().text = text;
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
        if (visivel){
            Manager.Instance.StartCoroutine(FadeTextToZeroAlpha(2f, objText.GetComponent<Text>()));
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
        Manager.Instance.StartCoroutine(FadeTextToFullAlpha(1f, objText.GetComponent<Text>()));
    }
}
