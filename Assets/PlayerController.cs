using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool dead, movable;
    public float speed;

    [SerializeField] private RectTransform[] points;
    private Material mat;
    private int position;
    private Rigidbody rb;
    private Coroutine deathAnim, move;

    // Start is called before the first frame update
    void Start()
    {
        position = 4;
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().material;
        dead = false;
        movable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead && deathAnim == null)
        {
            deathAnim = StartCoroutine(DeathAnim());
        }

        if (movable && move == null)
        {
            SelectMove();
        }
        print(rb.velocity);
    }

    public void UpdateColor(Color _color)
    {
        // Red is Temp
        // Green is Nat
        // Blue is Water
        mat.color += (_color - Color.gray) * 0.2f;
        if (mat.color.r <= 0 || mat.color.r >= 1 || mat.color.g <= 0 || mat.color.g >= 1 || mat.color.b <= 0 || mat.color.b >= 1)
        {
            dead = true;
        }
        points[0].anchoredPosition = new Vector2(Mathf.Clamp(0.5f - mat.color.r, -0.5f, 0.5f) * 400, 0);
        points[1].anchoredPosition = new Vector2(Mathf.Clamp(0.5f - mat.color.g, -0.5f, 0.5f) * 400, 0);
        points[2].anchoredPosition = new Vector2(Mathf.Clamp(mat.color.b - 0.5f, -0.5f, 0.5f) * 400, 0);
    }


    private void SelectMove()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            move = StartCoroutine(Move(Vector3.forward));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            move = StartCoroutine(Move(Vector3.back));
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            move = StartCoroutine(Move(Vector3.left));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            move = StartCoroutine(Move(Vector3.right));
        }
    }

    private IEnumerator Move(Vector3 _direction)
    {
        if (position == 0 && _direction == Vector3.left || position == 8 && _direction == Vector3.right)
        {
            yield break;
        }

        else
        {
            position += (int) _direction.x;
        }

        float _sum = 0;
        while (_sum < 1 - speed * Time.deltaTime)
        {
            transform.Translate(_direction * speed * Time.deltaTime);
            _sum += speed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z));

        move = null;
    }

    private IEnumerator DeathAnim()
    {
        movable = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.AddForce(Vector3.up * 250);

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
