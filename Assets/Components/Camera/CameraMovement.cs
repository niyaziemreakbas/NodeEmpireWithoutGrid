using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    [SerializeField]private Vector3 offset;
    [SerializeField]private float damping;
    
    public float speed;
    private Vector3 vel=Vector3.zero;
    void Start()
    {
        
    }


    void Update()
    {
    Vector3 movementInput=new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
    movementInput.Normalize();
    Vector3 targetPosition=transform.position+movementInput*speed;
    transform.position=Vector3.SmoothDamp(transform.position,targetPosition,ref vel,damping);        
    }
}
