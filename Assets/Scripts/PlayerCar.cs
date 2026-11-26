using System.Collections;
using UnityEngine;

// O'yinchi mashinasi: W/S - oldinga/orqaga, A/D - burilish (yoki strelkalar)
[RequireComponent(typeof(Rigidbody))]
public class PlayerCar : MonoBehaviour
{
    public float speed = 14f;
    public float turnSpeed = 120f;
    public Renderer bodyRenderer; // zarba olganda qizarishi uchun

    Rigidbody rb;
    float moveInput;
    float turnInput;
    Color normalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        normalColor = bodyRenderer.material.color;
    }

    void Update()
    {
        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        // Oldinga harakat
        Vector3 velocity = transform.forward * moveInput * speed;
        velocity.y = rb.velocity.y;
        rb.velocity = velocity;

        // Burilish (orqaga yurganda teskari buriladi, xuddi haqiqiy mashinadek)
        float direction = moveInput >= 0 ? 1f : -1f;
        float turn = turnInput * turnSpeed * direction * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turn, 0f));
    }

    // Dushman urganda mashina qisqa vaqt qizarib ketadi
    public void ShowHit()
    {
        StopAllCoroutines();
        StartCoroutine(HitFlash());
    }

    IEnumerator HitFlash()
    {
        bodyRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        bodyRenderer.material.color = normalColor;
    }
}
