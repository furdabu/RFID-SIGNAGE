using System;
using System.Collections;
using System.Runtime;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UTRSDKV1PC;
using UniRx;
using UnityEngine.UI;

public class rfid : MonoBehaviour
{
    RFIDControl rc = new RFIDControl();
    private UdpClient udpClient;
    private Subject<byte[]> subject = new Subject<byte[]>();
    [SerializeField] Text message;

    [SerializeField] GameObject atn0, atn1, atn2, atn3, pos;

    public int[] powers = new int[4];
    public Vector3 targetPos;
    void Start()
    {
        udpClient = new UdpClient(2002);
        udpClient.BeginReceive(OnReceived, udpClient);

        subject
            .ObserveOnMainThread()
            .Subscribe(msg => {

    

                var stringmsg = BitConverter.ToString(msg);
                message.text = stringmsg + "\r\n" + message.text;
                int nu = msg[1];
                powers[nu] = 900 + (BitConverter.ToInt32(msg, 32));

                float all = 0;
                for(int i = 0; i < powers.Length; i++)
                {
                    all += powers[i];
                }
    

                float effectA = powers[0] / all;
                float effectB = powers[1] / all;
                float effectC = powers[2] / all;
                float effectD = powers[3] / all;
                Debug.Log(effectA);
                targetPos = atn0.transform.position * effectA
                + atn1.transform.position * effectB
                + atn2.transform.position * effectC
                + atn3.transform.position * effectD;

                pos.transform.position = targetPos;
            }).AddTo(this);
    }


    public void connect()
    {
        rc.ResponseRFID += new RFIDControl.InputEventHandler(rfidEventHandler);

        if (rc.Open(4, RFID_BaudRate.BaudRate115200))
        {

            rc.GetROMVersion();
        }

    }

    private void rfidEventHandler(System.Object sender, InputEventArgs e)
    {

        switch (e.SendCommand)
        {
            case RFID_SendCommand.GetROMVersion:
                break;
            default:
                break;
        }

    }
    private void OnReceived(System.IAsyncResult result)
    {
        UdpClient getUdp = (UdpClient)result.AsyncState;
        IPEndPoint ipEnd = null;

        byte[] getByte = getUdp.EndReceive(result, ref ipEnd);

        subject.OnNext(getByte);

        getUdp.BeginReceive(OnReceived, getUdp);
    }

    private void OnDestroy()
    {
        udpClient.Close();
    }
}