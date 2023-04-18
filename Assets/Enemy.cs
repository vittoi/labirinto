using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float timeToRetrace = 5;
    private Transform player;
    public bool started = false;

    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = Manager.Instance.player;
        

    }

    // Update is called once per frame
    void Update()
    {
        if(Manager.Instance.startLab)
            findMainCharacter();
        
    }
    private void findMainCharacter() {
        if (timeToRetrace < 0)
        {
            agent.enabled = true;
            timeToRetrace = 5;
            agent.isStopped = false;
            agent.destination = player.position;
            started = true;
        } else if (!started) {
            float step = 1 * Time.deltaTime;
            Vector3.MoveTowards(transform.position, new Vector3(0, 2, 0), step);
        }
        if (Vector3.Distance(this.transform.position, player.position) <= 10 && started)
        {
            Manager.Instance.messageFull.showText("Voce Morreu");
            Manager.Instance.RestartButton.SetActive(true);
            
            
            started = false;
            Time.timeScale = 0;
        }

        timeToRetrace -= Time.deltaTime;
        //TODO print time to reset em algum lugar da tela, talvez seja legal ele ir diminuindo ate 2 e ficar perseguindo com mais pavor
    }

    public void killEnemy() {
        gameObject.SetActive(false);
        transform.position = new Vector3(0, 2, 0);
        agent.speed *= 1.5f;
        gameObject.SetActive(true);
    }
}
