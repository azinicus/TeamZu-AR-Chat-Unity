using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    private Recorder playerRecorder;

    // Start is called before the first frame update
    void Start()
    {
        playerRecorder = GetComponent<Recorder>();
    }

    public void GroupTalk()
    {
        //Set the talking animation for this player in AvatarSetup
        AvatarSetup.AS.PlayerTalkingAnimation();
        //User is talking so we'll set them to available
        AvatarSetup.AS.SetStatus(0);

        playerRecorder.TransmitEnabled = playerRecorder.TransmitEnabled ? false : true;
    }

    public void MuteAudio()
    {
        AudioListener.pause = AudioListener.pause ? false : true;
    }

}
