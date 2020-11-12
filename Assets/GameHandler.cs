using System;
using System.Data.Common;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameHandler : MonoBehaviour
{

    public GameObject RightController;
    public GameObject LeftController;
    public Rigidbody Ball;
    public TextMesh RightControllerInfo; 
    public TextMesh LeftControllerInfo;
    public TextMesh TextButton;

    public GameObject round1Cube;
    public GameObject round2Cube;
    public GameObject round3Cube;

    private OrderedDictionary rounds = new OrderedDictionary();
    private int actualRound = 0;

    // Start is called before the first frame update
    void Start()
    {
        rounds.Add(0, round1Cube);
        rounds.Add(1, round2Cube);
        rounds.Add(2, round3Cube);
    }

    // Update is called once per frame
    void Update()
    {
        var rightPosition = RightController.transform.position.y + 0.4f;
        var leftPosition = LeftController.transform.position.y + 0.4f;
        RightControllerInfo.text = rightPosition.ToString();
        LeftControllerInfo.text = leftPosition.ToString();

        ((GameObject)rounds[(object)actualRound]).transform.Rotate(0.0f, 0.0f, 0.3f * rightPosition, Space.Self);
        ((GameObject)rounds[(object)actualRound]).transform.Rotate(0.3f * leftPosition, 0.0f, 0.0f, Space.Self);


        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, inputDevices);

        var device = inputDevices[0];

        bool triggerValue = false;
        device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue);
        if(triggerValue) {
            TextButton.text = "zajebiście";
            resetBall();
        } else {
            TextButton.text = "chujowo";
        }
        checkIfGameOver();
    }

    void resetBall() {
        Ball.transform.position = new Vector3(0.0f, 30.0f, 7.0f);
        Ball.velocity = Vector3.zero;
        Ball.angularVelocity = Vector3.zero;
    }

    void checkIfGameOver() {
        if (Ball.transform.position.y < -100.0f) {
            nextRound();
            TextButton.text = "Nastepna runda.";
            resetBall();
        }
    }

    void nextRound() {
        ((GameObject)rounds[(object)actualRound]).transform.position = new Vector3(0.0f, -4.0f, -30.0f);
        actualRound = (actualRound + 1) % 3; 
        ((GameObject)rounds[(object)actualRound]).transform.position = new Vector3(0.0f, -4.0f, 7.0f);
    }
}

