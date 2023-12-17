using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;



public class Counter : MonoBehaviour
{
    public Button clickBtn;
    public TMP_Text counterTxt;
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        clickBtn.onClick.AddListener(ClickCounter);
    }

   public void ClickCounter()
    {
        count += 1;
        counterTxt.text = count.ToString();
        Debug.Log(PhotonNetwork.LocalPlayer.NickName +" Clicked");
    }
}
