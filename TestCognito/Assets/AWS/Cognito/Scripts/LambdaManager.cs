using Amazon.Lambda;
using Amazon.Lambda.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LambdaManager : MonoBehaviour {

  [SerializeField]
  private AuthenticationManager _authenticationManager;

  private string _lambdaFunctionName = "testFunction";

  public async void ExecuteLambda() {
    Debug.Log("ExecuteLambda");

    AmazonLambdaClient amazonLambdaClient =
      new AmazonLambdaClient(_authenticationManager.GetCredentials(), AuthenticationManager.Region);

    InvokeRequest invokeRequest = new InvokeRequest {
      FunctionName = _lambdaFunctionName,
      InvocationType = InvocationType.RequestResponse
    };

    InvokeResponse response = await amazonLambdaClient.InvokeAsync(invokeRequest);
    Debug.Log("Response statusCode: " + response.StatusCode);

    if (response.StatusCode == 200) {
      Debug.Log("Successful Lambda Call");

      string userId = _authenticationManager.GetUsersId();
      Debug.Log("UserId In Lambda: " + userId);
    }
  }

  private void OnMainMenuClick() {
    SceneManager.LoadScene("LoginScene");
    Debug.Log("Changed to Login Scene");
  }
}
