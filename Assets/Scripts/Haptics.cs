#define DEBUG_Haptic

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Haptics : MonoBehaviour {

	public SteamVR_Action_Vibration hapticAction = SteamVR_Input.GetAction<SteamVR_Action_Vibration>("Haptic");
	public SteamVR_Action_Boolean trackpadAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");


	void Update () {
		
		if (trackpadAction.GetStateDown (SteamVR_Input_Sources.LeftHand)) {
			Pulse (1, 150, 75, SteamVR_Input_Sources.LeftHand);
		}
		
		if (trackpadAction.GetStateDown (SteamVR_Input_Sources.RightHand)) {
			Pulse (1, 150, 75, SteamVR_Input_Sources.RightHand);
		}

	}


	private void Pulse (float duration, float frequency, float amplitude, SteamVR_Input_Sources source) {
		hapticAction.Execute (0, duration, frequency, amplitude, source);
		#if DEBUG_Haptic
		Debug.Log ("Pulse " + source.ToString());
		#endif
	} 

}
