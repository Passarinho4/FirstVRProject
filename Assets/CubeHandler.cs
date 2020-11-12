using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHandler : MonoBehaviour
{

    public GameObject RightController;
    public Rigidbody Ball;
    public TextMesh Text; 
    public TextMesh TextButton;

    private OrderedDictionary rounds = new OrderedDictionary();
    private int actualRound = 1;


    // Start is called before the first frame update
    void Start()
    {

     rounds.Add(1, "");
     rounds.Add(2, "");

    }

    // Update is called once per frame
    void Update()
    {
        Text.text = RightController.transform.position.x.ToString();
        // if (RightController.transform.position.x > 0) {
        //     transform.Rotate(0.0f, 0.0f, 0.1f, Space.Self);
        // } else {
        //     transform.Rotate(0.0f, 0.0f, -0.1f, Space.Self);
        // }
        transform.Rotate(0.0f, 0.0f, 0.3f * RightController.transform.position.x, Space.Self);

        // bool pressed = OVRInput.Get(OVRInput.Button.One);

        // if(pressed) {
        //     TextButton.text = "wciśniety";
        // } else {
        //     TextButton.text = "nie";
        // }

        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, inputDevices);

        var device = inputDevices[0];

        bool triggerValue = false;
        device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue);
        if(triggerValue) {
            TextButton.text = "zajebiście";
            Ball.transform.position = new Vector3(0.0f, 30.0f, 7.0f);
            Ball.velocity = Vector3.zero;
            Ball.angularVelocity = Vector3.zero;
        } else {
            TextButton.text = "chujowo";
        }

        //Debug.Log(RightController.transform.position);
    }
}
