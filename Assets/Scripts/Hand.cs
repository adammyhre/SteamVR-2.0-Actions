#define DEBUG_Pickup

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour {

	public SteamVR_Action_Boolean grabAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

	private SteamVR_Behaviour_Pose trackedObj = null;
	private FixedJoint joint = null;

	private Interactable currentInteractable;
	public List<Interactable> contactInteractables = new List<Interactable>(); 


	private void Awake () {
		trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
		joint = GetComponent<FixedJoint>();
	}
	
	private void Update () {

		// Pickup and Drop Object code
		// Trigger Down
		if (grabAction.GetStateDown(trackedObj.inputSource)) {
			#if DEBUG_Pickup
			Debug.Log (trackedObj.inputSource + " trigger down.");
			#endif
			PickUp();
		}

		// Trigger Up
		if (grabAction.GetStateUp(trackedObj.inputSource)) {
			#if DEBUG_Pickup
			Debug.Log (trackedObj.inputSource + " trigger up.");
			#endif
			Drop();
		}

	}

	private void OnTriggerEnter (Collider other) {
		if (!other.gameObject.CompareTag("Interactable"))
			return;

		contactInteractables.Add(other.gameObject.GetComponent<Interactable>());
	}

	private void OnTriggerExit (Collider other) {
		if (!other.gameObject.CompareTag("Interactable"))
			return;

		contactInteractables.Remove(other.gameObject.GetComponent<Interactable>());
	}

	public void PickUp () {

	}

	public void Drop () {

	}

	private Interactable GetNearestInteractable () {
		return null;
	}

}

