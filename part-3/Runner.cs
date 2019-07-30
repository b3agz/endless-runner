using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {

    Animator _animator;
    public AnimationCurve jumpCurve;
    public Rigidbody _rigidBody;
    float jumpTimer;
    float yPos = 0f;
    bool _jumping;
    public bool hasDied = false;

    public float[] xPos;
    int xPosIndex = 1;
    public float speed = 5f;
    public float floorHeight;

    private void Start() {

        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        Begin();

    }

    public void Begin () {

        _animator.SetBool("Running", true);

    }

    private void Update() {

        if (hasDied)
            return;

        if (Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.D))
            MoveRight();
        if (!jumping && Input.GetKeyDown(KeyCode.Space))
            jumping = true;

        if (jumping) {

            yPos = jumpCurve.Evaluate(jumpTimer);
            jumpTimer += Time.deltaTime;

            if (jumpTimer > 1f) {

                jumpTimer = 0;
                jumping = false;

            }

        }

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(xPos[xPosIndex], floorHeight + yPos, transform.position.z), Time.deltaTime * speed);

    }

    bool jumping {

        get { return _jumping; }

        set {

            _jumping = value;
            _animator.SetBool("Jumping", value);

        }

    }

    void Die () {

        _animator.SetTrigger("Die");
        hasDied = true;

    }

    void MoveLeft () {

        xPosIndex--;
        if (xPosIndex < 0)
            xPosIndex = 0;

    }

    void MoveRight () {

        xPosIndex++;
        if (xPosIndex > xPos.Length - 1)
            xPosIndex = xPos.Length - 1;

    }

    private void OnTriggerEnter(Collider other) {

        if (hasDied)
            return;

        if (other.tag == "Trap")
            Die();

    }

}
