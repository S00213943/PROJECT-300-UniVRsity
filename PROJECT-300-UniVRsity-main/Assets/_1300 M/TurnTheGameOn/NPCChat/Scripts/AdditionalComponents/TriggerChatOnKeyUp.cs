namespace TurnTheGameOn.NPCChat
{
    using UnityEngine;

    [RequireComponent(typeof(NPCChat))]
    public class TriggerChatOnKeyUp : MonoBehaviour
    {
        public KeyCode keyCode;
        private NPCChat npcChat;

        void Start()
        {
            npcChat = GetComponent<NPCChat>();
        }

        void Update()
        {
            if (Input.GetKeyUp(keyCode))
            {
                if (npcChat.talking)
                {
                    npcChat.FinishPage();
                }
                else
                {
                    npcChat.StartChat();
                }
            }
        }
    }
}