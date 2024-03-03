using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventRaiseExample : MonoBehaviourPun
{
    private SpriteRenderer spriteRenderer;
    public Button button;

    private const byte Colour_CHANGE_EVENT = 1;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (base.photonView.IsMine)
        {
            button.onClick.AddListener(ChangeColor);
        }
    }
    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    

    public void OnDisable()
    {
       
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    public void ChangeColor()
    {
        float r = Random.Range(0f,1f);
        float g = Random.Range(0f,1f);
        float b = Random.Range(0f,1f);

        spriteRenderer.color = new Color(r, g, b,1f);

        object[] datas = new object[] { r, g, b };
   
        PhotonNetwork.RaiseEvent(Colour_CHANGE_EVENT, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }
    private void OnEvent(EventData obj)
    {
        if(obj.Code == Colour_CHANGE_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            float r = (float)datas[0];
            float g = (float)datas[1];
            float b = (float)datas[2];
            spriteRenderer.color = new Color(r, g, b, 1f);
        }
    }
}
