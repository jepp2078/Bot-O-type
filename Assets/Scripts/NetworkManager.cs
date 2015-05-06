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
        cameraController.SetActive(true);
    }

    void spawnPlayer()
    {
        cameraController = PhotonNetwork.Instantiate("CameraController", new Vector3(0f, 0.5f, 0f), Quaternion.identity, 0);
    }

    [RPC]
    void OccupyTable(int tableID)
    {
        Debug.Log("Someone is trying to occupy table " + tableID);
        tablesOccupied[tableID] = true;
    }

    [RPC]
    void LeaveTable(int tableID)
    {
        Debug.Log("Someone is trying to leave table " + tableID);
        tablesOccupied[tableID] = false;
    }

}