using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class FuseOnOff : MonoBehaviour
    {

        [SerializeField] GameObject[] onoff;
        [SerializeField] fuseBoxManager fbm;

        // Start is called before the first frame update
        void OnEnable()
        {
            var _randomNum = Random.RandomRange(0, 2);
            onoff[_randomNum].SetActive(false);           

            if (onoff[0].activeSelf == true)
            {
                fbm.SuccessCount++;
                onoff[1].SetActive(false);
            }

            if (onoff[0].activeSelf == false)
            {
                onoff[1].SetActive(true);
            }
        }

        private void OnDisable()
        {
            for(int i=0; i<onoff.Length; i++)
            {
                onoff[i].SetActive(true);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseDown()
        {
            Debug.Log("click");

            if (onoff[0].activeSelf == true)
            {
                //SoundManager.instance.seManager.audiosource.PlayOneShot(SoundManager.instance.seManager.EffectSounds[3]);
                AudioSource.PlayClipAtPoint(SoundManager.instance.seManager.EffectSounds[3], transform.position);
                onoff[0].SetActive(false);
                onoff[1].SetActive(true);
                fbm.SuccessCount--;
            }
            else if (onoff[0].activeSelf == false)
            {
                //SoundManager.instance.seManager.audiosource.PlayOneShot(SoundManager.instance.seManager.EffectSounds[3]);
                AudioSource.PlayClipAtPoint(SoundManager.instance.seManager.EffectSounds[3], transform.position);
                onoff[0].SetActive(true);
                onoff[1].SetActive(false);
                fbm.SuccessCount++;
            }
        }
    }
}