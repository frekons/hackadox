using UnityEngine;

public class TextureOffsetLooper : MonoBehaviour
{
	public Vector2 Limit;

	[SerializeField]
	private float _xSpeed, _ySpeed;

	Material _material;

	void Start()
	{
		_material = GetComponent<Renderer>().material;
	}

	void Update()
	{
		if (_material)
		{
			var result = _material.GetTextureOffset("_MainTex") + new Vector2(Time.deltaTime * _xSpeed, Time.deltaTime * _ySpeed);

			_material.SetTextureOffset("_MainTex", normalize(result));
		}
	}

	Vector2 normalize(Vector2 vector)
	{
		float different = Limit.y - Limit.x;

		if (vector.x < Limit.x)
			vector.x += different;

		if (vector.x > Limit.y)
			vector.x -= different;

		if (vector.y < Limit.x)
			vector.y += different;

		if (vector.y > Limit.y)
			vector.y -= different;

		return vector;
	}
}
