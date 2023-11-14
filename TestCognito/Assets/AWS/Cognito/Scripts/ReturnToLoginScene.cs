using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToLoginScene : MonoBehaviour
{
  [SerializeField]
  private Button _loginButton;

  private void Start() {
    _loginButton.onClick.AddListener(ToLoginScene);
  }

  private void ToLoginScene() {
    SceneManager.LoadScene("LoginScene");
  }

}
