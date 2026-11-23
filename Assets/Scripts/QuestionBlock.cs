using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class QuestionBlock : MonoBehaviour
{
    public AudioClip sonido;
    public bool unSoloUso = false;   // true = como Mario (solo suena una vez)
    bool usado;
    Vector3 posInicial;

    void Awake() { posInicial = transform.position; }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.GetComponent<PlayerController>() == null) return;
        if (usado && unSoloUso) return;
        // Solo si el jugador golpea desde abajo
        if (c.transform.position.y > transform.position.y) return;

        GetComponent<AudioSource>().PlayOneShot(sonido);
        StopAllCoroutines();
        StartCoroutine(Rebote());
        usado = true;
        if (unSoloUso) GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.4f, 0.2f);
    }

    IEnumerator Rebote()
    {
        float t = 0f;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            transform.position = posInicial + Vector3.up * Mathf.Sin(t / 0.15f * Mathf.PI) * 0.25f;
            yield return null;
        }
        transform.position = posInicial;
    }
}
