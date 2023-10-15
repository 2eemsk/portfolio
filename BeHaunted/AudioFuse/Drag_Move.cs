using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag_Move : MonoBehaviour
{
    //[SerializeField] AudioFuseManager afm;

    [SerializeField] private bool oncollider = false;
    [SerializeField] private bool setPos = false;
    [SerializeField] CheckCollider _cc;

    public bool Oncollider { get => oncollider; set => oncollider = value; }
    // Start is called before the first frame update
    void Start()
    {

    }


    public void OnMouseDrag()
    {
        if (!oncollider)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7.3f);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(transform.position.x, objPosition.y, objPosition.z);
        }
    }

    /*public void OnTriggerEnter(Collider other)
    {
        //Invoke("setPosition", 1.0f);
        /*if (setPos)
        {
            transform.position = new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z);
            afm.count++;
            if (transform.position.y == other.transform.position.y)
            {
                oncollider = true;
            }
        }
        transform.position = new Vector3(transform.position.x, other.transform.position.y, other.transform.position.z);
        afm.count++;
        if (transform.position.y == other.transform.position.y)
        {
            oncollider = true;
        }

    }*/

    void setPosition()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
