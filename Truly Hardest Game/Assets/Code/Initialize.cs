using UnityEngine;
using UnityEngine.AddressableAssets;

public class Initialize : MonoBehaviour
{

    [SerializeField] string addressOfScene;

    private void Start() {
        
        Addressables.LoadSceneAsync(addressOfScene);

    }

}
