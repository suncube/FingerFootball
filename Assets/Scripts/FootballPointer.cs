using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballPointer : MonoBehaviour
{
  //  public Transform PowerView;
    public string RaycastMask = "PointerRaycast";

    public Transform Pointer;
    // связать с вьюшкой

    // make auto
    private float MaxViewForce = 5;
    public Vector2 DirPoint;

    public float PowerPercent { get; private set; }

    private FootballPlayer currentPlayer;
    public void AddToPlayer(FootballPlayer player)
    {
        gameObject.SetActive(true);
        currentPlayer = player;
        transform.position = currentPlayer.transform.position;
    }

    public void RemoteFromPlayer(FootballPlayer player)
    {
        currentPlayer = null;
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(currentPlayer == null) return;

        transform.position = currentPlayer.transform.position;
    }


    public void SetMoveDir(Vector3 targetPoint)
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layer_mask = LayerMask.GetMask(RaycastMask);

        if (Physics.Raycast(ray, out hit, 100, layer_mask))
        {
            Pointer.position = hit.point;

            var vector3 = transform.position - Pointer.position;
            vector3.y = 0;
            transform.LookAt(hit.point);

            //vector3
            PowerPercent = Mathf.Min(MaxViewForce, vector3.magnitude) / MaxViewForce;

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, PowerPercent);
        }
    }
}


