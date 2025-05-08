using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private PlayerMovement mov;
    private Animator anim;
    private SpriteRenderer spriteRend;

    [Header("Movement Tilt")]
    [SerializeField] private float maxTilt = 10f;
    [SerializeField][Range(0, 1)] private float tiltSpeed = 0.1f;

    public bool startedJumping { private get; set; }
    public bool justLanded { private get; set; }

    private void Start()
    {
        mov = GetComponent<PlayerMovement>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        anim = spriteRend.GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        //ApplyTilt();
        UpdateAnimatorParameters();
    }

    private void ApplyTilt()
    {
        float tiltProgress;
        int mult = -1;

        if (mov.IsSliding)
        {
            tiltProgress = 0.25f;
        }
        else
        {
            tiltProgress = Mathf.InverseLerp(-mov.Data.runMaxSpeed, mov.Data.runMaxSpeed, mov.RB.velocity.x);
            mult = (mov.IsFacingRight) ? 1 : -1;
        }

        float targetRot = ((tiltProgress * maxTilt * 2) - maxTilt);
        float smoothedRot = Mathf.LerpAngle(spriteRend.transform.localRotation.eulerAngles.z * mult, targetRot, tiltSpeed);
        spriteRend.transform.localRotation = Quaternion.Euler(0, 0, smoothedRot * mult);
    }

    private void UpdateAnimatorParameters()
    {
        // Устанавливаем скорость
        anim.SetFloat("Vel X", Mathf.Abs(mov.RB.velocity.x));
        anim.SetFloat("Vel Y", mov.RB.velocity.y);

        // Срабатывание триггеров
        if (startedJumping)
        {
            anim.SetTrigger("Jump");
            startedJumping = false;
        }

        if (justLanded)
        {
            anim.SetTrigger("Land");
            justLanded = false;
        }
    }
}