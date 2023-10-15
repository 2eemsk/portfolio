using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject target;
    public float moveSpeed;
    private Vector3 targetPosition;

    public BoxCollider2D bound;
    //'bound'라는 카메라 제한 영역 설정

    private Vector3 minBound;
    private Vector3 maxBound;
    //박스 콜라이더 영역의 최소 최대 xyz 값을 줌

    private float halfWidth; //카메라 반 너비 값
    private float halfHeight; //카메라 반 높이 값

    private Camera theCamera;
    //카메라의 반 높이 값을 구할 속성을 이용하기 위한 벼수


    // Start is called before the first frame update
    void Start()
    {
        theCamera = GetComponent<Camera>();

        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;

        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (target.gameObject != null)
        {
            targetPosition.Set(target.transform.position.x+0.28f, target.transform.position.y + 0.46f, -10.0f);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, moveSpeed * Time.deltaTime);


            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
        
    }
}
