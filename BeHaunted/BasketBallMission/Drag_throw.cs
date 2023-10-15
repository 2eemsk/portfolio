using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class Drag_throw : MonoBehaviour
    {
        [SerializeField]
        private GameObject _caseBase;
        [SerializeField]
        private GameObject _thisMission;
        [SerializeField]
        private MissionOpen _missionManager;

        bool dragging = false;
        float distance;
        public float ThrowSpeed;
        public float rotateSpeed;
        public float Speed;

        private Vector3 _initPoint;
        private Vector3 _currentPoint;
        private Vector3 _rayPoint;
        Rigidbody rb;

        private float _curvedAmount = 0;
        private bool checkin;

        private void Start()
        {
            _initPoint = transform.position;
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            checkin = false; 
        }

        private void OnDisable()
        {
            rb.useGravity = false;
            checkin = false;
            dragging = false;
        }

        private void OnMouseDown()
        {
            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            dragging = true;
            rb.isKinematic = false;
            //_initPoint = transform.position;
        }

        void OnMouseDrag()
        {

        }

        private void OnMouseUp()
        {
            if (rb.velocity.magnitude < 1f)
            {
                rb.velocity = Vector3.zero;
                dragging = false;
                transform.position = _initPoint;
            }
            else
            {
                //var _targetVec = _rayPoint - _initPoint;
                rb.useGravity = true;
                //rb.velocity += _targetVec * ThrowSpeed;
                //this.GetComponent<Rigidbody>().velocity -= this.transform.up * ArchSpeed;
                dragging = false;
            }
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.Equals(_caseBase))
                print("성공");
        }*/

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.Equals(_caseBase))
            {
                print("성공");
                StartCoroutine(ClearMission());
                checkin = true;
            }
            else
            {
                checkin = false;
            }
        }

        IEnumerator ClearMission()
        {
            yield return new WaitForSeconds(1);
            //_thisMission.SetActive(false);
            _missionManager.StartCoroutine(_missionManager.MissionClear(2));
            _missionManager.student.GetComponent<PlayerController>().IsMove = true;
        }

        private void Update()
        {
            if (dragging)
            {
                _currentPoint = transform.position;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                _rayPoint = ray.GetPoint(distance);
                //transform.position = Vector3.Lerp(this.transform.position, raypoint, Speed * Time.deltaTime);
                transform.position = _rayPoint;
                //CalcCurve();
                var _targetVec = transform.position - _currentPoint;
                var _targetAngular = _targetVec.magnitude;
                rb.velocity += _targetVec * ThrowSpeed;
                //transform.Rotate(new Vector3(0, _targetVec.y * rotateSpeed, 0));
                //rb.angularVelocity -= _targetVec * rotateSpeed;
                rb.angularVelocity = transform.forward * _curvedAmount * 8f + rb.angularVelocity;
            }
            if(checkin == false)
            {
                if (transform.position.y < -4.0f)
                {
                    transform.position = _initPoint;
                    rb.isKinematic = true;
                    //SoundManager.instance.seManager.audiosource.PlayOneShot(SoundManager.instance.seManager.EffectSounds[6]);
                    AudioSource.PlayClipAtPoint(SoundManager.instance.seManager.EffectSounds[6], transform.position);
                }
            }  
        }

        private void CalcCurve()
        {
            Vector2 _last = new Vector2(_currentPoint.x, _currentPoint.y);
            Vector2 _now = transform.position;
            Vector2 _mid = Vector2.zero;

            bool _isLeft = (_now.x < _last.x) && (_now.y < _last.y);

            if (_isLeft)
            {
                _curvedAmount -= Time.deltaTime * 2f;
            }
            else
            {
                _curvedAmount += Time.deltaTime * 2f;
            }
            print(_isLeft);
        }
    }

}


