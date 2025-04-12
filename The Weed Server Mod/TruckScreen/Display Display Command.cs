using Photon.Pun;
using UnityEngine;

namespace The_Weed_Server_Mod.TruckScreen
{
    public class Display_Display_Command
    {
        public static void ShowDisplayMessage(string message)
        {
            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null)
            {
                PhotonView pv = truckScreen.GetComponent<PhotonView>();
                if (pv != null)
                {
                    string formattedMessage = "<color=#00FF00>DISPLAYING MESSAGE:</color>\n" + message;
                    pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", formattedMessage);
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