using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SugaFog : ItemEx
{
    public override void handle(GameObject item)
    {
        //TODO Reset labirinth so q tem algo q muda na iluminacao
        //SceneManager.LoadScene("Labirinth", LoadSceneMode.Single);
        //SceneManager.LoadScene("FloatCity", LoadSceneMode.Additive);
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName("Labirinth"));
       
        inv.itemSucessfullyUsed = true;
        item.transform.parent.localScale = new Vector3(1, 1, 1);
        item.transform.parent.position = new Vector3(0, 0, 0);

        item.transform.position = (player.forward * 2) + player.position + new Vector3(0, 0.5f, 0);//aparece na frente do corpo

    }
    public override void runtime()
    {
        if (timeToDesapear >= 0 && iFunctions.isRunningOneItem)
        {
            timeToDesapear -= Time.deltaTime;
            if (timeToDesapear > 12 && RenderSettings.fogDensity > 0.02f)
            {
                RenderSettings.fogDensity -= (Time.deltaTime / 100) * 1.5f;
            }
            else if (timeToDesapear < 12)
            {
                RenderSettings.fogDensity = 0.02f;
            }
        }
        else if (iFunctions.isRunningOneItem)
        {
            iFunctions.finalizaItem(cd);
            timeToDesapear = 17;
        }
        else
        {
            if (iFunctions.timeCd < 0 && RenderSettings.fogDensity < 0.07)
                RenderSettings.fogDensity = 0.07f;
            RenderSettings.fogDensity += (Time.deltaTime / 100) * 1.8f;

        }
    }
}
