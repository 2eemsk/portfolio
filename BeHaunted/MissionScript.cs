using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace missionManager
{
    [CreateAssetMenu(fileName = "Mission_Manager", menuName = "Scriptable Object/Mission_Manager", order = int.MaxValue)]
    public class MissionScript : ScriptableObject
    {
        [SerializeField]
        private GameObject _mission;
        public GameObject Mission { get => _mission; set => _mission = value; }


    }
}

