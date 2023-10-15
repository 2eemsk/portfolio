using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class checkManager : MonoBehaviour
    {
        [SerializeField] MissionOpen _missionManager;
        [SerializeField] private int successCount;

        public int SuccessCount { get => successCount; set => successCount = value; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (successCount == 25)
            {
                Debug.Log("¼º°ø");
                StartCoroutine(_missionManager.MissionClear(0));
            }
        }
    }
}