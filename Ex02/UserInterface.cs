using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using Logic;

namespace UI
{
    public class Card
    {
        private char m_Value;
        private bool m_IsCoverd;
        private int m_RowIndex;
        private int m_ColumnIndex;

        public Card(char i_Value, int i_Row, int i_Column)
        {
            m_Value = i_Value;
            m_IsCoverd = true;
            m_RowIndex = i_Row;
            m_ColumnIndex = i_Column;
        }

        public char Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

        public bool IsCovered
        {
            get
            {
                return m_IsCoverd;
            }
            set
            {
                m_IsCoverd = value;
            }
        }

        public int RowIndex
        {
            get
            {
                return m_RowIndex;
            }
        }

        public int ColumnIndex
        {
            get
            {
                return m_ColumnIndex;
            }
        }

    }
    public class UserInterface
    {
        private const int k_NotOkIndex = -2;
        private const char k_WantToExit = 'Q';
        private const int k_WantToExitIndex = -1;
        private const char k_TypeOfHumanPlayer = 'P';
        private const char k_TypeOfCompuerPlayer = 'C';
        private const int k_MinLength = 4; 
        private const int k_MaxLength = 6;
        private const int k_NumOfPlayers = 2;
        private const char k_FirstTimeValue = '-';
        private Game m_TheGame;
        private int m_Hight;
        private int m_Width;

        private void printEntryScreen()
        {
            Console.WriteLine("******************************************");
            Console.WriteLine("*                                        *");
            Console.WriteLine("*          Welcome to Memory Game        *");
            Console.WriteLine("*                                        *");
            Console.WriteLine("******************************************");
            Console.WriteLine();
        }

        private string getNameOfPlayer()
        {
            string nameOfPlayer;

            Console.WriteLine("Please enter your name:");
            nameOfPlayer = Console.ReadLine();
            return nameOfPlayer;
        }

        private char askUserPlayerOrComputer()
        {
            char humanOrPlayer;

            Console.WriteLine(@"Please enter '{0}' if you want another player or '{1}' for computer:", k_TypeOfHumanPlayer, k_TypeOfCompuerPlayer);
            while (!char.TryParse(Console.ReadLine(), out humanOrPlayer) || (humanOrPlayer != k_TypeOfHumanPlayer && humanOrPlayer != k_TypeOfCompuerPlayer))
            {
                Console.WriteLine("Wrong Input ! try again");
                Console.WriteLine("Enter '{0}' if you want another player or '{1}' for computer:", k_TypeOfHumanPlayer, k_TypeOfCompuerPlayer);
            }

            return humanOrPlayer;
        }

        private void setBoardSize()
        {
            int hight, width;

            checkIfIntType(out hight, out width);
            while(!m_TheGame.GameBoard.CheckIfSizeOk(hight, width))
            {
                Console.WriteLine("Invalid input! You must enter two numbers between 4 - 6 and thier muliply should be even ");
                checkIfIntType(out hight, out width);
            }

            m_Hight = hight;
            m_Width = width;
            initBorad();
        }

        private void initBorad()
        {

            List<(int, int)> availableIndices = new List<(int, int)>();
            Random random = new Random();
            int firstIndex, secondIndex;
            char valueOfCard = 'A';

            m_TheGame.GameBoard.Height = m_Hight;
            m_TheGame.GameBoard.Wight = m_Width;
            m_TheGame.GameBoard.TheBoard = new Card[m_Hight, m_Width];
            for (int i = 0; i < m_Hight; i++)
            {
                for (int j = 0; j < m_Width; j++)
                {
                    availableIndices.Add((i, j));
                }
            }
            while (availableIndices.Count > 0)
            {
                firstIndex = random.Next(availableIndices.Count);
                (int, int) cell1 = availableIndices[firstIndex];

                availableIndices.RemoveAt(firstIndex);
                secondIndex = random.Next(availableIndices.Count);
                (int, int) cell2 = availableIndices[secondIndex];

                availableIndices.RemoveAt(secondIndex);
                m_TheGame.GameBoard.TheBoard[cell1.Item1, cell1.Item2] = new Card(valueOfCard, cell1.Item1, cell1.Item2);
                m_TheGame.GameBoard.TheBoard[cell2.Item1, cell2.Item2] = new Card(valueOfCard, cell2.Item1, cell2.Item2);
                valueOfCard++;
            }

            m_TheGame.GameBoard.TotalCardDiscoverd = 0;
        }

        private void checkIfIntType(out int o_Hight, out int o_Width)
        {
            int hight, width;
            bool checkHight, checkWidth;

            Console.WriteLine("Please enter width and hight:(4-6)");
            checkWidth = int.TryParse(Console.ReadLine(), out width);
            checkHight = int.TryParse(Console.ReadLine(), out hight);
            while (!checkHight || !checkWidth || hight > k_MaxLength || hight < k_MinLength || width > k_MaxLength || width < k_MinLength)
            {
                Console.WriteLine("Wrong input! You must enter numbers");
                Console.WriteLine("Please enter hight and width:(4-6)");
                checkWidth = int.TryParse(Console.ReadLine(), out width);
                checkHight = int.TryParse(Console.ReadLine(), out hight);
            }

            o_Hight = hight;
            o_Width = width;    
        }

        private void printBoard()
        {
            char colomTitle = 'A';
            StringBuilder bufferString = createBuffer(m_Width);
            Card currentCard;

            Ex02.ConsoleUtils.Screen.Clear();
            Console.Write("   ");
            for (int i = 0; i < m_Width; i++)
            {
                Console.Write(" " + colomTitle + "  ");
                colomTitle++;
            }
            for (int i = 0; i < m_Hight; i++)
            {
                Console.WriteLine();
                Console.WriteLine(bufferString);
                Console.Write((i + 1) + " | ");
                for (int j = 0; j < m_Width; j++)
                {
                    currentCard = m_TheGame.GameBoard.GetCardByIndex(i, j);
                    if (currentCard.IsCovered == false)
                    {
                        Console.Write(currentCard.Value);
                    }
                    else
                    {
                        Console.Write(" ");
                    }

                    Console.Write(" | ");
                }
            }

            Console.WriteLine();
            Console.WriteLine(bufferString);
            Console.WriteLine();
        }

        private  StringBuilder createBuffer(int i_Width)
        {
            StringBuilder stringBuilderBuffer = new StringBuilder();

            stringBuilderBuffer.Append("  ");
            for (int i = 0; i < (i_Width * 4) + 1; i++)
            {
                stringBuilderBuffer.Append("=");
            }

            return stringBuilderBuffer;
        }

        private void getUserChoise(out int o_CardRow, out int o_CardColumn)
        {
            string userChoise;
            StringBuilder errorMessage = new StringBuilder();
            int numberOfRow;

            Console.WriteLine("Please enter the wanted cell :");
            userChoise = Console.ReadLine();
            numberOfRow = checkIfChoiseOk(userChoise, ref errorMessage);
            while (numberOfRow == k_NotOkIndex) 
            {
                Console.WriteLine(errorMessage);
                errorMessage.Clear();
                userChoise = Console.ReadLine();
                numberOfRow = checkIfChoiseOk(userChoise, ref errorMessage);
            }
            if (numberOfRow == k_WantToExitIndex)
            {
                o_CardRow = k_WantToExitIndex;
                o_CardColumn = k_WantToExitIndex;
            }
            else
            {
                o_CardRow = numberOfRow - 1;
                o_CardColumn = userChoise[0] - 'A';
            }
        }

        private int checkIfChoiseOk(string i_UserChoise, ref StringBuilder io_ErrorMessage)
        {
            int numberOfRow;

            if (i_UserChoise.Length == 1 && char.Parse(i_UserChoise) == k_WantToExit)
            {
                numberOfRow = k_WantToExitIndex;
            }
            else if (i_UserChoise.Length != 2 ) 
            {

                numberOfRow = k_NotOkIndex;
                io_ErrorMessage.Append("Invalid input!");
            }
            else if (!int.TryParse(i_UserChoise[1].ToString(), out numberOfRow))
            {
                numberOfRow = k_NotOkIndex;
                io_ErrorMessage.Append("Invalid input!");
            }
            else if (char.IsLower(i_UserChoise[0]))
            {
                io_ErrorMessage.Append("Invalid input!");
                numberOfRow = k_NotOkIndex;
            }
            else if (!m_TheGame.GameBoard.IsIndexOk(i_UserChoise[0], numberOfRow, ref io_ErrorMessage))
            {
                numberOfRow = k_NotOkIndex;

            }

            return numberOfRow;
        }

        private void printEndOfAGame()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine("******************************************");
            Console.WriteLine("Game Over                                 ");
            foreach(Player player in m_TheGame.PlayerList)
            {
                Console.WriteLine(player.Name + ": " + player.NumOfPairsDiscovered);
            }

            Console.WriteLine("******************************************");
            Console.WriteLine();
        }

        private bool checkIfWantAnotherGame()
        {
            bool ifWantAnotherGame;
            int result;

            Console.WriteLine("Please enter 0 if you want another game or 1 to exit: ");
            while (!int.TryParse(Console.ReadLine(), out result) || (result != 0 && result != 1))
            {
                Console.WriteLine("Wrong Input ! try again");
                Console.WriteLine("Enter if you want another game or 1 to exit: ");
            }

            if (result == 0) 
            {
                ifWantAnotherGame = true;
            }
            else
            {
                ifWantAnotherGame = false;
                printByeBye();
            }

            return ifWantAnotherGame;
        }

        private void printByeBye()
        {
            Ex02.ConsoleUtils.Screen.Clear();
            Console.WriteLine("******************************************");
            Console.WriteLine("*                                        *");
            Console.WriteLine("*                 Bye Bye                *");
            Console.WriteLine("*                                        *");
            Console.WriteLine("******************************************");
            Console.WriteLine();
        }

        private void printPlayersTurn(Player i_Player)
        {
            StringBuilder playerTurn = new StringBuilder();

            playerTurn.Append(i_Player.Name);
            playerTurn.Append("'s turn:");
            Console.WriteLine(playerTurn);
        }

        private void printComputerChoise(int i_Row, int i_Coloumn)
        {
            char charColumn = (char)('A' + i_Coloumn);
            StringBuilder computerChoise = new StringBuilder();

            computerChoise.Append(charColumn);
            computerChoise.Append(i_Row + 1);
            Console.WriteLine(computerChoise);
        }

        private void setGame()
        {
            char humanOrCompuer;
            string playerName;

            printEntryScreen();
            m_TheGame = new Game();
            m_TheGame.GameInit(k_NumOfPlayers);
            playerName = getNameOfPlayer();
            m_TheGame.AddPlayer(playerName, k_TypeOfHumanPlayer);
            humanOrCompuer = askUserPlayerOrComputer();
            if (humanOrCompuer == k_TypeOfHumanPlayer)
            {
                playerName = getNameOfPlayer();
            }
            else
            {
                playerName = "Computer";
            }

            m_TheGame.AddPlayer(playerName, humanOrCompuer);
            setBoardSize();
        }

        private bool playerMove(Player i_Player, out Card o_FirstCard, out Card o_SecondCard)
        {
            int firstChoiseColumn, firstChoiseRow, secondChoiseColumn, secondChoiseRow;
            bool stayInTheGame = true;

            o_FirstCard = null;
            o_SecondCard = null;
            getUserChoise(out firstChoiseRow, out firstChoiseColumn);
            if (firstChoiseRow == k_WantToExitIndex)
            {
                stayInTheGame = false;
            }
            else
            {
                o_FirstCard = m_TheGame.Move(i_Player, firstChoiseRow, firstChoiseColumn);
                printBoard();
                printPlayersTurn(i_Player);
                getUserChoise(out secondChoiseRow, out secondChoiseColumn);
                if (secondChoiseRow == k_WantToExitIndex)
                {
                    stayInTheGame = false;
                }
                else
                {
                    o_SecondCard = m_TheGame.Move(i_Player, secondChoiseRow, secondChoiseColumn);
                    printBoard();
                }
            }

            return stayInTheGame;
        }

        private void computerMove(Player i_Player, out Card o_FirstCard, out Card o_SecondCard)
        {
            int firstChoiseColumn, firstChoiseRow, secondChoiseColumn, secondChoiseRow;

            m_TheGame.GetComputerChoise(i_Player, k_FirstTimeValue, out firstChoiseRow, out firstChoiseColumn);
            o_FirstCard = m_TheGame.Move(i_Player, firstChoiseRow, firstChoiseColumn);
            printBoard();
            printPlayersTurn(i_Player);
            printComputerChoise(firstChoiseRow, firstChoiseColumn);
            Thread.Sleep(1000);
            m_TheGame.GetComputerChoise(i_Player, o_FirstCard.Value, out secondChoiseRow, out secondChoiseColumn);
            o_SecondCard = m_TheGame.Move(i_Player, secondChoiseRow, secondChoiseColumn);
            printBoard();
            printPlayersTurn(i_Player);
            printComputerChoise(secondChoiseRow, secondChoiseColumn);
            Thread.Sleep(1000);
        }

        private bool move(Player i_Player)
        {
            Card firstCard, secondCard; 
            bool stayInTheGame = true; 

            printBoard();
            printPlayersTurn(i_Player);
            if (i_Player.TypeOfPlayer == k_TypeOfCompuerPlayer)
            {
                computerMove(i_Player, out firstCard, out secondCard);
            }
            else
            {
                stayInTheGame = playerMove(i_Player, out firstCard, out secondCard);
            }
            if (stayInTheGame)
            {
                if (!m_TheGame.CheckIfCardsEqualAndAddingPoints(firstCard, secondCard, i_Player))
                {
                    Thread.Sleep(2000);
                }
            }

            return stayInTheGame;
        }

        private bool runAGame()
        {
            int boardSize = m_Hight * m_Width;
            bool wantToStay = true;
            Player currentPlayer;
            int currentPlayerIndex = 0, currentPlayerPoints = 0;

            while (m_TheGame.GameBoard.TotalCardDiscoverd != boardSize && wantToStay)
            {
                currentPlayer = m_TheGame.PlayerList[currentPlayerIndex];
                currentPlayerPoints = currentPlayer.NumOfPairsDiscovered;
                wantToStay = move(currentPlayer);
                if (wantToStay)
                {
                    if (currentPlayerPoints == currentPlayer.NumOfPairsDiscovered) 
                    {
                        currentPlayerIndex++;
                    }
                }

                if (currentPlayerIndex == m_TheGame.PlayerList.Count)
                {
                    currentPlayerIndex = 0;
                }
            }

            return wantToStay; 
        }

        public void RunAllGames()
        {
            bool isDone = false;
            bool wantToStay = true;
            bool anotherGame;

            setGame();
            while (!isDone)
            {
                wantToStay = runAGame();
                if (!wantToStay)
                {
                    isDone = true;
                }
                else
                {
                    printEndOfAGame();
                    anotherGame = checkIfWantAnotherGame();
                    if (anotherGame)
                    {
                        setBoardSize();
                        foreach(Player player in m_TheGame.PlayerList)
                        {
                            player.NumOfPairsDiscovered = 0;
                            if (player.TypeOfPlayer == k_TypeOfCompuerPlayer)
                            {
                                player.ClearMemory();
                            }
                        }
                    }
                    else
                    {
                        isDone = true;
                    }
                }
            }

            printByeBye();
        }
    }
}
