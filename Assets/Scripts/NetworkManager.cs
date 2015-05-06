using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public GameObject[] tables;
    public bool[] tablesOccupied;

    private GameObject cameraController;
    // Use this for initialization
    void Start()
    {
        Connect();
    }

    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("0.1.0");

    }

    void OnDisconnectedFromPhoton()
    {
        PhotonNetwork.ConnectUsingSettings("0.1.0");
    }

    void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom("world1");
    }

    void OnJoinedRoom()
    {
        spawnPlayer();
        int emptyTableID = 0;

        for (emptyTableID = 0; emptyTableID < tablesOccupied.Length; emptyTableID++)
        {
            if (!tablesOccupied[emptyTableID])
            {
                cameraController.GetComponent<PhotonView>().RPC("OccupyTable", PhotonTargets.AllBufferedViaServer, emptyTableID);
                break;
            }
        }

        cameraController.transform.position = tables[emptyTableID].transform.position;
        cameraController.SetActive(true);
    }

    void spawnPlayer()
    {
        cameraController = PhotonNetwork.Instantiate("CameraController", new Vector3(0f, 0.5f, 0f), Quaternion.identity, 0);
    }

    void OnLeftRoom()
    {

    }

    [RPC]
    void OccupyTable(int tableID)
    {
        tablesOccupied[tableID] = true;
    }

    [RPC]
    void LeaveTable(int tableID)
    {
        tablesOccupied[tableID] = false;
    }

}