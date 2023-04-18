using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CipoAux : MonoBehaviour
{
    private Cipo parent;
    public bool started;
    public bool go;
    private Rigidbody rb;
    private Transform mCamera;
    public Vector3 goTo;
    private LineRenderer lr;
    private Vector3 grapplePoint;
    private Vector3 pontoLancamento;
    private float maxDistance = 100f;
    private SpringJoint joint;
    private Vector3 whereHit;
    private Vector3 currentGrapplePosition;
    private Transform player;


    private void OnTriggerEnter(Collider other)
    {
        parent.changeProps(other.gameObject.layer);
        if (other.gameObject.layer == 7)
        {
            go = true;
            goTo = whereHit = transform.position;
        }
        rb.constraints = RigidbodyConstraints.FreezePosition;
        GetComponent<MeshRenderer>().enabled = false;

    }
    private void Update()
    {
        if (transform.position.y < -1)
            parent.changeProps(8);
    }

    private void LateUpdate()
    {
        DrawRope();
    }
    private void Awake()
    {
        player = Manager.Instance.player;
        pontoLancamento = (player.forward) + player.position + new Vector3(0, 2f, 0);
        mCamera = Camera.main.transform;
        parent = transform.parent.GetComponent<Cipo>();
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
    }

    public void dispara()
    {
        transform.GetComponent<Rigidbody>().AddForce(mCamera.forward * parent.forcaArremesso, ForceMode.Impulse);

    }

    public void startGrapple()
    {
        if (Vector3.Distance(pontoLancamento, whereHit) <= maxDistance)
        {
            grapplePoint = whereHit;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = player.position + new Vector3(0, 2f, 0);
        }
    }

    public void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        lr.SetPosition(0, player.position + new Vector3(0, 2f, 0));//TODO Aqui tem q ser a mao do cavalheiro
   
        lr.SetPosition(1, currentGrapplePosition);

    }
    public void StopGrapple()
    {
        if (lr)
            lr.positionCount = 0;
        Destroy(joint);
        //Destroy(this.transform.parent.gameObject);
    }
}
