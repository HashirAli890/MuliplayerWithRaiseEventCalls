using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayHandler : MonoBehaviourPunCallbacks
{
    
    public static PlayHandler Instance;
    [Header("Next Scene")]
    public Lobby.Scene Scene;
   [Header("Buttons")]
    public GameObject Winbtn;
    public GameObject Losebtn;

    [Header("Panel Texts")]
    public string WinMessage;
    public string LoseMessage;
    public string DisConnectMessage;

    private const byte Winbyte = 6;
    private const byte Losebyte = 7;
   

    [Header("UI")]
    [InfoBox("Texts")]
    public Text WinText;
    public Text LoseText;
    [InfoBox("Panels")]
    public GameObject WinPanel;
    public GameObject LosePanel;

    [Header("Events")]
    [BoxGroup]
    [InfoBox("On Win")]
    public UnityEvent WinEvent;
    [BoxGroup]
    [InfoBox("On Lose")]
    public UnityEvent LoseEvent;
    [BoxGroup]
    [InfoBox("On Disconnect")]
    public UnityEvent DisconnectEvent;




    ChallenegAttributes CR;


    private void Awake()
    {
        if (PlayHandler.Instance == null)
        {
            Instance = this;
        }
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
      
        Winbtn.GetComponent<Button>().onClick.AddListener(() => { OnWin(); });
        Losebtn.GetComponent<Button>().onClick.AddListener(() => { OnLose(); });

        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDestroy()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(ExitGames.Client.Photon.EventData obj)
    {
        if (Lobby.Lob.ShowDebugs)
        {
            Debug.Log("Event Recived");
            Debug.Log(obj.Code);
        }
        switch (obj.Code)
        {
            case Winbyte:
                CR = JsonUtility.FromJson<ChallenegAttributes>((string)obj.CustomData);
                if (CR.Challenger)
                {
                    if (Lobby.Lob.ShowDebugs)
                        Debug.Log("we are in "+CR.ChallangerID);
                    if (CR.win)
                    {
                        if (Lobby.Lob.ShowDebugs)
                            Debug.Log("we are in in " + CR.ChallangerID);
                        LosePanel.SetActive(true);
                        LoseText.text = LoseMessage;
                        if (PhotonNetwork.IsMasterClient)
                            Invoke("SwitchScene", 5f);
                    }
                }
                else
                {
                    if (Lobby.Lob.ShowDebugs)
                        Debug.Log("we are in else" );
                    LosePanel.SetActive(true);
                    LoseText.text = LoseMessage;
                    if (PhotonNetwork.IsMasterClient)
                        Invoke("SwitchScene", 5f);
                }
          
                break;
            case Losebyte:
                if (Lobby.Lob.ShowDebugs)
                    Debug.Log("cehcking");
                CR = JsonUtility.FromJson<ChallenegAttributes>((string)obj.CustomData);
                if (CR.Challenger)
                {
                    if (CR.lose)
                    {
                        WinPanel.SetActive(true);
                        WinText.text = WinMessage;
                        if (PhotonNetwork.IsMasterClient)
                        {
                            Invoke("SwitchScene", 5f);
                        }
                    }
                }
                else
                {
                    WinPanel.SetActive(true);
                    WinText.text = WinMessage;
                    if (PhotonNetwork.IsMasterClient)
                        Invoke("SwitchScene", 5f);

                }
                break;
        }
    }
    public void OnWin()
    {
        if (Lobby.Lob.ShowDebugs)
            Debug.Log("i got clicked");
        WinEvent.Invoke();
        WinPanel.SetActive(true);
        WinText.text = WinMessage;
        Lobby.Lob.CA.win = true;
        string json = JsonUtility.ToJson(Lobby.Lob.CA) ;
      
        PhotonNetwork.RaiseEvent(Winbyte, json, RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);
        if (PhotonNetwork.IsMasterClient) 
        {
            Invoke("SwitchScene", 5f);
        }
    }
    public void OnLose() 
    {
        if (Lobby.Lob.ShowDebugs)
            Debug.Log("i called lose");
        LoseEvent.Invoke();
        LosePanel.SetActive(true);
        LoseText.text = LoseMessage;
        Lobby.Lob.CA.lose = true;
        string json = JsonUtility.ToJson(Lobby.Lob.CA);
        PhotonNetwork.RaiseEvent(Losebyte, json, RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);
        if (PhotonNetwork.IsMasterClient)
        {
            Invoke("SwitchScene", 5f);
        }
    }
    public void OnDiscconet() 
    {
        DisconnectEvent.Invoke();
        WinPanel.SetActive(true);
        WinText.text = DisConnectMessage;
        Invoke("SwitchScene", 5f);
    }

    public void SwitchScene() 
    {
        
        Lobby.Lob.SwitchScene((int)Scene);
    }

}
