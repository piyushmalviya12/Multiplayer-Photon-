using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;



public class Counter : MonoBehaviour
{
    public Button clickBtn;
    public TMP_Text counterTxt;
    private int randomNum;
    private int drawChance1 = 10, drawChance2 = 10;
    private int sum1,sum2;
    public static Counter cInstance;
    public byte buttonToggleEventCode = 1;
    void Start()
    {
        cInstance = this;
        //clickBtn = GetComponent<Button>();
       // clickBtn.onClick.AddListener(OnClick);
        clickBtn.onClick.AddListener(ClickCounter);
        //SetButtonInteractable(PhotonManager.instance.photonView.IsMine);
        if (PhotonNetwork.IsMasterClient)
        {
            clickBtn.gameObject.SetActive(true);
        }
    }
   
/*    public void OnClick()
    {
        if (PhotonManager.instance.photonView.IsMine)
        {
            PhotonNetwork.RaiseEvent(buttonToggleEventCode, null, RaiseEventOptions.Default, SendOptions.SendReliable);
        }
    }

    public void SetButtonInteractable(bool interactable)
    {
        clickBtn.interactable = interactable;
    }
*/
  
    public void ClickCounter()
    {

        Debug.Log("ClickCounter called");
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (drawChance1 > 0)
            {
                randomNum = Random.Range(0, 10);
                counterTxt.text = randomNum.ToString();
                sum1 += randomNum;
                Debug.Log(PhotonNetwork.LocalPlayer.NickName + " Clicked");
                drawChance1--;
              //  PhotonManager.instance.RaiseClickEvent();

            }
            else
            {
                Debug.Log(PhotonNetwork.LocalPlayer.NickName + " sum is Clicked " + sum1);
            }
        }
        else
        {
            if (drawChance2 > 0)
            {
                randomNum = Random.Range(0, 10);
                counterTxt.text = randomNum.ToString();
                sum2 += randomNum;
                Debug.Log(PhotonNetwork.LocalPlayer.NickName + " Clicked");
                drawChance2--;
            }
            else
            {
                Debug.Log(PhotonNetwork.LocalPlayer.NickName + " sum is Clicked " + sum2);
            }
        }
    }
}



       
    
