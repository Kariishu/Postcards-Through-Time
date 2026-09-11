using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] AudioSource ButtonClick;
    [SerializeField] GameObject FadeOut;
 
    void Start()
    {
        
    }

    public void StartGame()
    {
        ButtonClick.Play();
        FadeOut.SetActive(true);
        StartCoroutine(TransferToFirstScene());
    }

    void Update()
    {
        
    }

    IEnumerator TransferToFirstScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);

    }
}
