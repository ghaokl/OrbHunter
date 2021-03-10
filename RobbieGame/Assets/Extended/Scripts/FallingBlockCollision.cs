using UnityEngine;

public class FallingBlockCollision : MonoBehaviour
{
	Rigidbody2D _rigidBody;
	BoxCollider2D _box;
	AudioSource _audioSource;
	LayerMask _groundMask;
	int _groundLayer;


	void Start()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
		_box = GetComponent<BoxCollider2D>();
		_audioSource = GetComponent<AudioSource>();

		_groundMask = LayerMask.GetMask("Platforms");
		_groundLayer = LayerMask.NameToLayer("Platforms");
	}

	public void Fall()
	{
		_rigidBody.bodyType = RigidbodyType2D.Dynamic;
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer != _groundLayer)
			return;

		Vector3 pos = _rigidBody.position;
		RaycastHit2D hit;

		hit = Physics2D.Raycast(pos, Vector2.down, 1f, _groundMask);
		pos.y = hit.point.y + .5f;
		transform.position = pos;

		_box.usedByComposite = true;
		Destroy(_rigidBody);

		_audioSource.Play();
	}
}
