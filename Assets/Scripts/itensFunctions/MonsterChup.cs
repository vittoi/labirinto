using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChup : ItemEx
{
    public float activationDistance;//a distancia em metro de onde o item vai nascer
    public float killDistance;
    public float speed = 2;

    protected Transform itemTransform;

    private Vector3 targetPosition;
    private Transform enemy;
    public override void handle(GameObject item) 
    {
 
        inv.itemSucessfullyUsed = true;
        item.transform.parent.localScale = new Vector3(1, 1, 1);
        item.transform.parent.position = new Vector3(0, 0, 0);
        item.transform.position = (player.forward) + player.position + new Vector3(0, 2f, 0);

        //Defini em que distancia o item tem q parar pra comecar a funcionar
        targetPosition = (player.forward *activationDistance) + player.position + new Vector3(0, 4f, 0);
        enemy = Manager.Instance.enemy;

        itemTransform = item.transform;

    }

    public override void runtime()
    {
        
        float step = speed * Time.deltaTime;
        if (itemTransform != null)
        {
            itemTransform.position = Vector3.MoveTowards(itemTransform.position, targetPosition, step);

            if (Vector3.Distance(itemTransform.position, targetPosition) < 0.5f && timeToDesapear > 0)
            {
                //Se Enemy aparecer a killDistance do item ele é sugado
                if (Vector3.Distance(enemy.position, itemTransform.position) <= killDistance)
                {
                    enemy.GetComponent<Enemy>().killEnemy();
                    iFunctions.finalizaItem(cd);
                }
            }
            else if (timeToDesapear <= 0)
            {
                iFunctions.finalizaItem(cd);
            }

            timeToDesapear -= Time.deltaTime;
        }
        
    }
}
