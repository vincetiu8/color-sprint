using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    //[SerializeField] private float maxStrength, waitTime, loadTime;

    private Coroutine glow;
    private Material mat;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;

        //if (glow == null)
        //{
        //    glow = StartCoroutine(Glow());
        //}
    }

    public void setColor(Color _color)
    {
        mat.color = _color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().UpdateColor(mat.color);
        }
    }

    //private IEnumerator Glow()
    //{
    //    while (true)
    //    {
    //        for (float i = 0; i < loadTime; i += Time.deltaTime)
    //        {
    //            mat.color.;
    //            print(mat.color * i * maxStrength / loadTime);
    //        }

    //        yield return new WaitForSeconds(waitTime);

    //        for (float i = loadTime; i > 0; i -= Time.deltaTime)
    //        {
    //            mat.SetColor("_EmissionColor", mat.color * i * maxStrength / loadTime);
    //        }

    //        yield return new WaitForSeconds(waitTime);
    //    }
    //}
}
