#define DEBUG_Pickup

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour {

	public SteamVR_Action_Boolean triggerAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
	public SteamVR_Action_Boolean touchpadAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");

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
		if (triggerAction.GetStateDown(trackedObj.inputSource)) {
			#if DEBUG_Pickup
			Debug.Log (trackedObj.inputSource + " trigger down.");
			#endif

			if (currentInteractable != null){
				currentInteractable.Action ();
				return;
			}
			PickUp();
		}

		// Touchpad Down
		if (touchpadAction.GetStateDown(trackedObj.inputSource)) {
			#if DEBUG_Pickup
			Debug.Log (trackedObj.inputSource + " touchpad down.");
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

		// Get nearest
		currentInteractable = GetNearestInteractable();

		// Null check
		if (!currentInteractable) return;
			
		// Already Held check
		if (currentInteractable.activeHand)
			currentInteractable.activeHand.Drop ();
		
		// Position to controller
		currentInteractable.ApplyOffset(transform);

		// Attach rigid body to the joint
		Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
		joint.connectedBody = targetBody;

		// Set Active Hand variable
		currentInteractable.activeHand = this;

	}

	public void Drop () {
		
		// Mull check
		if (!currentInteractable) return;

		// Apply velocity
		Rigidbody targetBody = currentInteractable.GetComponent<Rigidbody>();
		targetBody.velocity = trackedObj.GetVelocity ();
		targetBody.angularVelocity = trackedObj.GetAngularVelocity ();

		// Detach from joint
		joint.connectedBody = null;

		// Clear variables
		currentInteractable.activeHand = null;
		currentInteractable = null;


	}

	private Interactable GetNearestInteractable () {

		Interactable nearest = null;
		float minDistance = float.MaxValue;
		float distance = 0.0f;

		foreach (Interactable interactable in contactInteractables) {
			distance = (interactable.transform.position - transform.position).sqrMagnitude;

			if (distance < minDistance) {
				minDistance = distance;
				nearest = interactable;
			}
		}

		return nearest;
	}

}

