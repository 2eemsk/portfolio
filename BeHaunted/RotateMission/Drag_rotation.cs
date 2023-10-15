using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class Drag_rotation : MonoBehaviour
    {
        [SerializeField] MissionOpen _missionManager;

        private float rotationSpeed = 5.0f;
        float checktime = 0;
        bool missionSuccess = false;

    private void OnMouseDrag()
        {
            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;

            transform.Rotate(Vector3.back, XaxisRotation);
        }

        private void Update()
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 20f, Color.red);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hitInfo, 20f))
            {
                if (hitInfo.collider.tag == ("target"))
                {
                    checktime = checktime + Time.deltaTime;
                    if (checktime >= 2)
                    {
                        //transform.LookAt(hitInfo.collider.transform);  
                        missionSuccess = true;
                        Debug.Log("미션성공");
                        StartCoroutine(_missionManager.MissionClear(6));
                    }
                }
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hitInfo.distance, Color.green);
            }
            else
            {
                checktime = 0;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 20f, Color.red);
            }
        }

    }
}

