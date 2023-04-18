using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cipo : ItemEx
{
    public float forcaArremesso;
    private CipoAux cipo;

    public override void handle(GameObject item)
    {
        cipo = item.GetComponent<CipoAux>();

        Collider itemBc = item.GetComponent<Collider>();
        item.transform.parent.localScale = new Vector3(1, 1, 1);
        item.transform.parent.position = new Vector3(0, 0, 0);
        item.transform.position = new Vector3(player.position.x, player.position.y + 2, player.position.z) + player.forward * 2;
        itemBc.isTrigger = true;

        cipo.dispara();
    }

    public void changeProps(int layer) {
        if (layer == 7)
        {
            inv.itemSucessfullyUsed = true;
        }
        else if (layer == 8)
        {
            iFunctions.isRunningOneItem = false;
            iFunctions.finalizaItem(cd);

        }
        
    }

    public override void runtime()
    {
        if (timeToDesapear <= 0 && !cipo.started) {
            iFunctions.finalizaItem(cd);
        }
        if (cipo.started && Input.GetMouseButtonDown(0))
        {
            cipo.StopGrapple();
            cipo.started = false;
            iFunctions.isRunningOneItem = false;
            iFunctions.finalizaItem(cd);
        }

        if (cipo.go)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.useGravity = false;
            Vector3 dir = cipo.goTo - player.position;
            dir.Normalize();
            rb.AddForce((dir * 0.3f), ForceMode.Impulse);
            if (Input.GetMouseButtonDown(0))
            {
                cipo.go = false;
                cipo.startGrapple();
                cipo.started = true;
                rb.useGravity = true;
            }
            else if (Vector3.Distance(player.position, cipo.goTo) < 10 && iFunctions.isRunningOneItem)
            {
                cipo.go = false;
                rb.useGravity = true;
                iFunctions.finalizaItem(cd);
                cipo.StopGrapple();
            }
        }
        timeToDesapear -= Time.deltaTime;
    }
}