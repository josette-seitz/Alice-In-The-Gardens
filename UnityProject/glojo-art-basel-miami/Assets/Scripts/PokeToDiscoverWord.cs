using System.Collections;
using UnityEngine;

namespace ArtBasel
{
    public class PokeToDiscoverWord : MonoBehaviour
    {
        private GameObject playParticle;

        private void Start()
        {
            playParticle = this.transform.GetChild(0).gameObject;
            playParticle.transform.SetParent(transform.root);
        }

        public void DiscoverWord()
        {
            playParticle.SetActive(true);
            StartCoroutine(RevealWordDelay());
        }

        private IEnumerator RevealWordDelay()
        {
            yield return new WaitForSeconds(0.15f);
            this.gameObject.SetActive(false);
        }
    }
}
