namespace TurnTheGameOn.NPCChat
{
    using UnityEngine.Playables;
    using UnityEngine;

    public class NPCChatBehaviour : PlayableBehaviour
    {
        public NPCChatAction action;
        bool m_FirstFrameHappened;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (Application.isPlaying)
            {
                NPCChat npcChat = playerData as NPCChat;
                if (!npcChat || m_FirstFrameHappened) { return; }

                if (action == NPCChatAction.StartConversation)
                {
                    npcChat.StartChat();
                }
                else if (action == NPCChatAction.NextPage)
                {
                    npcChat.FinishPage();
                }
                else if (action == NPCChatAction.StopConversation)
                {
                    npcChat.StopChat();
                }
                m_FirstFrameHappened = true;

                int inputCount = playable.GetInputCount();
                for (int i = 0; i < inputCount; i++)
                {
                    float inputWeight = playable.GetInputWeight(i);
                    if (inputWeight > 0f)
                    {
                        ScriptPlayable<NPCChatBehaviour> inputPlayable = (ScriptPlayable<NPCChatBehaviour>)playable.GetInput(i);
                        NPCChatBehaviour input = inputPlayable.GetBehaviour();

                        if (input.action == NPCChatAction.StartConversation)
                        {
                            npcChat.StartChat();
                        }
                        else if (input.action == NPCChatAction.NextPage)
                        {
                            npcChat.FinishPage();
                        }
                        else if (input.action == NPCChatAction.StopConversation)
                        {
                            npcChat.StopChat();
                        }
                        m_FirstFrameHappened = true;

                    }
                }
            }
        }
    }
}