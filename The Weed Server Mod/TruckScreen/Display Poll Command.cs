using Photon.Pun;
using UnityEngine;

namespace The_Weed_Server_Mod.TruckScreen
{
    public class Display_Poll_Command
    {
        public static void ShowPollMessage(string question)
        {
            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null)
            {
                PhotonView pv = truckScreen.GetComponent<PhotonView>();
                if (pv != null)
                {
                    string formattedQuestion = "<color=#FFFF00>POLL:</color>\n" + question;
                    pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", formattedQuestion);
                }
                else
                {
                    Plugin.Instance.mls.LogWarning("TruckScreenText does not have a PhotonView component.");
                }
            }
            else
            {
                Plugin.Instance.mls.LogWarning("TruckScreenText instance not found.");
            }
        }
    }
}