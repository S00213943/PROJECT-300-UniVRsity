namespace TurnTheGameOn.NPCChat
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LoadScene : MonoBehaviour
    {
        public string scene;

        public void Load()
        {
            SceneManager.LoadScene(scene);
        }
    }
}