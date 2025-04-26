using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public float gameTime;
        public bool isGameActive;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!isGameActive) return;

            gameTime += Time.deltaTime;
            // TODO: UIManager.Instance.UpdateTimer(gameTime);
        }

        public void StartGame()
        {
            gameTime = 0f;
            isGameActive = true;
        }

        public void EndGame()
        {
            isGameActive = false;
            // TODO: UIManager.Instance.ShowGameOver();
            // SceneManager.LoadScene("GameOverScene");
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
        }
    }
}