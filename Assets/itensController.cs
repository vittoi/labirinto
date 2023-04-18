using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itensController : MonoBehaviour
{
    ItemEx currentItemRunning;
    public bool isRunningOneItem;

    GameObject itemConsume;
    public float timeCd = 0;

    private void Update()
    {
        if(currentItemRunning !=null) {  
            currentItemRunning.runtime();
     
        }

        if (!isRunningOneItem ) {
            if (timeCd <= 0)
            {
                currentItemRunning = null;
            }
            else
            {
                timeCd -= Time.deltaTime;
            }
        }

    }

    public void finalizaItem(float cd) {
        isRunningOneItem = false;
        timeCd = cd;
        if(itemConsume)
            Destroy(itemConsume.transform.parent.gameObject);
    }

    public bool acionaItem(ItemEx i, Transform item) {
        if (i.id >= 100)
        {   
            currentItemRunning = i;
            if (!isRunningOneItem)
            {
                itemConsume = Instantiate(item.gameObject).transform.GetChild(0).gameObject;
                itemConsume.SetActive(true);

                i.handle(itemConsume);

                isRunningOneItem = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
