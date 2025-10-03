using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class OnceParticleSystemPlayer : MonoBehaviour
{
    [SerializeField] private bool isNullParent = true;
    [SerializeField] private float duration;

    public void Play()
    {
        gameObject.SetActive(true);
        if(isNullParent)
        {
            transform.SetParent(null);
		}
        StartCoroutine(PlayEffect());
	}

    private IEnumerator PlayEffect()
    {
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
	}
}
