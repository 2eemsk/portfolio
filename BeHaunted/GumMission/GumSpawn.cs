using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mission
{

    public class GumSpawn : MonoBehaviour
    {
        [SerializeField] MissionOpen _missionManager;

        public GameObject[] prefabs; //�� ���� ������Ʈ�� �־��
                                     //�迭�� ���� ������ ���� ������Ʈ��
                                     //�پ��ϰ� ���� ���ؼ� �Դϴ�
        private BoxCollider area;    //�ڽ��ݶ��̴��� ����� �������� ����
        public int count = 100;      //�� ���� ������Ʈ ����

        private List<GameObject> gumlist = new List<GameObject>();
        private GameObject instance;



        void OnEnable()
        {
            area = GetComponent<BoxCollider>();

            for (int i = 0; i < count; ++i)//count �� ��ŭ �����Ѵ�
            {
                Spawn();//���� + ������ġ�� �����ϴ� �Լ�
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
                Debug.Log("����");
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

            Vector3 spawnPos = GetRandomPosition();//������ġ�Լ�

            instance = Instantiate(selectedPrefab, spawnPos, Quaternion.Euler(-90, 0, 0));
            gumlist.Add(instance);
        }

    }
}
