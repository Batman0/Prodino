using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventManager: MonoBehaviour 
{
    public UnityEvent changeProspective;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            changeProspective.Invoke();
        }
    }   
}
