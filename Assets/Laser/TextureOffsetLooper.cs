using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureOffsetLooper : MonoBehaviour
{
    public Vector2 Limit;
    public float XSpeed, YSpeed;

    Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (_material)
        {
            var result = _material.GetTextureOffset("_MainTex") + new Vector2(Time.deltaTime * XSpeed, Time.deltaTime * YSpeed);

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
