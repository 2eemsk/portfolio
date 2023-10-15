using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class cameraController : MonoBehaviour
{
    /*
    public Transform target; // The target object to follow
    public float distance = 25.0f; // Distance from target
    public float height = 5.0f; // Height from target
    public float smoothSpeed = 0.15f; // Speed to follow the target
    public float weird = 3.6f;

    private Vector3 velocity = Vector3.zero; // Velocity for smooth dampening

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + new Vector3(0f, height - weird, 0f); // Add a small y-offset
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition - transform.forward * distance, ref velocity, smoothSpeed);
        transform.position = targetPosition - transform.forward * distance;
    }*/

    [SerializeField] private CinemachineVirtualCamera _cam;

    private void Start()
    {

    }

    // Function to zoom in/out using the mouse wheel
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * -1.5f;

        if (scroll != 0.0f)
        {
            /*var distance = scroll * 0.5f;
            distance = Mathf.Clamp(distance, 12.0f, 20.0f);

            _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = distance;*/

            if(_cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance >= 20f && scroll > 0)
            {
                _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 20f;
            }
            else if(_cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance <= 12f && scroll < 0)
            {
                _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = 12f;
            }
            else
            {
                _cam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance += scroll;
            }

            /*CinemachineComponentBase componentBase = _cam.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (componentBase is CinemachineFramingTransposer)
            {
                (componentBase as CinemachineFramingTransposer).m_CameraDistance = distance; // your value
            }*/
        }
    }

}
