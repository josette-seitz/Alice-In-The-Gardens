using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArtBasel
{
    public class WaypointMove : MonoBehaviour
    {
        [SerializeField]
        private bool DisableScriptOnEnd = true;

        private const string WaypointTag = "Waypoint";
        private List<Transform> waypoints;
        private Transform glowTrail;
        private int index = 0;
        private float speed = 5f;

        void Start()
        {
            waypoints = new List<Transform>();
            waypoints = GetComponentsInChildren<Transform>().Where(x => x.tag == WaypointTag).ToList();
            foreach (var waypoint in waypoints)
            {
                waypoint.GetComponentInChildren<Renderer>().enabled = false;
            }

            glowTrail = GetComponentInChildren<ParticleSystem>().transform;
            glowTrail.localPosition = waypoints[index].localPosition;
            index++;
        }

        void Update()
        {
            if (index < waypoints.Count)
            {
                if (glowTrail.localPosition != waypoints[index].localPosition && index < waypoints.Count)
                {
                    glowTrail.localPosition = Vector3.MoveTowards(glowTrail.localPosition, waypoints[index].localPosition, Time.deltaTime * speed);
                }
                else
                {
                    index++;
                    if (index >= waypoints.Count)
                    {
                        if (DisableScriptOnEnd)
                            this.enabled = false;
                        else
                        {
                            glowTrail.GetComponent<ParticleSystem>().Stop();
                            // dealy play
                            StartCoroutine(DelayReplayingParticleSystem());
                        }
                    }
                }
            }
        }

        private IEnumerator DelayReplayingParticleSystem()
        {
            yield return new WaitForSeconds(2f);
            index = 0;
            glowTrail.GetComponent<ParticleSystem>().Play();
        }
    }
}
