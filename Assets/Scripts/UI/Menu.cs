using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public static Menu instance { get; protected set; }
	public GameObject defeatPanel;

	void Start()
	{
		instance = this;
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void Defeat()
	{
		defeatPanel.SetActive(true);
	}
}
