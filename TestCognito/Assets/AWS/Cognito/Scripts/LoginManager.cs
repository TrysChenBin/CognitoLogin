using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

  // save data path
  public static string CachePath;
  
  // unauth panel
  [SerializeField]
  private GameObject _unauthPanel;
  
  // login panel
  [SerializeField]
  private GameObject _loginPanel;
  
  [SerializeField]
  private TMP_InputField _emailFieldLogin;
  
  [SerializeField]
  private TMP_InputField _passwordFieldLogin;
    
  [SerializeField]
  private Button _loginButton;

  // signup panel
  [SerializeField]
  private GameObject _signupPanel;
  
  [SerializeField]
  private TMP_InputField _emailFieldSignup;

  [SerializeField]
  private TMP_InputField _usernameFieldSignup;
  
  [SerializeField]
  private TMP_InputField _passwordFieldSignup;

  [SerializeField]
  private Button _signupButton;
  
  // confirm panel
  [SerializeField]
  private GameObject _confirmPanel;
  
  [SerializeField]
  private TMP_InputField _codeField;
  
  [SerializeField]
  private Button _verificationButton;
  
  [SerializeField]
  private Button _cancelVerificationButton;
  
  // auth panel
  [SerializeField]
  private GameObject _authPanel;
  
  [SerializeField]
  private Button _startButton;

  [SerializeField]
  private Button _logoutButton;
  
  // loading panel
  [SerializeField]
  private GameObject _loadingPanel;
  
  // manager
  [SerializeField]
  private AuthenticationManager _authenticationManager;

  [SerializeField]
  private LambdaManager _lambdaManager;


  private void Awake() {
    CachePath = Application.persistentDataPath;
    
    _unauthPanel.SetActive(true);
    _loginPanel.SetActive(true);
    _signupPanel.SetActive(true);
    _confirmPanel.SetActive(false);
    _authPanel.SetActive(false);
    _loadingPanel.SetActive(false);
  }

  private void Start() {
    // check if user is already authenticated
    RefreshToken();
    
    _loginButton.onClick.AddListener(OnLoginClick);
    _signupButton.onClick.AddListener(OnSignupClick);
    _startButton.onClick.AddListener(OnStartClick);
    _logoutButton.onClick.AddListener(OnLogoutClick);
    _verificationButton.onClick.AddListener(OnVerificationClick);
    _cancelVerificationButton.onClick.AddListener(OnCancelVerificationClick);
  }

  private void DisplayComponentdFromAuthStatus(bool authStatus) {
    if (authStatus) {
      _unauthPanel.SetActive(false);
      _loadingPanel.SetActive(false);
      _authPanel.SetActive(true);
    }
    else {
      _unauthPanel.SetActive(true);
      _loadingPanel.SetActive(false);
      _authPanel.SetActive(false);
    }
    
    // clear out password
    _passwordFieldLogin.text = "";
    _passwordFieldSignup.text = "";
  }

  private async void OnLoginClick() {
    //_unauthPanel.SetActive(false);
    _loadingPanel.SetActive(true);
    
    bool isLoginSuccess = await _authenticationManager.Login(_emailFieldLogin.text, _passwordFieldLogin.text);
    DisplayComponentdFromAuthStatus(isLoginSuccess);
  }

  private async void OnSignupClick() {
    //_unauthPanel.SetActive(false);
    _loadingPanel.SetActive(true);
    
    bool isSignupSuccess =
      await _authenticationManager.Signup(_usernameFieldSignup.text, _emailFieldSignup.text, _passwordFieldSignup.text);

    if (isSignupSuccess) {
      _signupPanel.SetActive(false);
      _confirmPanel.SetActive(true);
      
      //copy signup input test to login input field
      _emailFieldLogin.text = _emailFieldSignup.text;
      _passwordFieldLogin.text = _passwordFieldSignup.text;
    }
    else {
      _signupPanel.SetActive(true);
      _confirmPanel.SetActive(false);
      _passwordFieldSignup.text = "";
    }
    
    //_unauthPanel.SetActive(true);  
    _loadingPanel.SetActive(false);
  }

  private async void OnVerificationClick() {
    //_unauthPanel.SetActive(false);
    _loadingPanel.SetActive(true);

    bool isVerificationSuccess = await _authenticationManager.Verification(_emailFieldSignup.text, _codeField.text);

    if (isVerificationSuccess) {
      _signupPanel.SetActive(true);
      _confirmPanel.SetActive(false);
      _usernameFieldSignup.text = "";
      _emailFieldSignup.text = "";
      _passwordFieldSignup.text = "";
    }
    else {
      _signupPanel.SetActive(false);
      _confirmPanel.SetActive(true);
    }
    
    //_unauthPanel.SetActive(true);
    _loadingPanel.SetActive(false);
  }

  private void OnCancelVerificationClick() {
    _signupPanel.SetActive(true);
    _confirmPanel.SetActive(false);
    _codeField.text = "";
  }

  private void OnLogoutClick() {
    _authenticationManager.SignOut();
    DisplayComponentdFromAuthStatus(false);
  }

  private void OnStartClick() {
    SceneManager.LoadScene("GameScene");
    Debug.Log("change to game scene");
    
    _lambdaManager.ExecuteLambda();
  }

  private async void RefreshToken() {
    bool isRefreshSuccess = await _authenticationManager.RefreshSession();
    DisplayComponentdFromAuthStatus(isRefreshSuccess);
  }
}
