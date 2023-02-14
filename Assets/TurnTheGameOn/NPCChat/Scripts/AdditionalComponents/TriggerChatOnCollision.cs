namespace TurnTheGameOn.NPCChat
{
    using UnityEngine;

    [RequireComponent(typeof(NPCChat))]
    public class TriggerChatOnCollision : MonoBehaviour
    {
        public string triggerTag = "Player";
        private NPCChat npcChat;

        void Start()
        {
            npcChat = GetComponent<NPCChat>();
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.tag == triggerTag)
            {
                npcChat.StartChat();
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.tag == triggerTag)
            {
                npcChat.StopChat();
            }
        }
    }
}