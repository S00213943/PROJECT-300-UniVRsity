namespace TurnTheGameOn.NPCChat
{
    using UnityEngine;

    [RequireComponent(typeof(NPCChat))]
    public class TriggerChatOnMouseDown : MonoBehaviour
    {
        public int mouseButton;
        public bool mouseOverToStart;
        private bool mouseOver;
        private NPCChat npcChat;

        void OnMouseEnter() { mouseOver = true; }

        void OnMouseExit() { mouseOver = false; }

        void Start() { npcChat = GetComponent<NPCChat>(); }

        void Update()
        {
            if (Input.GetMouseButtonDown(mouseButton))
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