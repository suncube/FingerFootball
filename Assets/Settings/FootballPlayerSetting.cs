using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FootballPlayerSetting", menuName = "Football")]
public class FootballPlayerSetting : ScriptableObject
{
    public float MaxForce = 1f;

    [SerializeField]
    private float m_Mass = 1f;
    [SerializeField]
    private float m_Drag = 0.5f;
    [SerializeField]
    private float m_AngularDrag = 0.5f;

    public void InitializeRigidbody(Rigidbody rigidbody)
    {
        rigidbody.mass = m_Mass;
        rigidbody.drag = m_Drag;
        rigidbody.angularDrag = m_AngularDrag;
    }


}
