using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get; private set; }

    public bool startLab = false;
    public Transform player;
    public Transform enemy;
    public Transform Map;

    public Transform messageFullT;
    public Transform messageTipT;
    public TextsAction messageFull;
    public TextsAction messagetip;

    public GameObject RestartButton;
    public GameObject initPortalPrefab;
    private GameObject initPortal;
    Manager()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        

    }

    private void Awake()
    {
        messageFull = new TextsAction();
        messagetip = new TextsAction();

        initPortal = Instantiate(initPortalPrefab);
        initPortal.SetActive(true);

        messageFull.setObjText(messageFullT);
        messagetip.setObjText(messageTipT);

    }

    private void Update()
    {
        if (player.position.y <= 1)
        {
            startLab = true;
            //RenderSettings.fog = true;
            Destroy(initPortal);
        }

        


        if (Input.GetKeyDown(KeyCode.R)) {
            Instance.GetComponent<MakeLevel>().shuffleLab();
        }
        //if () { //Se o player estiver fora das dimensoes do labirinto ele ganha
        
        //}
    }

    public void setMap(Transform map) {
        this.Map = map;
    }

    public void restartGame() {
        if (CursorLockMode.Locked == Cursor.lockState)
        {
            Cursor.visible = true;
            Cursor.lockState = Cursor.lockState = CursorLockMode.None;
        }

        RenderSettings.fog = false;

        player.position = new Vector3(45, 150, 220);

        Instance.enemy.gameObject.SetActive(false);
        
        Instance.enemy.GetComponent<Enemy>().timeToRetrace = 10;
        Instance.enemy.GetComponent<Enemy>().started = false;
        Instance.enemy.GetComponent<Enemy>().agent.enabled = false;
        Instance.enemy.position = new Vector3(0, 5, 0);

        Instance.enemy.gameObject.SetActive(true);
        

        Time.timeScale = 1;
        
        Instance.messageFull.fadeOut();
        Instance.startLab = false;
        initPortal = Instantiate(initPortalPrefab);
        initPortal.SetActive(true);
        Instance.RestartButton.SetActive(false);

        Instance.GetComponent<MakeLevel>().shuffleLab();
       
    }

}
