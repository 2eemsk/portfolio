using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class GumMissionManager : MonoBehaviour
    {
        private int successCount;

        // Start is called before the first frame update

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (successCount == 10)
            {
                Debug.Log("¼º°ø");
            }
        }
    }
}
