using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //public Transform target;
    public float t;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        /*if(Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 200.0f))
        {
            if (hit.transform != null)
            {
                  target= hit.transform; 
            }
        }
        

        }*/
       /* Vector3 a = transform.position;
        Vector3 b = target.position;
        transform.position = Vector3.MoveTowards(a,Vector3.Lerp(a,b,t),speed);*/
    }
    public void Goto(Vector3 b) {
        {
             
             transform.position = b; /*Vector3.MoveTowards(a,Vector3.Lerp(a,b,t),speed);*/
        }
    }
}
