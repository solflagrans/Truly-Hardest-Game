using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    //������ ����. ��������� �������� ���� � ������
    //� ����� ���������� ������ �� ����������� ������,
    //�.�. ���������� �� �������.

    [Tooltip("From left to right")]
    [field: SerializeField] public Peak[] Peaks { get; set; }
    
    List<PeakCard> CardsInDeck = new List<PeakCard>();

    [System.Serializable]
    public struct Peak {
        [Tooltip("From down to up")]
        public Row[] rows;
    }

    [System.Serializable]
    public struct Row {
        [Tooltip("From left to right")]
        public PeakCard[] cards;
    }

    private void Awake() {

        for(byte peakIndex = 0; peakIndex < Peaks.Length; peakIndex++) {
            Peak peak = Peaks[peakIndex];
            for(byte rowIndex = 0; rowIndex < peak.rows.Length; rowIndex++) {
                Row row = peak.rows[rowIndex];
                for(byte cardOrder = 0; cardOrder < row.cards.Length; cardOrder++) {
                    PeakCard card = row.cards[cardOrder];
                    card.InitializeDeckPosition(peakIndex, cardOrder, rowIndex);
                    if(!CardsInDeck.Contains(card)) CardsInDeck.Add(card);
                }
            }
        }

    }

    public List<PeakCard> GetInitialCardsInDeck() {

        return CardsInDeck;

    }

    public PeakCard FindCardByPosition(byte peak, byte order, byte row) {

        if(peak >= Peaks.Length) return null;
        if(order >= Peaks[peak].rows.Length) return null;
        if(order >= Peaks[peak].rows[row].cards.Length) return null;

        return Peaks[peak].rows[row].cards[order];

    }

}
