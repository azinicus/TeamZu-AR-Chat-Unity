using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{


    //Room info
    public static PhotonRoom room;
    private PhotonView PV;

    //public bool isGameStarted;
    public int currentScene;
    public int multiPlayerScene;

    private void Awake()
    {
        //set up singleton
        if(PhotonRoom.room == null)
        {
            PhotonRoom.room = this;
        } else
        {
            if(PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Room successfully joined");
        if (!PhotonNetwork.IsMasterClient)
            return;

        StartGame();
    }


    void StartGame()
    {
        Debug.Log("Loading Level");
        PhotonNetwork.LoadLevel(multiPlayerScene);
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;

        if(currentScene == multiPlayerScene)
        {
            CreatePlayer();
        }
    }

    //[PunRPC]
    private void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
}
