using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalCamera : MonoBehaviour
{

	private Transform playerCamera;
	public Transform portal;
	public Transform otherPortal;

    private void Awake()
    {
		playerCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
	{
		//So funciona com ambos paralelos, lem q pensar em algo pra funcinar com rotacoes variadas e tanto o offset quanto o direction
		
		Vector3 playerOffsetFromPortal =  playerCamera.position - otherPortal.position;

		if (portal.eulerAngles.y == 90 || otherPortal.eulerAngles.y == 90)
		{

			playerOffsetFromPortal = new Vector3(playerOffsetFromPortal.z, playerOffsetFromPortal.y, playerOffsetFromPortal.x);
		} else if (portal.eulerAngles.y == -90 || otherPortal.eulerAngles.y == -90) {
			playerOffsetFromPortal = new Vector3(playerOffsetFromPortal.z*-1, playerOffsetFromPortal.y, playerOffsetFromPortal.x);
		}

		transform.position = portal.position + playerOffsetFromPortal;

		float angularDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

		Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
		Vector3 newCameraDirection = portalRotationalDifference * playerCamera.forward;
		transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
		
	}
}