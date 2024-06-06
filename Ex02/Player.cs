using System;
using System.Collections.Generic;
using UI;

namespace Logic
{
    public class Player
    {
        private string m_Name;
        private int m_NumOfPairsDiscovered;
        private char m_TypeOfPlayer;
        private List<(int row, int column, char cardValue)> m_MemoryOfComputer;
        private const int k_WrongIndexForComputer = -100;

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public int NumOfPairsDiscovered
        {
            get
            {
                return m_NumOfPairsDiscovered;
            }
            set
            {
                m_NumOfPairsDiscovered = value;
            }
        }

        public char TypeOfPlayer
        {
            get
            {
                return m_TypeOfPlayer;
            }
        }

        public void InitPlayer(string i_PlayerName, char i_TypeOfPlayer)
        {
            m_Name = i_PlayerName;
            m_NumOfPairsDiscovered = 0;
            m_TypeOfPlayer = i_TypeOfPlayer;
            m_MemoryOfComputer = new List<(int, int, char)>(2);
        }

        public void AddToMemory(Card i_Card)
        {
            m_MemoryOfComputer.Add((i_Card.RowIndex, i_Card.ColumnIndex, i_Card.Value));
        }

        public (int, int) GetKnownCard(char i_CardValue, Board i_Board)
        {
            (int row, int col) = (k_WrongIndexForComputer, k_WrongIndexForComputer);

            foreach (var item in m_MemoryOfComputer)
            {
                if (item.cardValue == i_CardValue && i_Board.GetCardByIndex(item.row, item.column).IsCovered)
                {
                    (row, col) =  (item.row, item.column);
                }
            }

            return (row, col);
        }

        public (int, int) GetRandomMove(Board i_Board)
        {
            Random rand = new Random();
            int row, col;

            do
            {
                row = rand.Next(i_Board.Height);
                col = rand.Next(i_Board.Wight);
            } while (!i_Board.GetCardByIndex(row, col).IsCovered);

            return (row, col);
        }

        public void ClearMemory()
        {
            m_MemoryOfComputer.Clear();
        }

    }


}
