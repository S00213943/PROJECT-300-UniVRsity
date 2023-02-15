namespace TurnTheGameOn.NPCChat
{
    using UnityEngine;
    using UnityEngine.Timeline;
    using UnityEngine.Playables;

    [TrackBindingType(typeof(NPCChat))]
    [TrackClipType(typeof(NPCChatClip))]
    public class NPCChatTrack : TrackAsset
    {
        /// This will be passed to mixer if needed and figured out.
        /// If we're only triggering a single event with npc chat the mixer is not required.

        //public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        //{
        //    return ScriptPlayable<NPCChatTrackMixer>.Create(graph, inputCount);
        //}
    }
}