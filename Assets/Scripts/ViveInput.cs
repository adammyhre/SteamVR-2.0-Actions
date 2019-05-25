using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour {

	public SteamVR_Action_Boolean grabAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
	public SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");
	public SteamVR_Action_Single squeezeAction = SteamVR_Input.GetAction<SteamVR_Action_Single>("Squeeze");
	public SteamVR_Action_Vector2 touchPadAction = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("TouchpadTouch");

	void Update () {

		if (teleportAction.GetStateDown(SteamVR_Input_Sources.Any)){
			print("Teleport down.");
		}
		if (grabAction.GetStateUp(SteamVR_Input_Sources.Any)){
			print("Grab Pinch up.");
		}

		float triggerValue = squeezeAction.GetAxis (SteamVR_Input_Sources.Any);

		if (triggerValue > 0.0f) {
			print(triggerValue);
		
		}
	
		Vector2 touchPadValue = touchPadAction.GetAxis (SteamVR_Input_Sources.Any);

		if (touchPadValue != Vector2.zero) {
			print(touchPadValue);

		}

	}

}
