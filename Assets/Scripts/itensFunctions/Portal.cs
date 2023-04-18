using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : ItemEx
{
    
    public override void handle(GameObject item)
    {
        inv.itemSucessfullyUsed = true;
        Vector2 cord = map.fromWorldToLab(player.position);
        int line = map.isVerticalOrHorizontal((int)cord.x, (int)cord.y); //1 horizontal, 2 veertical, 0 quina
        if (line == 0)
            line++;

        //sorteia uma posicao na mesma orientacao de onde foi chamado
        bool isValid = false;
        int iDest = 0, jDest = 0;
        while (isValid != true)
        {
            //sorteia i && j
            iDest = (int)Random.Range(0, map.dim);
            jDest = (int)Random.Range(0, map.dim);

            //verifica se e livre
            //verifica se e da mesma line
            if (map.isEmptySpace(jDest, iDest) == 1 && line == map.isVerticalOrHorizontal(iDest, jDest))
            {
                isValid = true;
            }
        }

        //Gera os portais nas cordenadas
        item.transform.parent.position = new Vector3(0, 0, 0);
        item.transform.parent.Rotate(0, 180, 0);
        item.transform.parent.localScale = new Vector3(1, 1, 1);

        item.transform.GetChild(0).position = (player.forward * 2) + player.position + new Vector3(0, 2, 0);//aparece na frente do corpo
        item.transform.GetChild(1).position = map.fromLabToWorld(new Vector2(iDest, jDest));

        if (line == 2)//se for vertical eu rotaciono 90graus
        {
            item.transform.GetChild(0).Rotate(0, 90, 0);
            item.transform.GetChild(1).Rotate(0, 90, 0);
        }
    }
    public override void runtime()
    {
        if (timeToDesapear >= 0 && iFunctions.isRunningOneItem)
        {
            timeToDesapear -= Time.deltaTime;
        }
        else if (iFunctions.isRunningOneItem)
        {
            iFunctions.finalizaItem(cd);
            timeToDesapear = 15;
        }
    }
}
