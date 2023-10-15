using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Data", menuName = "Scriptable Object/Map Data", order = 1)]

public class MapData : ScriptableObject
{
    [SerializeField] private GameObject startMapPrefab;
    [SerializeField] private List<GameObject> normalMapPrefabs;
    [SerializeField] private List<GameObject> specialMapPrefabs;
    [SerializeField] private GameObject nextMapPrefab;

    public GameObject StartMapPrefab { get => startMapPrefab; set => startMapPrefab = value; }
    public List<GameObject> NormalMapPrefabs { get => normalMapPrefabs; set => normalMapPrefabs = value; }
    public List<GameObject> SpecialMapPrefabs { get => specialMapPrefabs; set => specialMapPrefabs = value; }
    public GameObject NextMapPrefab { get => nextMapPrefab; set => nextMapPrefab = value; }


}
