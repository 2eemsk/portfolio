using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{

    public class GumSpawn : MonoBehaviour
    {
        [SerializeField] MissionOpen _missionManager;

        public GameObject[] prefabs; //찍어낼 게임 오브젝트를 넣어요
                                     //배열로 만든 이유는 게임 오브젝트를
                                     //다양하게 찍어내기 위해서 입니다
        private BoxCollider area;    //박스콜라이더의 사이즈를 가져오기 위함
        public int count = 100;      //찍어낼 게임 오브젝트 갯수

        private List<GameObject> gumlist = new List<GameObject>();
        private GameObject instance;



        void OnEnable()
        {
            area = GetComponent<BoxCollider>();

            for (int i = 0; i < count; ++i)//count 수 만큼 생성한다
            {
                Spawn();//생성 + 스폰위치를 포함하는 함수
            }

            area.enabled = false;

        }

        private void OnDisable()
        {
            Destroy(instance);
            gumlist.Clear();
        }

        private void Update()
        {
            for(int i = 0; i < gumlist.Count; i++)
            {
                if (gumlist[i].activeSelf == false)
                {
                    Destroy(gumlist[i]);
                    gumlist.RemoveAt(i);  
                }
            }

            if(gumlist.Count == 0)
            {
                Debug.Log("성공");
                StartCoroutine(_missionManager.MissionClear(4));
            }
        }
        
        private Vector3 GetRandomPosition()
        {
            Vector3 basePosition = transform.position;
            Vector3 size = area.size;

            float posX = basePosition.x + UnityEngine.Random.Range(-size.x / 3f, size.x / 3f);
            float posZ = basePosition.z + UnityEngine.Random.Range(-size.z / 5f, size.z / 5f);

            Vector3 spawnPos = new Vector3(posX, 0.1f, posZ);


            return spawnPos;
        }

        private void Spawn()
        {
            int selection = UnityEngine.Random.Range(0, prefabs.Length);

            GameObject selectedPrefab = prefabs[selection];

            Vector3 spawnPos = GetRandomPosition();//랜덤위치함수

            instance = Instantiate(selectedPrefab, spawnPos, Quaternion.Euler(-90, 0, 0));
            gumlist.Add(instance);
        }

    }
}
