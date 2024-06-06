using Ex02.ConsoleUtils;
using System.Threading;
using System.Collections.Generic;
using UI;



namespace Logic
{
    class Game
    {
        private List<Player> m_Players;
        private Board m_GameBoard;
        private const char k_TypeOfHumanPlayer = 'P';
        private const char k_TypeOfCompuerPlayer = 'C';
        private const int k_WrongIndexForComputer = -100;
        private const char k_FirstTimeValue = '-';

        public Board GameBoard
        {
            get
            {
                return m_GameBoard;
            }
            set 
            {
                m_GameBoard = value;
            }
        }

        public List<Player> PlayerList
        {
            get
            {
                return m_Players;
            }
        }

        public void GameInit(int i_NumOfPlayers)
        {
            m_Players = new List<Player>(i_NumOfPlayers);
            m_GameBoard = new Board();
        }

        public void AddPlayer(string i_PlayerName, char i_PlayerType)
        {
            Player newPlayer = new Player();

            newPlayer.InitPlayer(i_PlayerName, i_PlayerType);
            m_Players.Add(newPlayer);
        }

        public void GetComputerChoise(Player i_Player, char i_FirstCardValue, out int o_Row,out int o_Column)
        {
            (int, int) knownCard;

            if (i_FirstCardValue == k_FirstTimeValue) 
            {
                (o_Row, o_Column) = i_Player.GetRandomMove(m_GameBoard);
            }
            else
            {
                knownCard = i_Player.GetKnownCard(i_FirstCardValue, m_GameBoard);
                if (knownCard == (k_WrongIndexForComputer, k_WrongIndexForComputer))
                {
                    (o_Row, o_Column) = i_Player.GetRandomMove(m_GameBoard);
                }
                else
                {
                    (o_Row, o_Column) = knownCard;
                }
            }
        }

        public Card Move(Player i_Player, int i_Row, int i_Column)
        {
            Card resultCard;

            m_GameBoard.TurningCardByIndex(i_Row, i_Column);
            resultCard = m_GameBoard.GetCardByIndex(i_Row, i_Column);
            foreach (Player player in m_Players)
            {
                if (player.TypeOfPlayer == k_TypeOfCompuerPlayer)
                {
                    player.AddToMemory(resultCard);
                }
            }

            return resultCard;
        }

        public bool CheckIfCardsEqualAndAddingPoints(Card i_FirstCard, Card i_SecondCard, Player i_Player) 
        {
            bool isEqualCardsValues = i_FirstCard.Value == i_SecondCard.Value;

            if (isEqualCardsValues)
            {
                i_Player.NumOfPairsDiscovered += 1;
                m_GameBoard.TotalCardDiscoverd += 2;
            }
            else
            {
                m_GameBoard.TurningCard(i_FirstCard);
                m_GameBoard.TurningCard(i_SecondCard);
            }

            return isEqualCardsValues;
        }

    }
}
