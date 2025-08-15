using System;

namespace Mr.BrickAdventures.game.repositories;

public sealed class PlayerRepository
{
    public int Coins { get; private set; } = 0;
    public int Lives { get; private set; } = 3;

    public event Action<int>? CoinsChanged;
    public event Action<int>? LivesChanged;

    public void SetCoins(int value)    { Coins = Math.Max(0, value); CoinsChanged?.Invoke(Coins); }
    public void AddCoins(int amount)   { SetCoins(Coins + amount); }
    public void RemoveCoins(int amount){ SetCoins(Coins - amount); }

    public void SetLives(int value)    { Lives = value; LivesChanged?.Invoke(Lives); }
    public void AddLives(int amount)   { SetLives(Lives + amount); }
    public void RemoveLives(int amount){ SetLives(Lives - amount); }

    public PlayerState Export() => new() { Coins = Coins, Lives = Lives };
    public void Load(PlayerState s) { SetCoins(s.Coins); SetLives(s.Lives); }
}

public record PlayerState { public int Coins; public int Lives; }