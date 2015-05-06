using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
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
    }

    void spawnPlayer()
    {
        GameObject cameraController = PhotonNetwork.Instantiate("CameraController", new Vector3(0f, 0.5f, 0f), Quaternion.identity, 0);
        cameraController.SetActive(true);
    }
}