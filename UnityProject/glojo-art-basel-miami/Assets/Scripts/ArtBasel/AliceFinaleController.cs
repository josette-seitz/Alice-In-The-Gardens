using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ArtBasel
{
    public class AliceFinaleController : MonoBehaviour
    {
        [SerializeField]
        private Camera MainCam;
        [SerializeField]
        private GameObject Alice3Piece;
        [SerializeField]
        private GameObject RabbitParent;
        [SerializeField]
        private GameObject WalkTowardsRabbit;
        [SerializeField]
        private ParticleSystem BlueSmoke;
        [SerializeField]
        private GameObject RabbitWorld;
        [SerializeField]
        private GameObject OpenPortal;
        [SerializeField]
        private GameObject PetRabbit;
        [SerializeField]
        private GameObject RabbitGreenLight;
        [SerializeField]
        private List<GameObject> Clocks;
        [SerializeField]
        private TextMeshPro EndText1;
        [SerializeField]
        private TextMeshPro EndText2;
        [SerializeField]
        private TextMeshPro EndText3;
        [SerializeField]
        private Transform EndPanelTarget;

        private GameObject rabbit;
        private float range = 100f;
        private int layerMask = 1 << 6; // Layer 6 = Rabbit
        private const string MainCameraTag = "MainCamera";
        private const string RabbitTag = "Rabbit";
        private const string RabbitRuns = "RabbitRuns";
        private const string RabbitIdles = "RabbitIdles";
        private Animator rabbitAnim;
        private BoxCollider finaleCollider;
        private bool peekRabbit = false;
        private bool rabbitRunning = false;
        private int index = 0;
        private float fadeSpeed = 2.5f;
        private AudioSource endAudio;
        private Vector3 rabbitRunDirection = Vector3.zero;
        private bool showEndText = false;
        private Transform endPanel;

        private enum FadeDirection
        {
            In, // Alpha = 1
            Out // Alpha = 0
        }

        private void Start()
        {
            RabbitParent.transform.SetParent(null);
            rabbit = RabbitParent.transform.GetChild(0).gameObject;

            rabbitAnim = rabbit.GetComponent<Animator>();
            finaleCollider = this.GetComponent<BoxCollider>();

            endAudio = this.GetComponent<AudioSource>();
            endPanel = EndText1.transform.parent;
        }

        void Update()
        {
            if (rabbitRunning)
                rabbit.transform.position += rabbitRunDirection * (Time.deltaTime * 0.15f);

            if (showEndText)
            {
                float speed = 0.08f * Time.deltaTime; // calculate distance to move
                endPanel.position = Vector3.MoveTowards(endPanel.position, EndPanelTarget.position, speed);
            }

            // Show Thank You for Visiting Alice in The Gardens (Finale)
            if (Vector3.Distance(endPanel.position, EndPanelTarget.position) == 0f && showEndText)
            {
                StartCoroutine(FadeEndText(FadeDirection.Out, EndText1));
                showEndText = false;
            }
        }

        void FixedUpdate()
        {
            RaycastHit hitInfo;

            if (Physics.Raycast(MainCam.transform.position, MainCam.transform.forward, out hitInfo, range, layerMask)
                && !peekRabbit)
            {
                Alice3Piece.SetActive(false);
                StartCoroutine(WalkTowardsRabbitDelay());
                peekRabbit = true;
            }
        }

        private IEnumerator WalkTowardsRabbitDelay()
        {
            yield return new WaitForSeconds(0.75f);
            WalkTowardsRabbit.SetActive(true);
            finaleCollider.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            // User is Walking Towards The Rabbit
            // Rabbit World is Enabled
            if (other.transform.parent.tag == MainCameraTag)
            {
                finaleCollider.center = new Vector3(finaleCollider.center.x, finaleCollider.center.y, 0.6f);
                WalkTowardsRabbit.SetActive(false);
                OpenPortal.SetActive(false);
                BlueSmoke.Play();
                RabbitWorld.SetActive(true);

                // Rotate to Vector3.zero (0,0,0)
                rabbit.transform.Rotate(0, -180, 0);
                rabbitAnim.SetTrigger(RabbitRuns);
                RabbitGreenLight.SetActive(true);
                rabbitRunDirection = Vector3.forward;
                rabbitRunning = true;
            }

            if (other.transform.parent.tag == RabbitTag)
            {
                finaleCollider.enabled = false;
                rabbitRunning = false;
                rabbit.transform.Rotate(0, 180, 0);
                rabbitAnim.SetTrigger(RabbitIdles);
                PetRabbit.SetActive(true);
                other.enabled = false;
                // Turn on the MRTK Touch Collider to Pet the Rabbit
                RabbitParent.GetComponentInChildren<StatefulInteractable>()
                    .gameObject.GetComponent<BoxCollider>()
                    .enabled = true;
            }
        }

        public void ShowBeautifulWomenPieces()
        {
            StartCoroutine(ShowClocksFromRabbit());
        }

        // Could swap out and use Timeline
        private IEnumerator ShowClocksFromRabbit()
        {
            yield return new WaitForSeconds(2f);
            Clocks[index].SetActive(true);
            yield return new WaitForSeconds(1f);
            index++;
            Clocks[index].SetActive(true);
            yield return new WaitForSeconds(3.5f);
            index++;
            Clocks[index].SetActive(true);
            endAudio.PlayDelayed(4f);
            yield return new WaitForSeconds(8.5f);
            rabbitAnim.SetTrigger(RabbitRuns);
            rabbitRunDirection = Vector3.back;
            rabbitRunning = true;
            endPanel.gameObject.SetActive(true);
            showEndText = true;
        }

        private IEnumerator FadeEndText(FadeDirection fadeDirection, TextMeshPro textFade)
        {
            float alpha = (fadeDirection == FadeDirection.Out) ? 1 : 0;
            float fadeEndValue = (fadeDirection == FadeDirection.Out) ? 0 : 1;

            if (fadeDirection == FadeDirection.Out)
            {
                yield return new WaitForSeconds(7f);

                while (alpha >= fadeEndValue)
                {
                    textFade.color = new Color(textFade.color.r, textFade.color.g, textFade.color.b, alpha);
                    alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);

                    yield return null;
                }

                // Once fade out is finished
                textFade.gameObject.SetActive(false);
                // Fade in final text
                StartCoroutine(FadeEndText(FadeDirection.In, EndText2));
            }
            else
            {
                while (alpha <= fadeEndValue)
                {
                    textFade.color = new Color(textFade.color.r, textFade.color.g, textFade.color.b, alpha);
                    alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
                    yield return null;
                }

                if (textFade == EndText2)
                {
                    yield return new WaitForSeconds(7f);
                    textFade.gameObject.SetActive(false);
                    StartCoroutine(FadeEndText(FadeDirection.In, EndText3));
                    RabbitParent.SetActive(false);
                }
            }
        }
    }
}
