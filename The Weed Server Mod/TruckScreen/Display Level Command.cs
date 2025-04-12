using Photon.Pun;
using UnityEngine;

namespace The_Weed_Server_Mod.TruckScreen
{
    public class Display_Level_Command
    {
        public static void ShowLevelMessage()
        {
            TruckScreenText truckScreen = GameObject.FindObjectOfType<TruckScreenText>();
            if (truckScreen != null)
            {
                PhotonView pv = truckScreen.GetComponent<PhotonView>();
                if (pv != null)
                {
                    // Format the decideLevel so that it is distinct on the truck screen.
                    string formattedLevel = "<color=#00FF00>VOTE ON THE NEXT LEVEL WITH /vote {level}:</color>\n\n" +
                        "<color=#D4A276>MANOR:</color>\n <size=0.25><mark=#6f4518>lll </mark></size>\n\n" +
                        "<color=#7241b4>WIZARD:</color>\n <size=0.25><mark=#502d7e>lll </mark></size>\n\n" +
                        "<color=#7CA1C2>ARTIC:</color>\n <size=0.25><mark=#3C6080>lll </mark></size>";
                    // Use the vanilla RPC implemented in TruckScreenText.
                    pv.RPC("MessageSendCustomRPC", RpcTarget.All, "", formattedLevel);
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

/*
$"<color=#D4A276>MANOR:</color>\n <mark=#6f4518>lll \n" +
$"<color=#8C60C6>WIZARD:</color>\n <mark=#5e2f98>lll \n" +
$"<color=#7CA1C2>WIZARD:</color>\n <mark=#3C6080>lll \n";
*/