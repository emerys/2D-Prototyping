using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
	private Vector3 _initialPosition;
	private bool _birdWasLaunched = false;
	private float _timeSittingAround = 0f;
	private LineRenderer _lineRenderer;

	[SerializeField] private float _launchPower = 500f;

	private void Awake() {
		_initialPosition = transform.position;
		_lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update() {
		if (_birdWasLaunched && GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1) {
			_timeSittingAround += Time.deltaTime;
		}

		if (transform.position.y > 10 || transform.position.y < -10 || 
			transform.position.x < -20 || transform.position.x > 20 ||
			_timeSittingAround > 3f) {
			string currentStringName = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene(currentStringName);
		}

		if(_lineRenderer) {
			_lineRenderer.SetPosition(0, transform.position);
			_lineRenderer.SetPosition(1, _initialPosition);
		}


	}

	private void OnMouseDown() {
		GetComponent<SpriteRenderer>().color = Color.red;
		if(_lineRenderer)
			_lineRenderer.enabled = true;
	}

	private void OnMouseUp() {
		GetComponent<SpriteRenderer>().color = Color.white;

		Vector2 directionToInitalPosition = _initialPosition - transform.position;
		directionToInitalPosition *= Vector2.Distance(_initialPosition, transform.position) * _launchPower;
		GetComponent<Rigidbody2D>().AddForce(directionToInitalPosition);

		GetComponent<Rigidbody2D>().gravityScale = 1.0f;
		_birdWasLaunched = true;
		if (_lineRenderer && _lineRenderer.enabled)
			_lineRenderer.enabled = false;
	}

	private void OnMouseDrag() {
		Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		newPosition.z = 0.0f; // remove Z-transform
		transform.position = newPosition;
	}
}
