using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

namespace ArtBasel
{
    public class YouGotThisController : MonoBehaviour
    {
        [SerializeField]
        private GameObject PerformanceArt;
        [SerializeField]
        private GameObject Gem;
        [SerializeField]
        private Transform YouGotThis;
        [SerializeField]
        private GameObject GemExplosion;
        [SerializeField]
        private GameObject PerformanceDance;
        [SerializeField]
        private AudioSource TheDifference;
        [SerializeField]
        private GameObject AliceThreePiecePortal;
        [SerializeField]
        private GameObject YouGotThisEnding;
        [SerializeField]
        private GameObject TurnAround;

        private bool dancePieceTurnOn = false;
        private bool moveGem = false;
        private bool explodeGem = false;
        private bool showPricePanel = false;
        private float rotateSpeed = 350f;

        // When user touches "Gem"
        public void ActivateDancePiece()
        {
            if (!dancePieceTurnOn)
            {
                moveGem = true;
                PerformanceArt.SetActive(true);
                dancePieceTurnOn = true;
            }
        }

        private void Update()
        {
            if (moveGem)
            {
                //Vector3 moveGem = Vector3.forward * 3f;
                //Gem.transform.Translate(moveGem * Time.deltaTime);

                // Move our position a step closer to the target.
                //var step = speed * Time.deltaTime; // calculate distance to move
                var speed = 0.175f * Time.deltaTime;
                // Move Current to Target position
                Gem.transform.position = Vector3.MoveTowards(Gem.transform.position, YouGotThis.position, speed);

                // Rotate gem on Y-axis
                Vector3 rotateGem = Vector3.up * rotateSpeed;
                Gem.transform.transform.Rotate(rotateGem * Time.deltaTime);
            }

            if (Gem.transform.position == YouGotThis.position && !explodeGem)
            {
                moveGem = false;
                GemExplosion.SetActive(true);
                Gem.SetActive(false);
                PerformanceDance.SetActive(true);
                PerformanceArt.GetComponentInChildren<PlayableDirector>().gameObject.SetActive(false);
                showPricePanel = true;
                explodeGem = true;
            }

            if(!TheDifference.isPlaying && showPricePanel)
            {
                PerformanceArt.SetActive(false);

                //Show price panel
                // Fade Text to "Now Turn Around"
                AliceThreePiecePortal.SetActive(true);
                YouGotThisEnding.SetActive(true);
                StartCoroutine(TurnAroundDelay());
                showPricePanel = false;
            }
        }

        private IEnumerator TurnAroundDelay()
        {
            yield return new WaitForSeconds(3.5f);
            TurnAround.SetActive(true);
        }
    }
}
