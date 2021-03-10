using UnityEngine;

public class SwingAxe : MonoBehaviour
{
	public AnimationCurve SwingPattern;
	public float HalfArcPerSecond = 25f;
	public float AngleForAudio = 2f;

	AudioSource _audioSource;
	float _timeModifier;
	float _elapsedTime;
	float _swingSize;
	int _direction;


	void Start ()
	{
		_audioSource = GetComponent<AudioSource>();

		float currentAngle = transform.rotation.eulerAngles.z;

		_timeModifier = Mathf.Abs(currentAngle) / HalfArcPerSecond;
		_swingSize = Mathf.Abs(currentAngle);

		if (currentAngle > 0f)
			_direction = 1;
		else
			_direction = -1;
	}

	void Update()
	{
		_elapsedTime += Time.deltaTime / _timeModifier;

		if (_elapsedTime >= 1f)
			_elapsedTime -= 1f;

		float angle = SwingPattern.Evaluate(_elapsedTime) * _swingSize * _direction;

		if (angle < AngleForAudio && angle > -AngleForAudio)
			_audioSource.Play();

		Vector3 rot = transform.rotation.eulerAngles;
		rot.z = angle;
		transform.rotation = Quaternion.Euler(rot);
	}
}
