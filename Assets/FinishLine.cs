using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(Player);
            winScreen.SetActive(true);
        }
    }
    public void restartGame()
    {
        winScreen.SetActive(false);
        SceneManager.LoadScene("Title Screen");
    }
    public void QuitGame()
    {
        Application.Quit();

    }
}
