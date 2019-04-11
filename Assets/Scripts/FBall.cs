using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBall : MonoBehaviour, IRestarted
{
    public Action<FGate> OnGoal;
    public Rigidbody Rigidbody;
    private Vector3 startPos;
    
    public void SaveStartState()
    {
        startPos = transform.position;
    }

    public void RestartStartState()
    {
        transform.position = startPos;
        StopPhysics();
    }

    public void StopPhysics()
    {
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        var fGate = other.gameObject.GetComponent<FGate>();
        if(fGate != null && OnGoal != null)
            OnGoal.Invoke(fGate);
    }
}
