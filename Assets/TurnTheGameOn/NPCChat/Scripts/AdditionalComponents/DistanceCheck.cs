namespace TurnTheGameOn.NPCChat
{
    using UnityEngine;

    [RequireComponent(typeof(NPCChat))]
    public class DistanceCheck : MonoBehaviour
    {
        public float distanceToChat = 7;
        public Transform playerTransform;
        public bool findPlayerOnStart;
        public string findPlayerName;
        private float distanceToPlayer;
        private NPCChat npcChat;
        public GameObject[] activeObjectsInRange;
        public Behaviour[] activeBehavioursInRange;
        public Collider[] activeCollidersInRange;
        private int currentState = -1;

        void Start()
        {
            npcChat = GetComponent<NPCChat>();
            if (findPlayerOnStart)
            {
                playerTransform = GameObject.Find(findPlayerName).transform;
            }
            if (playerTransform == null)
            {
                Debug.LogWarning("[NPC Chat] [DistanceCheck] Player Transform is not assigned, disabling Distance Check component.");
                enabled = false;
            }
        }

        void Update()
        {
            distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer >= distanceToChat)
            {
                npcChat.canChat = false;
                if (currentState != 0)
                {
                    currentState = 0;
                    for (int i = 0; i < activeObjectsInRange.Length; i++)
                    {
                        activeObjectsInRange[i].SetActive(false);
                    }
                    for (int i = 0; i < activeBehavioursInRange.Length; i++)
                    {
                        activeBehavioursInRange[i].enabled = false;
                    }
                    for (int i = 0; i < activeCollidersInRange.Length; i++)
                    {
                        activeCollidersInRange[i].enabled = false;
                    }
                }
            }
            else
            {
                npcChat.canChat = true;
                if (currentState != 1)
                {
                    currentState = 1;
                    for (int i = 0; i < activeObjectsInRange.Length; i++)
                    {
                        activeObjectsInRange[i].SetActive(true);
                    }
                    for (int i = 0; i < activeBehavioursInRange.Length; i++)
                    {
                        activeBehavioursInRange[i].enabled = true;
                    }
                    for (int i = 0; i < activeCollidersInRange.Length; i++)
                    {
                        activeCollidersInRange[i].enabled = true;
                    }
                }
            }
        }

    }
}