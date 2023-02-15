namespace TurnTheGameOn.NPCChat
{
    using UnityEngine;
    using UnityEngine.Playables;

    public class NPCChatClip : PlayableAsset
    {
        public NPCChatAction action;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<NPCChatBehaviour>.Create(graph);

            NPCChatBehaviour npcChatBehaviour = playable.GetBehaviour();
            npcChatBehaviour.action = action;

            return playable;
        }
    }
}