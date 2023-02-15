namespace TurnTheGameOn.NPCChat
{
	using System.Collections.Generic;
	using UnityEngine;

	public class MouseOverOutline : MonoBehaviour
	{
		public Material outlineMaterial;
		public List<Renderer> outlineRendererList;
		private List<Material[]> defaultMaterials = new List<Material[]>();
		private List<Material[]> outlineMaterials = new List<Material[]>();

		private void Awake()
		{
			/// Cache Default Materials
			for (int i = 0; i < outlineRendererList.Count; i++)
			{
				Material[] matArray = new Material[outlineRendererList[i].sharedMaterials.Length];
				for (int j = 0; j < matArray.Length; j++)
				{
					matArray[j] = outlineRendererList[i].sharedMaterials[j];
				}
				defaultMaterials.Add(matArray);
			}
			/// Cache Outline Materials
			for (int i = 0; i < outlineRendererList.Count; i++)
			{
				Material[] matArray = new Material[outlineRendererList[i].sharedMaterials.Length + 1];
				for (int j = 0; j < matArray.Length - 1; j++)
				{
					matArray[j] = outlineRendererList[i].sharedMaterials[j];
				}
				matArray[matArray.Length - 1] = outlineMaterial;
				outlineMaterials.Add(matArray);
			}
		}

		private void OnMouseExit()
		{
			for (int i = 0; i < outlineRendererList.Count; i++) /// Assign Cached Default Materials
			{
				outlineRendererList[i].materials = defaultMaterials[i];
			}
		}

		private void OnMouseEnter()
		{
			for (int i = 0; i < outlineRendererList.Count; i++) /// Assign Cached Outline Materials
			{
				outlineRendererList[i].materials = outlineMaterials[i];
			}
		}
	}
}