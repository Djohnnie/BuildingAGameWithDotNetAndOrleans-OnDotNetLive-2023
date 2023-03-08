﻿using CSharpWars.Enums;
using CSharpWars.Orleans.Contracts.Model;

namespace CSharpWars.Scripting;

public class BotResult
{
    private readonly Dictionary<Guid, int> _damageInflicted = new();
    private readonly Dictionary<Guid, (int X, int Y)> _teleportations = new();

    public int X { get; set; }
    public int Y { get; set; }
    public Orientation Orientation { get; set; }
    public int CurrentHealth { get; set; }
    public int CurrentStamina { get; set; }
    public Dictionary<string, string> Memory { get; set; }
    public Move Move { get; set; }
    public int LastAttackX { get; set; }
    public int LastAttackY { get; set; }
    public string Message { get; set; }

    private BotResult() { }

    public static BotResult Build(BotProperties botProperties)
    {
        return new BotResult
        {
            X = botProperties.X,
            Y = botProperties.Y,
            Orientation = botProperties.Orientation,
            CurrentHealth = botProperties.CurrentHealth,
            CurrentStamina = botProperties.CurrentStamina,
            Memory = botProperties.Memory,
            Move = Move.Idling,
            LastAttackX = botProperties.MoveDestinationX,
            LastAttackY = botProperties.MoveDestinationY,
            Message = botProperties.Message
        };
    }

    public void InflictDamage(Guid botId, int damage)
    {
        if (!_damageInflicted.ContainsKey(botId))
        {
            _damageInflicted.Add(botId, 0);
        }

        _damageInflicted[botId] += damage;
    }

    public int GetInflictedDamage(Guid botId)
    {
        return _damageInflicted.TryGetValue(botId, out int value) ? value : 0;
    }

    public void Teleport(Guid botId, int destinationX, int destinationY)
    {
        if (!_teleportations.ContainsKey(botId))
        {
            _teleportations.Add(botId, (destinationX, destinationY));
        }

        _teleportations[botId] = (destinationX, destinationY);
    }

    public (int X, int Y) GetTeleportation(Guid botId)
    {
        return _teleportations.TryGetValue(botId, out (int X, int Y) value) ? value : (-1, -1);
    }
}