using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class fuseBoxManager : MonoBehaviour
    {
        [SerializeField] private int _successCount;
        [SerializeField] MissionOpen _missionManager;

        public int SuccessCount { get => _successCount; set => _successCount = value; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_successCount == 8)
            {
                Debug.Log("¼º°ø");
                StartCoroutine(_missionManager.MissionClear(3));
            }
        }
    }
}