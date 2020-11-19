
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System;
using System.Data.Common;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameHandler : MonoBehaviour
{
    private float MAX_ROTATION = 25;
    public GameObject RightController;
    public GameObject LeftController;
    public Rigidbody Ball;

    public GameObject gameCourt1;
    public GameObject gameCourt2;
    public GameObject gameCourt3;

    public TextMesh rightDebug;
    public TextMesh leftDebug;

    private OrderedDictionary rounds = new OrderedDictionary();
    private int actualRound = 0;
    private GameObject actualGameCourt;
    private Vector3 gameCourtInitialPosition = new Vector3(0.0f, -4.0f, 7.0f);

    private ControllerWrapper rightControllerWrapper;
    private ControllerWrapper leftControllerWrapper;

    IEnumerator Log() {
        while (true) {
            rightDebug.text = rightControllerWrapper.getPosition().ToString();
            leftDebug.text = leftControllerWrapper.getPosition().ToString();

            updateRotation(rightControllerWrapper);
            updateRotation(leftControllerWrapper);

            yield return new WaitForSeconds(.05f);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rounds.Add(0, gameCourt1);
        rounds.Add(1, gameCourt2);
        rounds.Add(2, gameCourt3);
        
        actualGameCourt = Instantiate(gameCourt1, gameCourtInitialPosition, Quaternion.identity);

        rightControllerWrapper = new ControllerWrapper(RightController, "right");
        leftControllerWrapper = new ControllerWrapper(LeftController, "left");

        StartCoroutine("Log");

    }

    // Update is called once per frame
    void Update()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithRole(UnityEngine.XR.InputDeviceRole.RightHanded, inputDevices);

        var device = inputDevices[0];

        bool triggerValue = false;
        device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerValue);
        if(triggerValue) {
            resetBall();
        } else {
        }
        checkIfGameOver();
    }

    void updateRotation(ControllerWrapper controller) {
        if(controller.isRotationAllowed(actualGameCourt.transform.rotation.eulerAngles)) {
            actualGameCourt.transform.Rotate(controller.getRotationVector(), Space.Self);
        }
    }

    void resetBall() {
        Ball.transform.position = new Vector3(0.0f, 30.0f, 7.0f);
        Ball.velocity = Vector3.zero;
        Ball.angularVelocity = Vector3.zero;
    }

    void checkIfGameOver() {
        if (Ball.transform.position.y < -100.0f) {
            nextRound();
            resetBall();
        }
    }

    void nextRound() {
        Destroy(actualGameCourt);
        actualRound = (actualRound + 1) % 3; 
        actualGameCourt = Instantiate(((GameObject)rounds[(object)actualRound]), gameCourtInitialPosition, Quaternion.identity);
    }
}

public class ControllerWrapper {

    private float MAX_ROTATION = 25;
    private GameObject controller;
    private String controllerName;
    private float rotationStepDeg = 0.3f;
    
    public ControllerWrapper(GameObject controller, String controllerName) {
        this.controller = controller;
        this.controllerName = controllerName;
    }

    public ControllerPosition getPosition() {
        if((controller.transform.position.y + 0.2f) > 0.1) {
            return ControllerPosition.UP;
        } else if ((controller.transform.position.y + 0.2f) < -0.1) {
            return ControllerPosition.DOWN;
        } else {
            return ControllerPosition.ZERO;
        }
    }

    public float getRotationStep() {
        return rotationStepDeg;
    }

    public Vector3 getRotationVector() {
        if (controllerName == "right") {
            return new Vector3(0.0f, 0.0f, ((int)getPosition()) * rotationStepDeg);
        } else {
            return new Vector3(((int)getPosition()) * rotationStepDeg, 0.0f, 0.0f);
        }
    }

    public bool isRotationAllowed(Vector3 actualGameCourtRotation) {
        if (controllerName == "right") {
            var newRotation = (actualGameCourtRotation.z + ((int)getPosition()) * getRotationStep());
            return (newRotation < MAX_ROTATION || (360 - newRotation < MAX_ROTATION));
        } else {
            var newRotation = (actualGameCourtRotation.x + ((int)getPosition()) * getRotationStep());
            return (newRotation < MAX_ROTATION || (360 - newRotation < MAX_ROTATION));
        }
    }


    

}

public enum ControllerPosition {
    UP = 1, 
    ZERO = 0, 
    DOWN = -1,
} 