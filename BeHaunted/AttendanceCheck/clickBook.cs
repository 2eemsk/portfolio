using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class clickBook : MonoBehaviour
    {
        public Animator openAnim;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseDown()
        {
            print("click!");
            openAnim.SetBool("onclick", true);
        }
    }
}