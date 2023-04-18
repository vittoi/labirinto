using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{

	private Transform player;
	public Transform receiver;
	private GameObject[] cams;

	private bool playerIsOverlapping = false;
	public float timeToTeleportAgain = 5;
	private static float timeto = 0;

    void Start()
    {
		player = GameObject.Find("Player").transform.GetChild(0);
		cams = GameObject.FindGameObjectsWithTag("cams");
	}
    void Update()
	{

		if (timeto > 0)
		{
			timeto -= Time.deltaTime;
		}

		if (playerIsOverlapping && timeto <= 0)
		{
			
			Vector3 portalToPlayer = player.position - transform.position;
			float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);
			timeto = timeToTeleportAgain;

			// If this is true: The player has moved across the portal
			if (dotProduct < 0f)
			{
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
				rotationDiff += 180;
				player.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.position = receiver.position + positionOffset + receiver.forward * -1;
				player.forward = receiver.forward * -1;
				
				foreach (GameObject cam in cams)
				{
					cam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = receiver.eulerAngles.y+180;
				}

				playerIsOverlapping = false;

			}

		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			playerIsOverlapping = false;
		}
	}
}