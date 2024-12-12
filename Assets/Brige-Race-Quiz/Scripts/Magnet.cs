using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Brige_Race_Quiz.Scripts
{
	[RequireComponent (typeof(SphereCollider))]
	public class Magnet : Singleton<Magnet>
	{
		[SerializeField] float magnetForce;

		private readonly List<Rigidbody> _affectedRigidbodies = new List<Rigidbody> ();
		private Transform _magnet;

		private void Start ()
		{
			_magnet = transform;
			RestartMagnet();
		}

		public void RestartMagnet()
		{
			_affectedRigidbodies.Clear ();
		}

		private void FixedUpdate ()
		{
			if (!Game.isGameover && Game.isMoving) {
				foreach (var rb in _affectedRigidbodies) {
					rb.AddForce ((_magnet.position - rb.position) * (magnetForce * Time.fixedDeltaTime));
				}
			}
		}
		private void OnTriggerEnter (Collider other)
		{
			if (CanBeAdd(other.tag)) {
				AddToMagnetField (other.attachedRigidbody);
			}
		}

		private void OnTriggerExit (Collider other)
		{
			if (CanBeAdd(other.tag)) {
				RemoveFromMagnetField (other.attachedRigidbody);
			}
		}

		private void AddToMagnetField (Rigidbody rb)
		{
			_affectedRigidbodies.Add (rb);
		}

		public void RemoveFromMagnetField (Rigidbody rb)
		{
			_affectedRigidbodies.Remove (rb);
		}

		private bool CanBeAdd(string s) => !Game.isGameover && (s.Equals("Obstacle") || s.Equals("Object"));
	}
}
