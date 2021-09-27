using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public Transform moveLocation;

    public LayerMask StopsUnit;
    // Start is called before the first frame update
    void Start()
    {
        moveLocation.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveLocation.position, moveSpeed*Time.deltaTime);
        if(Vector3.Distance(transform.position,moveLocation.position)<=.05f) 
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(moveLocation.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, StopsUnit))
                {
                    moveLocation.position += new Vector3(Input.GetAxisRaw("Horizontal")*2, 0f, 0f);
                }
            }
            if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(moveLocation.position + new Vector3(0f,Input.GetAxisRaw("Vertical"), 0f), .2f, StopsUnit))
                {
                    moveLocation.position += new Vector3(0f, Input.GetAxisRaw("Vertical")*2, 0f);
                }
            }
        }
    }

}
