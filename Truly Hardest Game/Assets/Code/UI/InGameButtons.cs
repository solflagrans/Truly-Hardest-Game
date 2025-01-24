using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class InGameButtons : MonoBehaviour
{

    //Писать сохранения в JSON? Не.
    //Написать костыль для того, чтобы нагружать систему
    //трёмя сценами одновременно? Да!
    //Но в рамках задания, думаю, это допустимо,
    //т.к. сцены очень лёгкие.

    //Если сцен больше одной - вручную удаляем текущую сцену и создаём заново аддитивно.
    //Если сцена всего одна - просто её перезагружаем.
    public async void Restart(string addressOfCurrentScene) {

        if(SceneManager.sceneCount > 1) {
            await SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            await Addressables.LoadSceneAsync(addressOfCurrentScene, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(addressOfCurrentScene));
        } else {
            await Addressables.LoadSceneAsync(addressOfCurrentScene);
        }

    }

    //Если сцена уже загружена, но не активна - переключаемся между их корневыми элементами
    //Если сцена ещё не загружена, то загружаем её.
    public async void SwitchScene(string addressOfDesiredScene) {

        if(SceneManager.GetSceneByName(addressOfDesiredScene).IsValid()) {
            GameObject desiredSceneRoot = SceneManager.GetSceneByName(addressOfDesiredScene).GetRootGameObjects()[0];
            GameObject currentSceneRoot = transform.root.gameObject;

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(addressOfDesiredScene));

            currentSceneRoot.SetActive(false);
            desiredSceneRoot.SetActive(true);
        } else {
            transform.root.gameObject.SetActive(false);
            await Addressables.LoadSceneAsync(addressOfDesiredScene, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(addressOfDesiredScene));
        }

    }

    public void Exit() {

        Application.Quit();

    }

}
