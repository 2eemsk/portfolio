using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class clickCheck : MonoBehaviour
    {

        [SerializeField] GameObject[] check;
        [SerializeField] checkManager cm;

        // Start is called before the first frame update
        void OnEnable()
        {
            var _randomNum = Random.RandomRange(0, 2);
            check[_randomNum].SetActive(false);


            if (check[0].activeSelf == true)
            {
                cm.SuccessCount++;
                check[1].SetActive(false);
            }

            if (check[0].activeSelf == false)
            {
                check[1].SetActive(true);
            }
        }

        private void OnMouseDown()
        {

            if (check[0].activeSelf == true)
            {
                check[0].SetActive(false);
                check[1].SetActive(true);
                cm.SuccessCount--;
            }
            else if (check[0].activeSelf == false)
            {
                check[0].SetActive(true);
                check[1].SetActive(false);
                cm.SuccessCount++;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}