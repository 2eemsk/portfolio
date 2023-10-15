using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private GameObject myPortal;
    [SerializeField] private GameObject nextPortal;

    public GameObject NextPortal { get => nextPortal; set => nextPortal = value; }
    public GameObject MyPortal { get => myPortal; set => myPortal = value; }

    private float moveInterval = 2.0f;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            NextRoom(collision.gameObject);
        }
    }

    public void ConnectedPortal(GameObject portal)
    {
        nextPortal = portal;
    }

    private void NextRoom(GameObject _player)
    {
        if(nextPortal != null)
        {
            switch (this.gameObject.name)
            {
                case "up":
                    _player.transform.position = new Vector3(nextPortal.transform.position.x, 0, nextPortal.transform.position.z + moveInterval);
                    MapGenerator.OnOffPortal(MapGenerator.portalList);
                    break;
                case "down":
                    _player.transform.position = new Vector3(nextPortal.transform.position.x, 0, nextPortal.transform.position.z - moveInterval);
                    MapGenerator.OnOffPortal(MapGenerator.portalList);
                    break;
                case "right":
                    _player.transform.position = new Vector3(nextPortal.transform.position.x+ moveInterval, 0, nextPortal.transform.position.z);
                    MapGenerator.OnOffPortal(MapGenerator.portalList);
                    break;
                case "left":
                    _player.transform.position = new Vector3(nextPortal.transform.position.x - moveInterval, 0, nextPortal.transform.position.z);
                    MapGenerator.OnOffPortal(MapGenerator.portalList);
                    break;

            }

            //Camera.main.transform.position = NextPortal.GetComponentInParent<Room>().transform.position + nextPortal.GetComponentInParent<Room>().CamPos;
            GetComponentInParent<Room>().gameObject.layer = 14;
            NextPortal.GetComponentInParent<Room>().gameObject.layer = 13;
        }
        else
        {
            print("null");
        }
    }
}
