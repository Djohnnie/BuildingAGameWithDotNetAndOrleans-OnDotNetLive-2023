﻿using AutoMapper;
using CSharpWars.Orleans.Common;
using CSharpWars.Orleans.Contracts;
using CSharpWars.Orleans.Contracts.Grains;
using CSharpWars.WebApi.Contracts;

namespace CSharpWars.WebApi.Managers;

public interface IBotManager
{
    Task<GetAllActiveBotsResponse> GetAllActiveBots(GetAllActiveBotsRequest request);

    Task<CreateBotResponse> CreateBot(CreateBotRequest request);
}

public class BotManager : IBotManager
{
    private readonly IGrainFactoryHelperWithStringKey<IArenaGrain> _arenaGrainClient;
    private readonly IGrainFactoryHelperWithStringKey<IProcessingGrain> _processingGrainClient;
    private readonly IMapper _mapper;

    public BotManager(
        IGrainFactoryHelperWithStringKey<IArenaGrain> arenaGrainClient,
        IGrainFactoryHelperWithStringKey<IProcessingGrain> processingGrainClient,
        IMapper mapper)
    {
        _arenaGrainClient = arenaGrainClient;
        _processingGrainClient = processingGrainClient;
        _mapper = mapper;
    }

    public async Task<GetAllActiveBotsResponse> GetAllActiveBots(GetAllActiveBotsRequest request)
    {
        var bots = await _arenaGrainClient.FromGrain(request.ArenaName, g => g.GetAllActiveBots());
        await _processingGrainClient.FromGrain(request.ArenaName, g => g.Ping());
        return _mapper.Map<GetAllActiveBotsResponse>(bots);
    }

    public async Task<CreateBotResponse> CreateBot(CreateBotRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.PlayerName))
        {
            throw new ArgumentNullException(nameof(request.PlayerName));
        }

        var botToCreate = _mapper.Map<BotToCreateDto>(request);

        var bot = await _arenaGrainClient.FromGrain(request.ArenaName, g => g.CreateBot(request.PlayerName, botToCreate));
        return _mapper.Map<CreateBotResponse>(bot);
    }
}