using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{
    public class Piano_interactive : MonoBehaviour
    {
        [SerializeField] MissionManager _missionManager;

        private AudioClip sound;
        private AudioSource audioSource;
        private Animation _anim;
        //public int Do = 0;
        //public int Re = 1;
        //public int Mi = 2;
        //public int Fa = 3;
        //public int Sol = 4;

        // Start is called before the first frame update
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _anim = GetComponent<Animation>();
        }

        void OnMouseDown()
        {
            if (!_anim.isPlaying && !_missionManager.IsMissionPlaying)
            {
                audioSource.Play();
                _anim.Play();


                if (gameObject.layer == 6)
                {
                    print("도");
                    _missionManager.DoClickCount += 1;
                    _missionManager.CurrentClickCount += 1;
                }
                if (gameObject.layer == 7)
                {
                    print("레");
                    _missionManager.ReClickCount += 1;
                    _missionManager.CurrentClickCount += 1;
                }
                if (gameObject.layer == 8)
                {
                    print("미");
                    _missionManager.MiClickCount += 1;
                    _missionManager.CurrentClickCount += 1;
                }
                if (gameObject.layer == 9)
                {
                    print("파");
                    _missionManager.FaClickCount += 1;
                    _missionManager.CurrentClickCount += 1;
                }
                if (gameObject.layer == 10)
                {
                    print("솔");
                    _missionManager.SolClickCount += 1;
                    _missionManager.CurrentClickCount += 1;
                }
                if (gameObject.layer == 11)
                {
                    print("라");
                    _missionManager.LaClickCount += 1;
                    _missionManager.CurrentClickCount += 1;
                }
                if (gameObject.layer == 12)
                {
                    print("시");
                    _missionManager.SiClickCount += 1;
                    _missionManager.CurrentClickCount += 1;
                }
                if (gameObject.layer == 13)
                {
                    print("높은 도");
                    _missionManager.HighDoClickCount += 1;
                    _missionManager.CurrentClickCount += 1;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

    }
}

