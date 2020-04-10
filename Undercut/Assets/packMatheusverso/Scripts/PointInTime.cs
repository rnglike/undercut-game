using UnityEngine;

public class PointInTime {

	// Object to save momentum's Game Objects

	public Vector3 position;
	public Quaternion rotation;
	public Vector2 rbVelocity;

	// Constructor
	public PointInTime (Vector3 _position, Quaternion _rotation, Vector2 _rbVelocity)
	{
		position = _position;
		rotation = _rotation;
		rbVelocity = _rbVelocity;
	}

}
