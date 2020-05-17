using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    //Public variables
    public GameObject joinButton;
    public GameObject cancelButton;
    public GameObject offlineButton;
    public TMP_InputField inputNameField;
    public GameObject characterGrid;

    //Private variables
    bool userFound = false;

    private void Awake()
    {
        lobby = this; //creates the singleton, lives within the Main Menu scene        
    }

    // Start is called before the first frame update
    void Start()
    {
        //Connects to Master photon server
        PhotonNetwork.ConnectUsingSettings();

        //Check to see if user has a previously entered username
        if (PlayerPrefs.HasKey("username"))
        {
            inputNameField.text = PlayerPrefs.GetString("username");
        }

        if (PlayerPrefs.HasKey("MyCharacter"))
        {
            //set highlighted state of the appropriate avatar button
            characterGrid.transform.GetChild(PlayerPrefs.GetInt("MyCharacter")).GetComponent<Button>().Select();
        }
    }

    private void OnGUI()
    {
        //Detect enter key for name input
        if ((inputNameField.isFocused && inputNameField.text != "") && (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)))
        {
            OnJoinButtonClicked();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        joinButton.SetActive(true);
    }

    public void OnClickCharacterPick(int whichCharacter)
    {
        if (PlayerInfo.PI != null)
        {
            PlayerInfo.PI.mySelectedCharacter = whichCharacter;
            PlayerPrefs.SetInt("MyCharacter", whichCharacter);
        }
    }

    public void OnJoinButtonClicked()
    {
        joinButton.SetActive(false);
        cancelButton.SetActive(true);
        offlineButton.SetActive(false);

        //send the player name over to our PlayerInfo object
        PhotonNetwork.LocalPlayer.NickName = inputNameField.text;

        //Set PlayerPrefs for username
        PlayerPrefs.SetString("username", inputNameField.text);

        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random room game but failed. There must not be any rooms available");
        CreateRoom();
    }

    void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create a new room but failed, there must already be a room with the same name");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        cancelButton.SetActive(false);
        joinButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

}
