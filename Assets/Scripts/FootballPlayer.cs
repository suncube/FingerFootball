using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FootballPlayer : MonoBehaviour, IRestarted
{
    public bool IsCanMoved { get; set; }

    public Action OnMoveEnded;
    // rigidbody + physics settings
    public FootballPlayerSetting Setting;
    public Rigidbody Rigidbody;


    private void Awake()
    {
        Setting.InitializeRigidbody(Rigidbody);
    }

    void Start()
    {
        IsCanMoved = true;

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Drag;
        entry.callback.AddListener((data) => { OnDragDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        if(!IsCanMoved) return;

      //  Debug.Log("OnPointerDownDelegate called.");
        FootballManager.runtime.FootballPointer.AddToPlayer(this);
    }
    
    public void OnPointerUpDelegate(PointerEventData data)
    {
        if (!IsCanMoved) return;

        FootballManager.runtime.FootballPointer.RemoteFromPlayer(this);

        AddForce();

     //   Debug.Log("OnPointerUpDelegate called. " + FootballManager.runtime.FootballPointer.PowerPercent);

        if(OnMoveEnded != null)
            OnMoveEnded.Invoke();
    }

    public void OnDragDelegate(PointerEventData data)
    {
        if (!IsCanMoved) return;

        FootballManager.runtime.FootballPointer.SetMoveDir(data.position);
    }

    private void AddForce()
    {
        // make property to Pointer
        var pointerPosition = FootballManager.runtime.FootballPointer.Pointer.position;
        var vector3 = pointerPosition - transform.position;

        Rigidbody.AddForce(vector3.normalized * Setting.MaxForce * FootballManager.runtime.FootballPointer.PowerPercent);
        //Rigidbody.AddForce(vector3.normalized * Setting.Force, ForceMode.Impulse);
        //Rigidbody.AddForce(vector3.normalized * Setting.Force, ForceMode.Force);
    }

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
}
