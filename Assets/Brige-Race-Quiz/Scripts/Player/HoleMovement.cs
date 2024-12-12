using System.Collections.Generic;
using Brige_Race_Quiz.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace Brige_Race_Quiz.Scripts.Player
{
	public class HoleMovement : MonoBehaviour
	{
		public InputController inputController;
		public FxManager fxManager;

		[Space]
		[Header ("Hole mesh")]
		[SerializeField] private MeshFilter meshFilter;
		[SerializeField] private MeshCollider meshCollider;

		[Space]
		[Header ("Hole vertices radius")]
		[SerializeField] private Vector2 moveLimits;
		//Hole vertices radius from the hole's center
		[SerializeField] private float radius;
		[SerializeField] private Transform holeCenter;
		//rotating circle arround hole (animation)
		[SerializeField] private Transform rotatingCircle;

		[Space]
		[Header ("Hole Properties")]
		[SerializeField] private float moveSpeed;

		// private values
		private Mesh _mesh;
		private Vector3 _initialPoint;
		private readonly List<int> _holeVertices = new List<int> ();
		private readonly List<Vector3> _offsets = new List<Vector3> ();
		
		private int _holeVerticesCount;
		private Vector3 _touch, _targetPos;

		void Start ()
		{
			_mesh = meshFilter.mesh;
			_initialPoint = transform.localPosition;
			
			RotateCircleAnim();
			FindHoleVertices();

		}

		void Update ()
		{
			if (Game.isGameover) return;
			inputController.Update();

			if (!Game.isMoving) return;
				
			MoveHole ();
			UpdateHoleVerticesPosition ();
		}

		void MoveHole ()
		{
			_touch = Vector3.Lerp (
				holeCenter.position, 
				holeCenter.position + inputController.InputVector, 
				moveSpeed * Time.deltaTime
			);

			_targetPos = new Vector3 (
				//Clamp: to prevent hole from going outside of the ground
				Mathf.Clamp (_touch.x, -moveLimits.x, moveLimits.x),//limit X
				_touch.y,
				Mathf.Clamp (_touch.z, -moveLimits.y, moveLimits.y)//limit Z
			);

			holeCenter.position = _targetPos;
		}

		void UpdateHoleVerticesPosition ()
		{
			//Move hole vertices
			Vector3[] vertices = _mesh.vertices;
			for (int i = 0; i < _holeVerticesCount; i++) {
				vertices [_holeVertices [i]] = holeCenter.position + _offsets [i];
			}

			//update mesh vertices
			_mesh.vertices = vertices;
			//update meshFilter's mesh
			meshFilter.mesh = _mesh;
			//update collider
			meshCollider.sharedMesh = _mesh;
		}

		void RotateCircleAnim ()
		{
			//rotate circle arround Y axis by -90°
			//duration: 0.2 seconds
			//start: Vector3 (90f, 0f, 0f)
			//loop: -1 (infinite)
			rotatingCircle
				.DORotate (new Vector3 (90f, 0f, -90f), .2f)
				.SetEase (Ease.Linear)
				.From (new Vector3 (90f, 0f, 0f))
				.SetLoops (-1, LoopType.Incremental);
			fxManager.CreateEffects();
		}

		void FindHoleVertices ()
		{
			for (int i = 0; i < _mesh.vertices.Length; i++) {
				//Calculate distance between holeCenter & each Vertex
				float distance = Vector3.Distance (holeCenter.position, _mesh.vertices [i]);

				if (distance < radius) {
					//this vertex belongs to the Hole
					_holeVertices.Add (i);
					//offset: how far the Vertex from the HoleCenter
					_offsets.Add (_mesh.vertices [i] - holeCenter.position);
				}
			}
			//save hole vertices count
			_holeVerticesCount = _holeVertices.Count;
		}

		public void ResetPlayer()
		{
			transform.localPosition = _initialPoint;
			UpdateHoleVerticesPosition ();
		}

		//Visualize Hole vertices Radius in the Scene view
		void OnDrawGizmos ()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere (holeCenter.position, radius);
		}
	}
}
