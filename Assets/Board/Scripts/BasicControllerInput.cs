using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Tracked Objects Include:
 * Head-mounted display
 * controllers
 ** tracked object movement, velocity, and angular velocity from the real world 
 ** are tracked and sent to the virtual world by the vive */

public class BasicControllerInput : MonoBehaviour {

    //reference to object being tracked (for example, the vive controllers)
    private SteamVR_TrackedObject trackedObj;

    //device property used to link the controller (via index) and return the controller's input
    private SteamVR_Controller.Device Controller 
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    //when the script is loaded, get the tracked object (a vive controller) from the component list
    //and assign it to trackedObj (the first variable created above)
    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Update is called once per frame
    void Update () {
		
        //controller input output to the console
        
        // When the touchpad registers a position that is not the origin,
        // output the name of the object (controller left or right) and the (x,y) coords
        if (Controller.GetAxis() != Vector2.zero)
            Debug.Log(gameObject.name + Controller.GetAxis());

        /*** Hair Trigger (Bottom [main] Trigger) ***/

        if (Controller.GetHairTriggerDown())
            Debug.Log(gameObject.name + "Hair Trigger Pressed");

        if (Controller.GetHairTriggerUp())
            Debug.Log(gameObject.name + "Hair Trigger Released");

        /*** Grip Button (Side Controller Button) ***/

        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            Debug.Log(gameObject.name + "Grib Button Pressed");

        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            Debug.Log(gameObject.name + "Grip Button Released");

	}
}
