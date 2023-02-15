namespace TurnTheGameOn.NPCChat
{
	using UnityEngine;
	using UnityEngine.Events;
#if NPCCHAT_LIPSYNCPRO
	using RogoDigital.Lipsync;
#endif
	[System.Serializable]
	public class DialogueSettings
	{
		public float duration = 5f;
		public string name;
		[TextArea(3, 10)] public string text;
		public ChatBox chatBox;
#if NPCCHAT_LIPSYNCPRO
		public LipSyncData lipSyncData;
		public LipSync lipSync;
#endif
		public AudioClip audio;
		public bool loopAudio;
		public DialogueButtons buttons;
		public UnityEvent pageStartEvent;
		public UnityEvent pageStopEvent;
	}
}