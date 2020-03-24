using System.Collections;
using UnityEngine;

public class Shell : MonoBehaviour
{
    public Rigidbody _rb;
    public float forceMin;
    public float forceMax;

    public float lifetime = 4.0f;
    public float fadeTime = 2.0f;

    private void Start()
    {
        float force = Random.Range(forceMin, forceMax);
        _rb.AddForce(transform.right * force);
        _rb.AddTorque(Random.insideUnitSphere * force);
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(lifetime);

        float percent = 0;
        float fadeSpeed = 1 / fadeTime;
        Material mat = GetComponent<Renderer>().material;
        Color initialColor = mat.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(initialColor, Color.clear, percent);
            yield return null;
        }
        Destroy(gameObject);
    }
}
