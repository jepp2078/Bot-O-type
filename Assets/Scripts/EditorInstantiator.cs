﻿using UnityEngine;
using System.Collections;

public class EditorInstantiator : MonoBehaviour {
    private NetworkManager networkManager;
    public int currentTableID;

	void Start () {
        networkManager = GameObject.Find("ScriptManager").GetComponent<NetworkManager>();
        int emptyTableID = 0;

        for (emptyTableID = 0; emptyTableID < networkManager.tablesOccupied.Length; emptyTableID++)
        {
            if (!networkManager.tablesOccupied[emptyTableID])
            {
                GameObject.Find("ScriptManager").GetComponent<PhotonView>().RPC("OccupyTable", PhotonTargets.AllBufferedViaServer, emptyTableID);
                Debug.Log("Player " + PhotonNetwork.player.ID + " Occupying table " + emptyTableID);
                currentTableID = emptyTableID;
                PhotonPlayer.Find(PhotonNetwork.player.ID).SetScore(emptyTableID);
                break;
            }
        }


        this.gameObject.transform.position = networkManager.tables[emptyTableID].transform.FindChild("TableSurface").position + new Vector3(0f, 0.1f, 0f);//Setting the camara start position
        this.gameObject.GetComponent<CameraControllerScript>().focusObject = networkManager.tables[emptyTableID]; //Setting the camara focus object to the desired table.
        this.gameObject.GetComponent<ControllerGUI>().table = networkManager.tables[emptyTableID]; //gives the rez script the table to spawn the baseplate

	}
    void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        GameObject.Find("ScriptManager").GetComponent<PhotonView>().RPC("LeaveTable", PhotonTargets.AllBufferedViaServer, otherPlayer.GetScore());
        Debug.Log("Player " + otherPlayer.ID + " Leaving table " + otherPlayer.GetScore());
    }
}
