using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{

    public class removeGum : MonoBehaviour
    {

        private int ScrapCount;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 14)
            {
                Debug.Log("check" + ScrapCount);
                ScrapCount++;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (ScrapCount == 3)
            {
                gameObject.SetActive(false);

            }
        }
    }
}
