using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;

public class UiHandler : MonoBehaviourPunCallbacks
{
    //All User Interface Reference In MainMenu 

    public static UiHandler Instance;
    public GameObject StartGame;

    //Next Scene U want To load
    public Lobby.Scene NextScene;
    [Header("Room Show Aera of scroll view")]
    public GameObject RoomContext;
    [Header("Player Show Aera of scroll view")]
    public GameObject PlayerContext;
    [Header("if user want to name Room by own")]
    public bool byName;
    [ShowIf("byName", true)]
    public InputField RoomName;
    public GameObject CreateRoombtn;

    [Header("if user want to use nickname")]
    public bool UseNickName;
    [ShowIf("UseNickName", true)]
    public InputField PlayerNickname;
    //Text for user PhotonNetwork Id
    public Text OwnUserID;

    [InfoBox("Panel Messages")]
    public string MeassageForBusyHost;
    public string MesaageForDeclineChallenge;
    public string MessageForAcceptedChallenege;

    [Header("UI/Panels/Texts")]
    //Totatl Player in Room Text
    public Text PlayerCount;
    [InfoBox("Panels")]
    public GameObject ChallengePopUp;
    public GameObject BusyPopUP;
    public GameObject ChallenedAcceptedPopUp;
    public GameObject CancelRequestPopUp;
    [InfoBox("PopUp Texts")]
    public Text Challengetext;
    public Text BusyPopUpText;
    public Text ChallenedAcceptedPopUpText;
    public Text CancelRequestPopUpText;
    [InfoBox("Buttons")]
    public Button ChallengeAccepted;
    public Button ChallengeDeclined;
    public Button CancelRequest;


    [InfoBox("Events")]
    //Events You can Call on functions
    public UnityEvent OnRoomCreation;
    public UnityEvent OnLobbyJoined;
    public UnityEvent OnRoomJoined;
    [InfoBox("This Event Called When you Already Send one Request and didnt cancel Request And Try To send Another Request")]
    public UnityEvent PlayerAlredySentRquest;


    private void Awake()
    {
        if (UiHandler.Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Lobby.Lob.RemoveAllListners();
        Lobby.Lob.OnClickListenes();
        CheckForBackToLobby();
    }
    void CheckForBackToLobby()
    {
        //when User Getting Back From Gameplay And resetting Values

        if (PhotonNetwork.IsConnectedAndReady && !GameManager.Instance.once)
        {
            if (PhotonNetwork.CurrentRoom.Name != Lobby.Lob.IntialRoomName)
            {
                RestValues();
                PhotonNetwork.LeaveRoom();
            }
   
        }
    }

    void RestValues()
    {
        Lobby.Lob.CA.Acceptance = false;
        Lobby.Lob.CA.Challenger = false;
        Lobby.Lob.CA.win = false;
        Lobby.Lob.CA.lose = false;
        Lobby.Lob.CA.Meassage = null;
        Lobby.Lob.CA.ChallengedID = null;
        Lobby.Lob.CA.ChallangerID = null;
        Lobby.Lob.clinet = false;
        Lobby.Lob.RequestSent = false;
        Lobby.Lob.ClearList(RoomContext);
        Lobby.Lob.ClearList(PlayerContext);
        Lobby.Lob._PlayerBtnInfo.Clear();
        Lobby.Lob.RoomNameToJoin = null;

    }
}
