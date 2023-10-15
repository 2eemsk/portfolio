using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class ScraperControl : MonoBehaviour
    {
        [SerializeField] private AudioSource _audio;

        float distance = 6.5f;


        private void OnMouseDown()
        {
            //SoundManager.instance.seManager.audiosource.PlayOneShot(SoundManager.instance.seManager.EffectSounds[2]);
            //AudioSource.PlayClipAtPoint(SoundManager.instance.seManager.EffectSounds[2], transform.position);
        }

        private void OnMouseUp()
        {
            //SoundManager.instance.seManager.audiosource.Stop();
            _audio.Stop();
        }
        void OnMouseDrag()
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(objPosition.x, transform.position.y, objPosition.y);

            if (!_audio.isPlaying)
            {
                _audio.Play();
            }
        }
    }
}
