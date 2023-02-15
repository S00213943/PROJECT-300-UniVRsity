namespace TurnTheGameOn.NPCChat
{
    using UnityEngine;

    [RequireComponent(typeof(NPCChat))]
    public class TriggerChatOnKeyDown : MonoBehaviour
    {
        public KeyCode keyCode;
        private NPCChat npcChat;
        public int mouseButton;
        public bool mouseOverToStart;
        private bool mouseOver;
        void OnMouseEnter() { mouseOver = true; }

        void OnMouseExit() { mouseOver = false; }
        void Start()
        {
            npcChat = GetComponent<NPCChat>();
        }

        void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                if (npcChat.talking)
                {
                    npcChat.FinishPage();
                }
                else
                {
                    if (mouseOverToStart)
                    {
                        if (mouseOver)
                        {
                            npcChat.StartChat();
                        }
                    }
                    else
                    {
                        npcChat.StartChat();
                    }
                }
                }
        }
    }
}