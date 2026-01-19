using System;
using Microsoft.Xna.Framework;

namespace project;

public interface IGame
{
    public bool isGameOver { get; set;}
    public bool isGameStarted { get; set;}
    public bool isGamePaused { get; set;}
    public bool isGameWon { get; set; }
    public void Exit();
    public void RestartGame();
}
