using System;
using System.Collections.Generic;
using System.Text;
using UI;


namespace Logic
{
    public class Board
    {
        private int m_Hight;
        private int m_Width;
        private Card[,] m_Board;
        private int m_TotalCardDiscoverd;

        public int Height
        {
            get
            {
                return m_Hight;
            }
            set
            {
                m_Hight = value;
            }
        }

        public int Wight
        {
            get
            {
                return m_Width;
            }
            set
            {
                m_Width = value;
            }
        }

        public int TotalCardDiscoverd
        {
            get 
            {
                return m_TotalCardDiscoverd;
            }
            set
            {
                m_TotalCardDiscoverd = value;
            }
        }

        public Card[,] TheBoard
        {
            get
            {
                return m_Board;
            }
            set
            {
                m_Board = value;
            }
        }

        public bool CheckIfSizeOk(int i_Hight, int i_Width)
        {
            bool result = true;

            if ((i_Hight * i_Width) % 2 == 1)
            {
                result = false;
            }

            return result;
        }
    
        public bool IsIndexOk(char i_Column, int i_Row, ref StringBuilder io_ErrorMessage)
        {
            int intColumn = i_Column - 'A';
            bool result = true;

            if (i_Row < 1 || i_Row > m_Hight || intColumn < 0 || intColumn >= m_Width )
            {
                io_ErrorMessage.Append("Not in the board limits!");
                result = false;
            }
            else if (!m_Board[i_Row - 1, intColumn].IsCovered)
            {
                io_ErrorMessage.Append("This card in already open!");
                result = false;
            }
            return result;
        }

        public Card GetCardByIndex(int i_Row, int i_Column)
        {
            return m_Board[i_Row, i_Column];
        }

        public void TurningCard(Card i_Card)
        {
            m_Board[i_Card.RowIndex, i_Card.ColumnIndex].IsCovered = !m_Board[i_Card.RowIndex, i_Card.ColumnIndex].IsCovered;
        }

        public void TurningCardByIndex(int i_Row, int i_Column)
        {
            m_Board[i_Row, i_Column].IsCovered = !m_Board[i_Row, i_Column].IsCovered;
        }
    }
}
