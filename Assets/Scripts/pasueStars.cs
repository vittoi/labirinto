using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pasueStars : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        Invoke("pause", 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void pause() {
        GetComponent<ParticleSystem>().Pause();
    }
}
