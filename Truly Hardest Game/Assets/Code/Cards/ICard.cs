using UnityEngine.UI;

public interface ICard {

    //Карты - самостоятельные управляемые объекты.
    //Они сами определяют свой тип и понимают, как себя
    //вести в определённых ситуациях. Но ими можно управлять
    //с помощью набора функций.

    Image Image { get; set; }
    CardSettings Settings { get; set; }
    bool InWaste { get; set; }
    bool IsClosed { get; set; }

    void Initialize();
    void MoveToWaste();
    void UpdateState(bool closed);
    void Undo();
    sbyte GetRating();

}