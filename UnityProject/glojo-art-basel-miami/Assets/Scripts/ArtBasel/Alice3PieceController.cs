using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArtBasel
{
    public class Alice3PieceController : MonoBehaviour
    {
        [SerializeField]
        private GameObject MainCameraCollider;
        [SerializeField]
        private GameObject TurnAround;
        [SerializeField]
        private GameObject RevealMessage;
        [SerializeField]
        private GameObject Alice3PieceArt;
        [SerializeField]
        private GameObject Portal;
        [SerializeField]
        private GameObject LookBehindYou;
        [SerializeField]
        private GameObject YouGotThis;
        [SerializeField]
        private GameObject AliceFinale;

        [Header("How Strange Piece")]
        [SerializeField]
        private GameObject HowStrangeWord1;
        [SerializeField]
        private GameObject HowStrangeWord2;
        [SerializeField]
        private GameObject HowStrangeWord3;
        [SerializeField]
        private GameObject HowStrangePanel;
        [SerializeField]
        private GameObject LookRightPanel;
        private bool StrangeHidden = true;

        [Header("How Long Piece")]
        [SerializeField]
        private GameObject HowLongWord1;
        [SerializeField]
        private GameObject HowLongWord2;
        [SerializeField]
        private GameObject HowLongWord3;
        [SerializeField]
        private GameObject HowLongPanel;
        [SerializeField]
        private GameObject LookLeftPanel;
        private bool LongHidden = true;

        [Header("I Knew Piece")]
        [SerializeField]
        private GameObject IKnewWord1;
        [SerializeField]
        private GameObject IKnewWord2;
        [SerializeField]
        private GameObject IKnewWord3;
        [SerializeField]
        private GameObject IKnewPanel;
        private bool KnewHidden = true;


        private const string MainCameraTag = "MainCamera";
        private bool revealMessage = true;
        private bool hiddenMessageComplete = false;
        private bool hiddenMessageComplete1 = false;
        private bool hiddenMessageComplete2 = false;
        private bool hiddenMessageComplete3 = false;
        private List<GameObject> interactableHiddenWords;

        private void Start()
        {
            // MainCameraCollider
            // get parent object and get it's tag
            MainCameraCollider.SetActive(true);

            interactableHiddenWords = new List<GameObject>()
            {
                HowStrangeWord1,
                HowStrangeWord2,
                HowStrangeWord3,
                HowLongWord1,
                HowLongWord2,
                HowLongWord3,
                IKnewWord1,
                IKnewWord2,
                IKnewWord3
            };

            foreach (var hiddenWord in interactableHiddenWords)
            {
                hiddenWord.GetComponent<StatefulInteractable>().enabled = false;
            }
        }

        private void Update()
        {
            if (!HowStrangeWord1.activeSelf || !HowStrangeWord2.activeSelf || !HowStrangeWord3.activeSelf
                || !HowLongWord1.activeSelf || !HowLongWord2.activeSelf || !HowLongWord3.activeSelf
                || !IKnewWord1.activeSelf || !IKnewWord2.activeSelf || !IKnewWord3.activeSelf && revealMessage)
            {
                RevealMessage.SetActive(false);
                revealMessage = false;
            }

            if (!HowStrangeWord1.activeSelf && !HowStrangeWord2.activeSelf
                && !HowStrangeWord3.activeSelf && StrangeHidden)
            {
                HowStrangePanel.SetActive(true);
                StartCoroutine(LookToTheSideDelay(LookRightPanel));
                hiddenMessageComplete1 = true;
                StrangeHidden = false;
            }

            if (!HowLongWord1.activeSelf && !HowLongWord2.activeSelf
                && !HowLongWord3.activeSelf && LongHidden)
            {
                HowLongPanel.SetActive(true);
                StartCoroutine(LookToTheSideDelay(LookLeftPanel));
                hiddenMessageComplete2 = true;
                LongHidden = false;
            }

            if (!IKnewWord1.activeSelf && !IKnewWord2.activeSelf
                && !IKnewWord3.activeSelf && KnewHidden)
            {
                IKnewPanel.SetActive(true);
                hiddenMessageComplete3 = true;
                KnewHidden = false;
            }

            if (hiddenMessageComplete1 && hiddenMessageComplete2 && hiddenMessageComplete3
                && !hiddenMessageComplete)
            {
                StartCoroutine(LookBehindYouDelay());
                hiddenMessageComplete = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent.tag == MainCameraTag)
            {
                TurnAround.SetActive(false);
                Alice3PieceArt.SetActive(true);
                Portal.SetActive(false);
                this.GetComponent<Collider>().enabled = false;
            }
        }

        private IEnumerator LookBehindYouDelay()
        {
            yield return new WaitForSeconds(4f);
            LookBehindYou.SetActive(true);
            YouGotThis.SetActive(false);
            AliceFinale.SetActive(true);
        }

        private IEnumerator LookToTheSideDelay(GameObject lookToTheSide)
        {
            yield return new WaitForSeconds(5f);
            lookToTheSide.SetActive(true);
        }
    }
}
