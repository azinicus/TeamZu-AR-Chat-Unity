using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class AvatarSetup : MonoBehaviour
{
    //Public Singleton
    public static AvatarSetup AS;

    //Public variables
    private PhotonView PV;
    public int characterValue;
    public GameObject myCharacter;
    public bool facePresent = false;
    public bool faceCaptured = false;

    //Private variables
    private Image avatarBorder;
    private Image avatarImage;
    private TMPro.TextMeshProUGUI avatarNameText;
    private GameObject playerTalkingObject;

    private void OnEnable()
    {
        if (AvatarSetup.AS == null)
        {
            AvatarSetup.AS = this;
        }
    }

    private void Update()
    {

        //test keyboard shortcuts
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetStatus(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetStatus(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetStatus(2);
        }
#endif

    }

    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedCharacter);
        }

        //get the avatar status elements
        avatarBorder = myCharacter.transform.GetChild(0).GetComponent<Image>();
        avatarImage = myCharacter.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        playerTalkingObject = myCharacter.transform.GetChild(3).gameObject;

        StartCoroutine(ThingsThatNeedToWait());

        //Set player talking animation to off initially
        playerTalkingObject.SetActive(false);

    }

    //test coroutine
    IEnumerator ThingsThatNeedToWait()
    {
        yield return new WaitForSeconds(0.2f);
        avatarNameText = myCharacter.transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();

        //set the name text
        PV.RPC("RPC_SetCharacterName", RpcTarget.AllBuffered);
    }

    /// <summary>
    /// Set a player's status
    /// </summary>
    /// <param name="status">0 = available, 1 = away, 2 = DND</param>
    public void SetStatus(int status)
    {
        if (PV.IsMine)
        {
            PV.RPC("RPC_SetCharacterStatus", RpcTarget.AllBuffered, status);
        }
    }

    //when you're leaving the room
    public void InitiateLeaveRoom()
    {
        if (PV.IsMine)
        {
            PV.RPC("RPC_RemoveCharacter", RpcTarget.AllBuffered);
        }
    }

    //Set the talking visual
    public void PlayerTalkingAnimation()
    {
        PV.RPC("RPC_PlayerTalking", RpcTarget.AllBuffered);
    }


    #region Photon RPCs
    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        characterValue = whichCharacter;
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[whichCharacter], GameManager.GM.avatarGrid.transform.position, GameManager.GM.avatarGrid.transform.rotation, GameManager.GM.avatarGrid.transform);
    }

    [PunRPC]
    void RPC_SetCharacterName()
    {
        //sets the name of the person to that of the Photon View owner nickname
        avatarNameText.text = PV.Owner.NickName;
    }

    [PunRPC]
    void RPC_PlayerTalking()
    {
        //flips the active state of the player talking game object
        playerTalkingObject.SetActive(!playerTalkingObject.activeInHierarchy);
    }

    [PunRPC]
    void RPC_SetCharacterStatus(int status)
    {
        //quick object presence check because it may have been destroyed
        if (avatarBorder == null)
        {
            return;
        }

        if (status == 0)
        {
            avatarBorder.color = GameManager.GM.zuGreen;
            avatarImage.color = GameManager.GM.zuWhite;
        }
        else if (status == 1)
        {
            avatarBorder.color = GameManager.GM.zuGrey;
            avatarImage.color = new Color32(255, 255, 255, 136);
        }
        else if (status == 2)
        {
            avatarBorder.color = GameManager.GM.zuRed;
            //TODO: a DND friendly image
        }
    }

    [PunRPC]
    void RPC_RemoveCharacter()
    {
        Destroy(myCharacter);
    }
    #endregion
}
