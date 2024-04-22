using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Windows;
using System.IO;
using System.Linq;

using Image = System.Windows.Controls.Image;
using Path = System.IO.Path;

using System.Collections.ObjectModel;
using Microsoft.Win32;

namespace Checkers
{
    internal class MenuCommands : INotifyPropertyChanged
    {
        // Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event MouseButtonEventHandler MouseDown;

        // Class fields
        private List<Tuple<int, int>> validMoves = new List<Tuple<int, int>>();
        private Piece currentPiece;
        private bool allowMultipleMoves, kingPieces, fewPieces;
        private bool capturatedPiece;


        // Declarations with PropertyChanged
        private Game _game;
        public Game Game
        {
            get { return _game; }
            set
            {
                if (_game != value)
                {
                    _game = value;
                    NotifyPropertyChanged(nameof(Game));
                    NotifyPropertyChanged(nameof(Board));
                }
            }
        }

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    NotifyPropertyChanged(nameof(CurrentPlayer));
                }
            }
        }

        private string _round;
        public string Round
        {
            get { return _round; }
            set
            {
                if (_round != value)
                {
                    _round = value;
                    NotifyPropertyChanged(nameof(Round));
                }
            }
        }

        private string _numberPiecesRed;
        public string NumberPiecesRed
        {
            get { return _numberPiecesRed; }
            set
            {
                if (_numberPiecesRed != value)
                {
                    _numberPiecesRed = value;
                    NotifyPropertyChanged(nameof(NumberPiecesRed));
                }
            }
        }

        private string _numberPiecesWhite;
        public string NumberPiecesWhite
        {
            get { return _numberPiecesWhite; }
            set
            {
                if (_numberPiecesWhite != value)
                {
                    _numberPiecesWhite = value;
                    NotifyPropertyChanged(nameof(NumberPiecesWhite));
                }
            }
        }

        // Stats
        private int _redWonGames;
        public int RedWonGames
        {
            get { return _redWonGames; }
            set
            {
                if (_redWonGames != value)
                {
                    _redWonGames = value;
                    NotifyPropertyChanged(nameof(RedWonGames));
                }
            }
        }

        private int _whiteWonGames;
        public int WhiteWonGames
        {
            get { return _whiteWonGames; }
            set
            {
                if (_whiteWonGames != value)
                {
                    _whiteWonGames = value;
                    NotifyPropertyChanged(nameof(WhiteWonGames));
                }
            }
        }

        private int _maxPiecesBoard;
        public int MaxPiecesBoard
        {
            get { return _maxPiecesBoard; }
            set
            {
                if (_maxPiecesBoard != value)
                {
                    _maxPiecesBoard = value;
                    NotifyPropertyChanged(nameof(MaxPiecesBoard));
                }
            }
        }

        private int _totalPieces;
        public int TotalPieces
        {
            get { return _totalPieces; }
            set
            {
                if (_totalPieces != value)
                {
                    _totalPieces = value;
                    NotifyPropertyChanged(nameof(TotalPieces));
                }
            }
        }



        // const values
        private const string AboutMessage = "Name: Damian Mihai Cristian\n" +
                                            "Group: 10LF221\n" +
                                            "Email: mihai.damian@student.unitbv.ro\n" +
                                            "\r\nCheckers is a classic board game played on an 8x8 grid with alternating dark and light squares. Each player starts with 12 pieces, typically distinguished by color, placed on the dark squares of their respective sides of the board. The primary objective is to capture all of the opponent's pieces or to block them from making any legal moves.\r\n\r\n" +
                                            "In Checkers, pieces move diagonally forward, one square at a time, onto empty adjacent squares. They can capture an opponent's piece by jumping over it diagonally, provided the square immediately beyond is vacant. Multiple captures can be made in a single turn if the opportunity arises, leading to strategic play and the potential for rapid advancement.\r\n\r\n" +
                                            "When a player's piece reaches the farthest row on the opponent's side of the board, it is \"kinged\" or \"crowned,\" becoming a \"king.\" A king is typically denoted by placing a second piece of the same color on top of the original piece. Kings gain enhanced movement capabilities, allowing them to move and capture both forwards and backwards diagonally. This elevation in status enhances their strategic value and introduces additional layers of complexity to the game.";



        // Commands
        private ICommand saveGame;
        private ICommand chooseFewPieces;
        private ICommand newGame;
        private ICommand about;
        private ICommand closeGame;
        private ICommand chooseKingPieces;
        private ICommand showStatistics;
        private ICommand openGame;
        private ICommand chooseMultipleJump;
        private ICommand save;

        // Constructor
        public MenuCommands()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject()))
            {
                return;
            }
            ReadStatistics();
        }


        // Statistics related methods
        private void SaveStatistics()
        {
            if (TotalPieces > MaxPiecesBoard)
                MaxPiecesBoard = TotalPieces;

            var json = new
            {
                RedWonGames,
                WhiteWonGames,
                MaxPiecesBoard
            };

            string jsonString = JsonConvert.SerializeObject(json, Formatting.Indented);
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saves", "Statistics.txt");

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            File.WriteAllText(path, jsonString);
        }
        private void ReadStatistics()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Saves", "Statistics.txt");

            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }

            string fileContent = File.ReadAllText(path);
        }



        // Count pieces
        private void CountPieces()
        {
            NumberPiecesRed = Game.Board.Pieces.SelectMany(x => x).Count(x => x.Color == ColorType.Red).ToString();
            NumberPiecesWhite = Game.Board.Pieces.SelectMany(x => x).Count(x => x.Color == ColorType.White).ToString();
            TotalPieces = Game.Board.Pieces.SelectMany(x => x).Count(x => x.Color == ColorType.White || x.Color == ColorType.Red);
        }



        // Valid moves
        public void ValidMoves(int row, int column)
        {
            if (column < 0 || column > 7)
            {
                return;
            }

            Piece currentPiece = Game.Board.Pieces[row][column];

            if (currentPiece.Type == PieceType.King || currentPiece.Color == ColorType.White)
            {
                AddValidMove(row + 1, column - 1);
                AddValidMove(row + 1, column + 1);
            }

            if (currentPiece.Type == PieceType.King || currentPiece.Color == ColorType.Red)
            {
                AddValidMove(row - 1, column - 1);
                AddValidMove(row - 1, column + 1);
            }

            ValidMovesCaptureOpponent(row, column);
        }
        private void ValidMovesCaptureOpponent(int row, int column)
        {
            Piece currentPiece = Game.Board.Pieces[row][column];

            if (currentPiece.Type == PieceType.King || currentPiece.Color == ColorType.White)
            {
                AddValidCaptureMove(row + 1, column - 1, row + 2, column - 2);
                AddValidCaptureMove(row + 1, column + 1, row + 2, column + 2);
            }

            if (currentPiece.Type == PieceType.King || currentPiece.Color == ColorType.Red)
            {
                AddValidCaptureMove(row - 1, column - 1, row - 2, column - 2);
                AddValidCaptureMove(row - 1, column + 1, row - 2, column + 2);
            }
        }
        private void AddValidCaptureMove(int captureRow, int captureColumn, int landingRow, int landingColumn)
        {
            if (IsWithinBoard(captureRow, captureColumn) && IsWithinBoard(landingRow, landingColumn))
            {
                Piece capturePiece = Game.Board.Pieces[captureRow][captureColumn];
                Piece landingPiece = Game.Board.Pieces[landingRow][landingColumn];

                if (capturePiece.Type != PieceType.None && capturePiece.Color != currentPiece.Color && landingPiece.Type == PieceType.None)
                {
                    validMoves.Add(Tuple.Create(landingRow, landingColumn));
                }
            }
        }
        private void AddValidMove(int row, int column)
        {
            if (row >= 0 && row < 8 && column >= 0 && column < 8)
            {
                if (Game.Board.Pieces[row][column].Type == PieceType.None)
                {
                    validMoves.Add(Tuple.Create(row, column));
                }
            }
        }
        private void ShowValidMoves()
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Images");
            foreach (var move in validMoves)
            {
                Console.WriteLine("\n");
                Console.WriteLine($"Updating image for piece at position ({move.Item1}, {move.Item2})");
                Game.Board.Pieces[move.Item1][move.Item2].ImagePath = Path.Combine(directoryPath, "PossibleMove.png");
                Console.WriteLine($"New ImagePath: {Game.Board.Pieces[move.Item1][move.Item2].ImagePath}");
                Console.WriteLine("\n");
            }
        }
        private void ClearValidMoves()
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../Images");
            string possibleMovePath = Path.Combine(directoryPath, "PossibleMove.png");
            string darkSquarePath = Path.Combine(directoryPath, "DarkSquare.png");

            foreach (var row in Game.Board.Pieces)
            {
                foreach (var piece in row)
                {
                    if (piece.ImagePath == possibleMovePath)
                    {
                        piece.ImagePath = darkSquarePath;
                    }
                }
            }
        }
        private bool IsWithinBoard(int row, int column)
        {
            return row >= 0 && row < 8 && column >= 0 && column < 8;
        }



        // Handling mouse events
        public void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Image clickedImage = sender as Image;
            if (clickedImage == null) return;

            Piece clickedPiece = clickedImage.DataContext as Piece;
            if (clickedPiece == null) return;

            if (e.LeftButton == MouseButtonState.Pressed && !capturatedPiece)
            {
                HandleLeftClick(clickedPiece);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                HandleRightClick(clickedPiece);
            }
        }
        private void HandleLeftClick(Piece clickedPiece)
        {
            if (clickedPiece.Type != PieceType.None && clickedPiece.Color != ColorType.None && clickedPiece.Color == CurrentPlayer.Color)
            {
                currentPiece = clickedPiece;
                validMoves.Clear();
                ClearValidMoves();
                ValidMoves(clickedPiece.Coordonates.Item1, clickedPiece.Coordonates.Item2);
                if (validMoves.Count > 0)
                {
                    ShowValidMoves();
                    NotifyPropertyChanged("Game");
                }
            }
        }
        private void HandleRightClick(Piece clickedPiece)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.GetFullPath(Path.Combine(basePath, "../../Images"));
            Console.WriteLine("R. CLICK @ " + clickedPiece.Coordonates.Item1 + " " + clickedPiece.Coordonates.Item2 + '\n' + directoryPath);
            if (clickedPiece.Type == PieceType.None && clickedPiece.ImagePath == Path.Combine(directoryPath, $"PossibleMove.png"))
            {
                PerformMove(clickedPiece, directoryPath);
            }

            Round = CurrentPlayer.Color.ToString() + " player's turn.";
            CountPieces();
            CheckForGameEnd();
        }



        // Move related methods
        private void PerformMove(Piece clickedPiece, string directoryPath)
        {
            PromotePieceIfNecessary(clickedPiece, directoryPath);
            CapturePieceIfJumpedOver(clickedPiece, directoryPath);
            MovePieceToNewLocation(clickedPiece, directoryPath);
            ClearValidMoves();
            CheckForAdditionalValidMoves(clickedPiece);
            SwitchCurrentPlayer();
        }
        private void PromotePieceIfNecessary(Piece clickedPiece, string directoryPath)
        {
            if ((clickedPiece.Coordonates.Item1 == 7 || clickedPiece.Coordonates.Item1 == 0) && currentPiece.Type == PieceType.Pawn)
            {
                currentPiece.Type = PieceType.King;
                currentPiece.ImagePath = currentPiece.Color == ColorType.Red
                    ? Path.Combine(directoryPath, $"PieceRedKing.png")
                    : Path.Combine(directoryPath, $"PieceWhiteKing.png");
            }
        }
        private void CapturePieceIfJumpedOver(Piece clickedPiece, string directoryPath)
        {
            if (Math.Abs(currentPiece.Coordonates.Item1 - clickedPiece.Coordonates.Item1) == 2)
            {
                int middleRow = (currentPiece.Coordonates.Item1 + clickedPiece.Coordonates.Item1) / 2;
                int middleCol = (currentPiece.Coordonates.Item2 + clickedPiece.Coordonates.Item2) / 2;
                Game.Board.Pieces[middleRow][middleCol] = new Piece(PieceType.None, ColorType.None, Path.Combine(directoryPath, $"DarkSquare.png"), Tuple.Create(middleRow, middleCol));
                capturatedPiece = true;
            }
        }
        private void MovePieceToNewLocation(Piece clickedPiece, string directoryPath)
        {
            Game.Board.Pieces[currentPiece.Coordonates.Item1][currentPiece.Coordonates.Item2] = new Piece(PieceType.None, ColorType.None, Path.Combine(directoryPath, $"DarkSquare.png"), Tuple.Create(currentPiece.Coordonates.Item1, currentPiece.Coordonates.Item2));
            Game.Board.Pieces[clickedPiece.Coordonates.Item1][clickedPiece.Coordonates.Item2] = new Piece(currentPiece.Type, currentPiece.Color, currentPiece.ImagePath, Tuple.Create(clickedPiece.Coordonates.Item1, clickedPiece.Coordonates.Item2));
        }
        private void CheckForAdditionalValidMoves(Piece clickedPiece)
        {
            if (allowMultipleMoves && capturatedPiece)
            {
                currentPiece = Game.Board.Pieces[clickedPiece.Coordonates.Item1][clickedPiece.Coordonates.Item2];
                ValidMovesCaptureOpponent(currentPiece.Coordonates.Item1, currentPiece.Coordonates.Item2);
                if (validMoves.Count > 0)
                {
                    ShowValidMoves();
                    NotifyPropertyChanged("Game");
                    return;
                }
            }
        }
        private void SwitchCurrentPlayer()
        {
            CurrentPlayer = CurrentPlayer.Color == ColorType.Red ? Game.Player2 : Game.Player1;
            capturatedPiece = false;
        }
        private void CheckForGameEnd()
        {
            if (NumberPiecesRed == "0")
            {
                RedWonGames++;
                SaveStatistics();
                MessageBox.Show("White player won the game!", "Game Status");
            }
            else if (NumberPiecesWhite == "0")
            {
                WhiteWonGames++;
                SaveStatistics();
                MessageBox.Show("Red player won the game!", "Game Status");
            }
        }



        // Initialize board & pieces
        public void InitializeBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                ObservableCollection<Piece> rowPieces = new ObservableCollection<Piece>();
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = CreatePiece(row, col);
                    rowPieces.Add(piece);
                }
                Game.Board.Pieces.Add(rowPieces);
            }
            CountPieces();
            Round = CurrentPlayer.Color.ToString() + " player's turn.";
            NotifyPropertyChanged("Game");
        }
        private Piece CreatePiece(int row, int col)
        {
            PieceType type = PieceType.None;
            ColorType color = ColorType.None;

            if (ShouldPlaceWhitePiece(row, col))
            {
                color = ColorType.White;
                type = kingPieces ? PieceType.King : PieceType.Pawn;
            }
            else if (ShouldPlaceRedPiece(row, col))
            {
                color = ColorType.Red;
                type = kingPieces ? PieceType.King : PieceType.Pawn;
            }

            return new Piece(type, color, GetImagePath(type, color, col, row), Tuple.Create(row, col));
        }
        private bool ShouldPlaceWhitePiece(int row, int col)
        {
            return ((row == 0 || (!fewPieces && row == 2)) && col % 2 == 1) ||
                   (!fewPieces && row == 1 && col % 2 == 0);
        }
        private bool ShouldPlaceRedPiece(int row, int col)
        {
            bool shouldPlace = (((!fewPieces && row == 5) || row == 7) && col % 2 == 0) ||
                               (!fewPieces && row == 6 && col % 2 == 1);
            if (shouldPlace)
            {
                Console.WriteLine($"Placing red piece at row {row}, col {col}");
            }
            return shouldPlace;
        }


        // For directory paths
        private string GetDirectoryPath(string relativePath)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(basePath, relativePath));
        }



        // Image related methods
        private string GetImagePath(PieceType type, ColorType color, int row, int col)
        {
            string directoryPath = GetDirectoryPath("../../Images");
            // Console.WriteLine(directoryPath);
            if (type == PieceType.None && color == ColorType.None)
            {
                return GetSquareImagePath(directoryPath, row, col);
            }
            else
            {
                return GetPieceImagePath(directoryPath, type, color);
            }
        }
        private string GetSquareImagePath(string directoryPath, int row, int col)
        {
            string squareColor = col % 2 == row % 2 ? "Light" : "Dark";
            return Path.Combine(directoryPath, $"{squareColor}Square.png");
        }
        private string GetPieceImagePath(string directoryPath, PieceType type, ColorType color)
        {
            string colorString = color == ColorType.White ? "White" : "Red";
            string typeString = type == PieceType.Pawn ? "Pawn" : "King";
            Console.WriteLine(Path.Combine(directoryPath, $"Piece{colorString}{typeString}.png"));
            return Path.Combine(directoryPath, $"Piece{colorString}{typeString}.png");
        }

        // Save
        public void SaveGame(object parameter)
        {
            string filePath = GenerateSaveFilePath();
            string gameData = SerializeGameData();

            File.WriteAllText(filePath, gameData);
        }
        private string GenerateSaveFilePath()
        {
            string directoryPath = "Saves";
            int numFiles = Directory.GetFiles(directoryPath, "*.txt").Length;
            string fileName = $"Save_{numFiles}.txt";

            return Path.Combine(directoryPath, fileName);
        }
        private string SerializeGameData()
        {
            var gameData = new
            {
                Game.Board.Pieces,
                Game.Player1,
                Game.Player2,
                CurrentPlayer,
                currentPiece,
                validMoves,
                allowMultipleMoves,
                kingPieces,
                fewPieces,
                capturatedPiece,
            };

            return JsonConvert.SerializeObject(gameData, Formatting.Indented);
        }



        // Open
        public void OpenGame(object parameter)
        {
            OpenFileDialog openFileDialog = CreateOpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string fileContent = File.ReadAllText(openFileDialog.FileName);
                JObject jsonObject = JObject.Parse(fileContent);
                LoadGameFromJson(jsonObject);
                NotifyPropertyChanged("Game");
            }
        }
        private OpenFileDialog CreateOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.GetFullPath(Path.Combine(basePath, "Saves"));
            openFileDialog.InitialDirectory = directoryPath;
            return openFileDialog;
        }
        private void LoadGameFromJson(JObject jsonObject)
        {
            Game = new Game();

            Game.Board.Pieces = ConvertToObservableCollection(jsonObject["Pieces"]?.ToObject<Piece[][]>()); Game.Player1 = jsonObject["Player1"]?.ToObject<Player>();
            Game.Player1 = jsonObject["Player1"]?.ToObject<Player>();
            Game.Player2 = jsonObject["Player2"]?.ToObject<Player>();
            CurrentPlayer = jsonObject["CurrentPlayer"]?.ToObject<Player>();
            currentPiece = jsonObject["currentPiece"]?.ToObject<Piece>();
            validMoves = jsonObject["validMoves"]?.ToObject<List<Tuple<int, int>>>();
            allowMultipleMoves = jsonObject["allowMultipleMoves"]?.ToObject<bool>() ?? false;
            kingPieces = jsonObject["kingPieces"]?.ToObject<bool>() ?? false;
            fewPieces = jsonObject["fewPieces"]?.ToObject<bool>() ?? false;
            capturatedPiece = jsonObject["capturatedPiece"]?.ToObject<bool>() ?? false;

            CountPieces();
            UpdateRound();
        }
        private void UpdateRound()
        {
            Round = CurrentPlayer != null ? CurrentPlayer.Color.ToString() + " player's turn." : "No current player.";
        }



        // New Game methods

        // Classic Checkers
        public void DefaultNewGame(object parameter)
        {
            Game = new Game();
            CurrentPlayer = Game.Player1;
            allowMultipleMoves = false;
            kingPieces = false;
            fewPieces = false;
            InitializeBoard();
        }
        // Gamemodes
        public void MultipleJumpNewGame(object parameter)
        {
            Game = new Game();
            CurrentPlayer = Game.Player1;
            allowMultipleMoves = true;
            kingPieces = false;
            fewPieces = false;
            InitializeBoard();
        }
        public void FewPiecesNewGame(object parameter)
        {
            Game = new Game();
            CurrentPlayer = Game.Player1;
            allowMultipleMoves = false;
            kingPieces = false;
            fewPieces = true;
            InitializeBoard();
        }
        public void KingPiecesNewGame(object parameter)
        {
            Game = new Game();
            CurrentPlayer = Game.Player1;
            allowMultipleMoves = false;
            kingPieces = true;
            fewPieces = false;
            InitializeBoard();
        }



        // Show methods
        public void ShowStatisticsMethod(object parameter)
        {
            string message = $"Number of games won by the red team: {RedWonGames}\n" +
                     $"Number of games won by the white team: {WhiteWonGames}\n" +
                     $"Maximum number of pieces remaining on the board: {MaxPiecesBoard}";
            MessageBox.Show(message, "Statistics");
        }
        public void ShowAbout(object parameter)
        {
            MessageBox.Show(AboutMessage, "About");
        }




        // ICommand methods
        public ICommand Statistics
        {
            get
            {
                if (showStatistics == null)
                {
                    showStatistics = new RelayCommand(ShowStatisticsMethod);
                }
                return showStatistics;
            }
        }
        public ICommand Open
        {
            get
            {
                if (openGame == null)
                    openGame = new RelayCommand(OpenGame);
                return openGame;
            }
        }
        public ICommand About
        {
            get
            {
                if (about == null)
                {
                    about = new RelayCommand(ShowAbout);
                }
                return about;
            }
        }
        public ICommand NewGame
        {
            get
            {
                if (newGame == null)
                    newGame = new RelayCommand(DefaultNewGame);
                return newGame;
            }
        }
        public ICommand Save
        {
            get
            {
                if (saveGame == null)
                    saveGame = new RelayCommand(SaveGame);
                return saveGame;
            }
        }
        public ICommand ChooseKingPieces
        {
            get
            {
                if (chooseKingPieces == null)
                {
                    chooseKingPieces = new RelayCommand(KingPiecesNewGame);
                }
                return chooseKingPieces;
            }
        }
        public ICommand ChooseMultipleJump
        {
            get
            {
                if (chooseMultipleJump == null)
                {
                    chooseMultipleJump = new RelayCommand(MultipleJumpNewGame);
                }
                return chooseMultipleJump;
            }
        }
        public ICommand ChooseFewPieces
        {
            get
            {
                if (chooseFewPieces == null)
                {
                    chooseFewPieces = new RelayCommand(FewPiecesNewGame);
                }
                return chooseFewPieces;
            }
        }




        // Misc methods needed
        private ObservableCollection<ObservableCollection<Piece>> ConvertToObservableCollection(Piece[][] pieces)
        {
            var result = new ObservableCollection<ObservableCollection<Piece>>();
            foreach (var row in pieces)
            {
                result.Add(new ObservableCollection<Piece>(row));
            }
            return result;
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}