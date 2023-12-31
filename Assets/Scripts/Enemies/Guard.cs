﻿using System.Collections;
using UnityEngine;

public class Guard : MonoBehaviour {
	
	public float speed = 1;
	public float waitTime = .3f;
	public float turnSpeed = 90;
	public float timeToSpotPlayer = .5f;

	public Light spotlight;
	public float viewDistance;
	public LayerMask viewMask;
	private Animator animator;

	float viewAngle;
	float playerVisibleTimer;

	public Transform pathHolder;
	Transform player;
	Color originalSpotlightColour;

	void Start() {
		animator = GetComponent<Animator>();
    animator.SetBool("IsSitting", false);
    animator.SetBool("IsWalking", true);
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		viewAngle = spotlight.spotAngle;
		originalSpotlightColour = spotlight.color;

		Vector3[] waypoints = new Vector3[pathHolder.childCount];
		for (int i = 0; i < waypoints.Length; i++) {
			waypoints [i] = pathHolder.GetChild (i).position;
			waypoints [i] = new Vector3 (waypoints [i].x, transform.position.y, waypoints [i].z);
		}

		StartCoroutine (FollowPath (waypoints));
	}

	void Update() {
		if (!animator.GetBool("IsSitting")) {
			HandlePlayerVisibility();

			if (playerVisibleTimer >= timeToSpotPlayer) {
				SceneManagerScript.Instance.stateManagerScript.TakeDamage();
			}
		}
	}

	void HandlePlayerVisibility() {
		if (CanSeePlayer ()) {
			playerVisibleTimer += Time.deltaTime;
		} else {
			playerVisibleTimer -= Time.deltaTime;
		}

		playerVisibleTimer = Mathf.Clamp (playerVisibleTimer, 0, timeToSpotPlayer);
		spotlight.color = Color.Lerp (originalSpotlightColour, Color.red, playerVisibleTimer / timeToSpotPlayer);
	}

	bool CanSeePlayer() {
		if (Vector3.Distance(transform.position,player.position) < viewDistance) {
			Vector3 dirToPlayer = (player.position - transform.position).normalized;
			float angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
			if (angleBetweenGuardAndPlayer < viewAngle / 2f) {
				if (!Physics.Linecast (transform.position, player.position, viewMask)) {
					return true;
				}
			}
		}
		return false;
	}

	IEnumerator FollowPath(Vector3[] waypoints) {
		transform.position = waypoints [0];

		int targetWaypointIndex = 1;
		Vector3 targetWaypoint = waypoints [targetWaypointIndex];
		transform.LookAt (targetWaypoint);

		while (!animator.GetBool("Kill")) {
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, speed * Time.deltaTime);
			if (Mathf.Abs(Vector3.Distance(transform.position, targetWaypoint)) > 30) transform.position = targetWaypoint;
			if (Mathf.Abs(Vector3.Distance(transform.position, targetWaypoint)) <= 0.5)
				transform.position = targetWaypoint;
			if (transform.position == targetWaypoint) {
				targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints [targetWaypointIndex];
    			animator.SetBool("IsWalking", false);
            	float randomWait = Random.Range(0, waitTime);
            	if (randomWait >= waitTime - 1)
            	{
            		spotlight.gameObject.SetActive(false);
					animator.SetBool("IsSitting", true);
					yield return new WaitForSeconds (randomWait);
					animator.SetBool("IsSitting", false);
					spotlight.gameObject.SetActive(true);
				} else {
					yield return new WaitForSeconds (randomWait);	
				}
            	
				yield return StartCoroutine (TurnToFace (targetWaypoint));
				animator.SetBool("IsWalking", true);
			}
			yield return null;
		}
	}

	IEnumerator TurnToFace(Vector3 lookTarget) {
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f && !animator.GetBool("Kill")) {
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}
	}

	void OnDrawGizmos() {
		Vector3 startPosition = pathHolder.GetChild (0).position;
		Vector3 previousPosition = startPosition;

		foreach (Transform waypoint in pathHolder) {
			Gizmos.DrawSphere (waypoint.position, .3f);
			Gizmos.DrawLine (previousPosition, waypoint.position);
			previousPosition = waypoint.position;
		}
		Gizmos.DrawLine (previousPosition, startPosition);

		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position, transform.forward * viewDistance);
	}
}
