using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Teclas (cambialas en el Inspector)")]
    public KeyCode teclaIzquierda = KeyCode.UpArrow;   // flecha arriba = izquierda
    public KeyCode teclaDerecha   = KeyCode.Space;     // espacio = derecha
    public KeyCode teclaSalto1    = KeyCode.V;         // V = salto
    public KeyCode teclaSalto2    = KeyCode.P;         // P = salto

    [Header("Movimiento")]
    public float velocidad = 6f;
    public float fuerzaSalto = 10f;

    Rigidbody2D rb;
    BoxCollider2D col;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 3f;
        col.sharedMaterial = new PhysicsMaterial2D { friction = 0f, bounciness = 0f };
    }

    bool EnSuelo()
    {
        Bounds b = col.bounds;
        Vector2 centro = new Vector2(b.center.x, b.min.y - 0.05f);
        Vector2 tam = new Vector2(b.size.x * 0.9f, 0.1f);
        foreach (var c in Physics2D.OverlapBoxAll(centro, tam, 0f))
            if (c != col && !c.isTrigger) return true;
        return false;
    }

    void Update()
    {
        float x = 0f;
        if (Input.GetKey(teclaIzquierda)) x -= 1f;
        if (Input.GetKey(teclaDerecha))   x += 1f;
        rb.linearVelocity = new Vector2(x * velocidad, rb.linearVelocity.y);
        // En Unity 2022 o anterior cambia "linearVelocity" por "velocity"

        if ((Input.GetKeyDown(teclaSalto1) || Input.GetKeyDown(teclaSalto2)) && EnSuelo())
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
    }
}
