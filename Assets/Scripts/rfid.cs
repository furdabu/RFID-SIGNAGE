using System.Collections;
using System.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UTRSDKV1PC;

public class rfid : MonoBehaviour
{
    RFIDControl rc = new RFIDControl();
    // Start is called before the first frame update
    void Start()
    {
        rc.ResponseRFID += new RFIDControl.InputEventHandler(rfidEventHandler);
        
    }

    private void rfidEventHandler(Object sender, InputEventArgs e){
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
