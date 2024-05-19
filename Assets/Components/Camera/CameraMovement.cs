using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float borderX;
    public float borderY;
    public float minusBorderY;
    
    [SerializeField]private Vector3 offset;
    [SerializeField]private float damping;
    [SerializeField] private Transform startPos;

    public float speed;
    private Vector3 vel=Vector3.zero;
    void Start()
    {
        transform.position = startPos.position+offset;
    }


    void Update()
    {
        RestrictCamera();
        
    Vector3 movementInput=new Vector3(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"),0);
    movementInput.Normalize();
    Vector3 targetPosition=transform.position+movementInput*speed;
    transform.position=Vector3.SmoothDamp(transform.position,targetPosition,ref vel,damping);        
    }

    void RestrictCamera()
    {
        if (transform.position.x > borderX)
        {
            transform.position = new Vector3(borderX,transform.position.y,transform.position.z);
        }
        else if(transform.position.x < borderX * -1)
        { 
            transform.position = new Vector3(borderX * -1, transform.position.y, transform.position.z);
        }

        if(transform.position.y > borderY)
        {
            transform.position = new Vector3(transform.position.x, borderY, transform.position.z);
        }
        else if (transform.position.y < minusBorderY)
        {
            transform.position = new Vector3(transform.position.x, minusBorderY, transform.position.z);
        }
    }
}
