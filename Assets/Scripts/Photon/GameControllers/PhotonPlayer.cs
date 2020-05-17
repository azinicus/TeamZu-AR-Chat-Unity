using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        
        if(PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate("PlayerAvatar", GameManager.GM.avatarGrid.gameObject.transform.position, GameManager.GM.avatarGrid.gameObject.transform.rotation, 0);
        }
    }
}
