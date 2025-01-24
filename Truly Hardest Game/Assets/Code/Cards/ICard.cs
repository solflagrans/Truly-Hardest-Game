using UnityEngine.UI;

public interface ICard {

    //����� - ��������������� ����������� �������.
    //��� ���� ���������� ���� ��� � ��������, ��� ����
    //����� � ����������� ���������. �� ��� ����� ���������
    //� ������� ������ �������.

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