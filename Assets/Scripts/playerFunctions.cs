using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class playerFunctions : MonoBehaviour
{
    public GameObject itemTeste;
    public Vector3 goTo;
    public bool go;
    private bool started;

    public GameObject aimCam;
    public GameObject mCam;
    public GameObject aimSprite;
    private Transform body;

    private Rigidbody rb;
    
    //TODO quando apertar esc aparece o cursor 
    private bool m_cursorIsLocked = true;

    void Awake()
    {
        body = this.transform.GetChild(0);
        rb = body.GetComponent<Rigidbody>();
        
    }

    void Update()
    {
        InternalLockUpdate();
        if (body.position.y < -1) {
            body.position += new Vector3(0,3,0);
        }

        goToAimCam();
        lockTheRangeOfAimCam();
    }

    private void lockTheRangeOfAimCam(){

        aimCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxValue = body.rotation.eulerAngles.y +90;
        aimCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MinValue = body.rotation.eulerAngles.y -90;
    }
    private void goToAimCam(){
        if(Input.GetMouseButtonDown(1)){
            aimCam.SetActive(true);
            aimCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = body.rotation.eulerAngles.y;
            aimCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 0;
            aimSprite.SetActive(true);
        }
        if(Input.GetMouseButtonUp(1)){
            aimCam.SetActive(false);
            aimSprite.SetActive(false);

            mCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.Value = body.rotation.eulerAngles.y;
            mCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value = 0;
        }
    }
    private void InternalLockUpdate()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

}
