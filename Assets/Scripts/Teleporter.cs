using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour {

	public GameObject pointer;
	public SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");

	private SteamVR_Behaviour_Pose trackedObj = null;

	private bool hasPosition = false;

	private float fadeTime = 0.5f;
	private bool isTeleporting = false;


	private void Awake () {
		trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
	}
	
	private void Update () {

		// Pointer
		hasPosition = UpdatePointer ();
		pointer.SetActive (hasPosition);

		// Teleport
		if (teleportAction.GetStateUp(trackedObj.inputSource)) {
			TryTeleport ();
		}

	}

	private void TryTeleport () {

		// Check boolean values to see if we have a position or we are already teleporting
		if (isTeleporting || !hasPosition) {
			return;
		}

		// Get camera rig and head position
		Transform cameraRig = SteamVR_Render.Top ().origin;
		Vector3 headPosition = SteamVR_Render.Top ().head.position;

		// Calculate translation
		Vector3 groundPosition = new Vector3 (headPosition.x, cameraRig.position.y, headPosition.z);
		Vector3 translateVector = pointer.transform.position - groundPosition;

		// Move
		StartCoroutine(MoveRig(cameraRig, translateVector));

	}

	private IEnumerator MoveRig (Transform cameraRig, Vector3 translation) {

		// Flag
		isTeleporting = true;

		// Fade to black
		SteamVR_Fade.Start(Color.black, fadeTime, true);

		// Apply translation
		yield return new WaitForSeconds(fadeTime);
		cameraRig.position += translation;

		// Fade to clear
		SteamVR_Fade.Start(Color.clear, fadeTime, true);

		// Remove flag
		isTeleporting = false;

	} 

	private bool UpdatePointer () {

		// Ray coming from controller
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		// Check if is hit
		if (Physics.Raycast (ray, out hit)) {
			pointer.transform.position = hit.point;
			return true;
		}

		// Return false if not a hit
		return false;

	}

}
