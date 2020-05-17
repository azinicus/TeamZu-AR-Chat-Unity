using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    //public variables
    public GameObject arFoundationElement;
    public GameObject avatarGrid;

    //Editor exposed private variables
    [SerializeField]
    private float awayMaxTime = 10.0f;
    [SerializeField]
    private TMP_Dropdown m_StatusDropdown;
    [SerializeField]
    private GameObject openCvElement;
    
    [Header("Theme Colors")]
    public Color32 zuGreen;
    public Color32 zuDarkBlue;
    public Color32 zuLightGreen;
    public Color32 zuPurple;
    public Color32 zuLavender;
    public Color32 zuPink;
    public Color32 zuRed;
    public Color32 zuYellow;
    public Color32 zuWhite;
    public Color32 zuGrey;

    [Header("AR Stuff")]
    public ARFaceManager m_ARFaceManager;
    public ARSession m_Session;

    //Private variables
    private bool m_ARFoundationSupported = false;
    private bool m_OpenCVSupported = false;

    private void OnEnable()
	{
        if(GameManager.GM == null)
		{
            GameManager.GM = this;
		}

        // Detect face tracking capability - or set accordingly
        // need to diagnose this, but might not use it anyway, for now just saying Andorid and iPhone X+ or something

        //var subsystem = m_Session?.subsystem;
        //if (subsystem != null)
        //{
        //    var configs = subsystem.GetConfigurationDescriptors(Allocator.Temp);
        //    if (configs.IsCreated)
        //    {
        //        using (configs)
        //        {
        //            foreach (var config in configs)
        //            {
        //                if (config.capabilities.All(Feature.FaceTracking))
        //                {
        //                    m_FaceTrackingSupported = true;
        //                }
        //            }
        //        }
        //    }
        //}

#if UNITY_IOS
        if (UnityEngine.iOS.Device.generation.ToString().Contains("iPhoneX") || UnityEngine.iOS.Device.generation.ToString().Contains("iPhone1"))
        {
            m_ARFoundationSupported = true;
            openCvElement.SetActive(false);

        } else
        {
            m_OpenCVSupported = true;
            arFoundationElement.SetActive(false);
        }
#elif UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            m_ARFoundationSupported = true;
            openCvElement.SetActive(false);
        }
#else
        //assuming it's a desktop or something with a webcam
        m_OpenCVSupported = true;
        arFoundationElement.SetActive(false);
#endif

    }

    private void Start()
    {

        //set the theme colors used in other scripts
        zuGreen = new Color32(100, 255, 102, 255);
        zuDarkBlue = new Color32(33, 215, 254, 255);
        zuLightGreen = new Color32(31, 255, 211, 255);
        zuPurple = new Color32(113, 92, 255, 255);
        zuLavender = new Color32(178, 62, 255, 255);
        zuPink = new Color32(253, 77, 212, 255);
        zuRed = new Color32(252, 23, 21, 255);
        zuYellow = new Color32(254, 226, 0, 255);
        zuWhite = new Color32(255, 255, 255, 255);
        zuGrey = new Color32(102, 102, 102, 255);

        //Start detecting face presence on an interval
        StartCoroutine(CheckForFace());

        //Add listener for when the value of the Status Dropdown changes, to take action
        m_StatusDropdown.onValueChanged.AddListener(delegate
        {
            StatusDropdownValueChanged(m_StatusDropdown);
        });

        //Prevent screen from going to sleep
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    /// <summary>
    /// Handle what happens when a participant quits the app
    /// </summary>
    private void OnApplicationQuit()
    {
        DisconnectPlayer();
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        AvatarSetup.AS.InitiateLeaveRoom();

        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene("Lobby");
    }

    /// <summary>
    /// Checks the AR Face Manager to see if there are faces present and sets the user's status accordingly
    /// </summary>
    /// <param name="awayTime"></param>
    /// <returns></returns>
    IEnumerator CheckForFace()
    {
        float awayTimer = 0.0f;
        bool userAvailable = true;
        float loopWaitInterval = 2.0f;

        if (!m_ARFoundationSupported && !m_OpenCVSupported)
            yield break;

        while(true)
        {
            //just giving it a bit of a delay so it's not constantly updating the statuses
            yield return new WaitForSeconds(loopWaitInterval);

            if (AvatarSetup.AS.facePresent)
            {
                if(!userAvailable)
                {
                    AvatarSetup.AS.SetStatus(0);
                    userAvailable = true;
                }

                awayTimer = 0.0f;
            }
            else
            {
                awayTimer += loopWaitInterval;

                if(awayTimer >= awayMaxTime && userAvailable)
                {
                    userAvailable = false;
                    AvatarSetup.AS.SetStatus(1);
                }
            }

            //TODO: account for DND and ultimately do a current status check in AvatarSetup
        }
    }

    /// <summary>
    /// Talks to the static Avatar class and sets the status from UI buttons
    /// </summary>
    /// <param name="m_StatusDropdown"></param>
    private void StatusDropdownValueChanged(TMP_Dropdown m_StatusDropdown)
    {
        AvatarSetup.AS.SetStatus(m_StatusDropdown.value);
    }

}
