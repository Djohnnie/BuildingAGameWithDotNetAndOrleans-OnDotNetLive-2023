﻿using CSharpWars.Common.Extensions;
using CSharpWars.Common.Helpers;
using CSharpWars.Enums;
using CSharpWars.Orleans.Common;
using CSharpWars.Orleans.Contracts;
using CSharpWars.Orleans.Contracts.Exceptions;
using CSharpWars.Orleans.Contracts.Grains;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;

namespace CSharpWars.Orleans.Grains;

public class BotState
{
    public bool Exists { get; set; }
    public string BotName { get; set; }
    public string ArenaName { get; set; }
    public string PlayerName { get; set; }
    public Orientation Orientation { get; set; }
    public Move Move { get; set; }
    public int MaximumHealth { get; set; }
    public int CurrentHealth { get; set; }
    public int MaximumStamina { get; set; }
    public int CurrentStamina { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int FromX { get; set; }
    public int FromY { get; set; }
    public int LastAttackX { get; set; }
    public int LastAttackY { get; set; }
    public DateTime? TimeOfDeath { get; set; }
    public string Memory { get; set; }
}

public class BotGrain : GrainBase<IBotGrain>, IBotGrain
{
    private readonly IGrainFactoryHelperWithGuidKey<IScriptGrain> _scriptGrainFactory;
    private readonly IGrainFactoryHelperWithStringKey<IArenaGrain> _arenaGrainFactory;
    private readonly IRandomHelper _randomHelper;
    private readonly ILogger<IBotGrain> _logger;
    private readonly IPersistentState<BotState> _state;

    public BotGrain(
        IGrainFactoryHelperWithGuidKey<IScriptGrain> scriptGrainFactory,
        IGrainFactoryHelperWithStringKey<IArenaGrain> arenaGrainFactory,
        IRandomHelper randomHelper, ILogger<IBotGrain> logger,
        [PersistentState("bot", "botStore")] IPersistentState<BotState> state) : base(logger)
    {
        _scriptGrainFactory = scriptGrainFactory;
        _arenaGrainFactory = arenaGrainFactory;
        _randomHelper = randomHelper;
        _logger = logger;
        _state = state;
    }

    public Task<BotDto> GetState()
    {
        if (!_state.State.Exists)
        {
            throw new ArgumentNullException();
        }

        return Task.FromResult(new BotDto
        {
            BotId = this.GetPrimaryKey(),
            BotName = _state.State.BotName,
            PlayerName = _state.State.PlayerName,
            CurrentHealth = _state.State.CurrentHealth,
            MaximumHealth = _state.State.MaximumHealth,
            CurrentStamina = _state.State.CurrentStamina,
            MaximumStamina = _state.State.MaximumStamina,
            X = _state.State.X,
            Y = _state.State.Y,
            FromX = _state.State.FromX,
            FromY = _state.State.FromY,
            LastAttackX = _state.State.LastAttackX,
            LastAttackY = _state.State.LastAttackY,
            Orientation = _state.State.Orientation,
            Move = _state.State.Move,
            Memory = _state.State.Memory,
            TimeOfDeath = _state.State.TimeOfDeath
        });
    }

    public async Task<BotDto> CreateBot(BotToCreateDto bot, ArenaDto arena, List<BotDto> activeBots)
    {
        if (_state.State.Exists)
        {
            throw new CSharpWarsException("Bot cannot be created because it already exists");
        }

        _state.State.BotName = bot.BotName;
        _state.State.ArenaName = bot.ArenaName;
        _state.State.PlayerName = bot.PlayerName;
        _state.State.CurrentHealth = bot.MaximumHealth;
        _state.State.MaximumHealth = bot.MaximumHealth;
        _state.State.CurrentStamina = bot.MaximumStamina;
        _state.State.MaximumStamina = bot.MaximumStamina;
        _state.State.Orientation = _randomHelper.Get<Orientation>();
        (_state.State.X, _state.State.Y) = FindFreeLocation(arena, activeBots);
        (_state.State.FromX, _state.State.FromY) = (_state.State.X, _state.State.Y);
        _state.State.Memory = new Dictionary<string, string>().Serialize();
        _state.State.TimeOfDeath = DateTime.MaxValue;
        _state.State.Move = Move.Idling;
        _state.State.Exists = true;

        await _state.WriteStateAsync();

        var botId = this.GetPrimaryKey();
        await _scriptGrainFactory.FromGrain(botId, g => g.SetScript(bot.Script));

        return await GetState();
    }

    private (int X, int Y) FindFreeLocation(ArenaDto arena, List<BotDto> activeBots)
    {
        var freeLocations = new List<(int X, int Y)>();

        for (int y = 0; y < arena.Width; y++)
        {
            for (int x = 0; x < arena.Width; x++)
            {
                if (!activeBots.Any(b => b.X == x && b.Y == y))
                {
                    freeLocations.Add((x, y));
                }
            }
        }

        if (freeLocations.Count == 0)
        {
            throw new CSharpWarsException("Your robot cannot be deployed because the arena is full :(");
        }

        return _randomHelper.GetItem(freeLocations);
    }

    public async Task DeleteBot(bool clearArena)
    {
        if (_state.State.Exists)
        {
            var botId = this.GetPrimaryKey();
            await _scriptGrainFactory.FromGrain(botId, g => g.DeleteScript());

            if (!clearArena)
            {
                await _arenaGrainFactory.FromGrain(_state.State.ArenaName, g => g.DeleteBot(botId));
            }

            await _state.ClearStateAsync();
        }

        DeactivateOnIdle();
    }

    public async Task UpdateState(BotDto bot)
    {
        if (_state.State.Exists)
        {
            _state.State.CurrentHealth = bot.CurrentHealth;
            _state.State.CurrentStamina = bot.CurrentStamina;
            _state.State.Orientation = bot.Orientation;
            _state.State.X = bot.X;
            _state.State.Y = bot.Y;
            _state.State.FromX = bot.FromX;
            _state.State.FromY = bot.FromY;
            _state.State.LastAttackX = bot.LastAttackX;
            _state.State.LastAttackY = bot.LastAttackY;
            _state.State.Memory = bot.Memory;
            _state.State.TimeOfDeath = bot.TimeOfDeath;
            _state.State.Move = bot.Move;

            await _state.WriteStateAsync();
        }
    }
}