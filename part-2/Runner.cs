using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour {

    Animator _animator;
    public AnimationCurve jumpCurve;
    float jumpTimer;
    float yPos = 0f;
    bool _jumping;

    public float[] xPos;
    int xPosIndex = 1;
    public float speed = 5f;
    public float floorHeight;

    private void Start() {

        _animator = GetComponent<Animator>();
        Begin();

    }

    public void Begin () {

        _animator.SetBool("Running", true);

    }

    private void Update() {

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

}
