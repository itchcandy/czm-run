using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float minX = -3, maxX = 3, handling = 10f;
    public GameManager gm;
    public Looper looper;
    bool lerpHori = false;
    Vector3 targetPosition, sourcePosition;
    Rigidbody rb;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

	void Start ()
    {
	
	}
	
	void Update ()
    {
        if (rb.IsSleeping())
            rb.WakeUp();
        Move();
	}

    public void Reset()
    {
        lerpHori = false;
        transform.position = Vector3.zero;
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Obstacle")
            gm.HitObstacle();
        else if (c.tag == "Currency")
            gm.HitCurrency();
        else if (c.tag == "Fuel")
            gm.HitFuel();
        else if (c.tag == "Shield")
            gm.HitShield();
        looper.Reset(c.gameObject);
    }

    void Move()
    {
        float f = Input.GetAxis("Horizontal");
        if (f != 0)
        {
            if (f < 0)
            {
                anim.ResetTrigger("Right");
                anim.SetTrigger("Left");
            }
            else if (f > 0)
            {
                anim.ResetTrigger("Left");
                anim.SetTrigger("Right");
            }
            sourcePosition = transform.position;
            targetPosition = transform.position + Vector3.right * f * Time.deltaTime * handling;
            if (targetPosition.x < minX)
                targetPosition.x = minX;
            else if (targetPosition.x > maxX)
                targetPosition.x = maxX;
            transform.position = Vector3.Lerp(sourcePosition, targetPosition, 0.2f);
        }
    }
}
