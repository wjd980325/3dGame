using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class DropItem : MonoBehaviour
{
    private SphereCollider col;
    public SphereCollider Col
    {
        get
        {
            if(col)
                return col;
            else
            {
                TryGetComponent<SphereCollider>(out col);
                return col;
            }
        }
    }
    private Rigidbody rig;
}
