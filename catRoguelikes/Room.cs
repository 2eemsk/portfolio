using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Room : MonoBehaviour
{

    [SerializeField] private bool _isClear = false;
    [SerializeField] private Vector3 _camPos;

    public bool IsClear { get => _isClear; set => _isClear = value; }
    public Vector3 CamPos { get => _camPos; set => _camPos = value; }


    [SerializeField]private GameObject _mapPrefab;
    public GameObject MapPrefab { get => _mapPrefab; set => _mapPrefab = value; }

}
