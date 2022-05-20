using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocksAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    List<GameObject> rocksPart = new List<GameObject>();
    public List<float> speed;
    void Awake()
    {
        int n = this.transform.childCount;
        for(int i =0; i< n; i++){

            rocksPart.Add(this.transform.GetChild(i).gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        int n = this.transform.childCount;
        for(int i =0; i< n; i++){
            float y = Mathf.PingPong(Time.time * speed[i], 1) * 6 ;
            rocksPart[i].transform.localPosition = new Vector3(0, y, 0);   
        }
    }
}
