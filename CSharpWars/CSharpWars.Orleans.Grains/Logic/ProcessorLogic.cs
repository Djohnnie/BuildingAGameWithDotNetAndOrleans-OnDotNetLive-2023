﻿using CSharpWars.Orleans.Common;
using CSharpWars.Orleans.Contracts.Grains;
using CSharpWars.Scripting;

namespace CSharpWars.Orleans.Grains.Logic;

public interface IProcessorLogic
{
    Task Go(string arenaName);
}

public class ProcessorLogic : IProcessorLogic
{
    private readonly IGrainFactoryHelperWithStringKey<IArenaGrain> _arenaGrainFactory;
    private readonly IGrainFactoryHelperWithGuidKey<IBotGrain> _botGrainFactory;
    private readonly IPreprocessingLogic _preprocessingLogic;
    private readonly IProcessingLogic _processingLogic;
    private readonly IPostprocessingLogic _postprocessingLogic;

    public ProcessorLogic(
        IGrainFactoryHelperWithStringKey<IArenaGrain> arenaGrainFactory,
        IGrainFactoryHelperWithGuidKey<IBotGrain> botGrainFactory,
        IPreprocessingLogic preprocessingLogic,
        IProcessingLogic processingLogic,
        IPostprocessingLogic postprocessingLogic)
    {
        _arenaGrainFactory = arenaGrainFactory;
        _botGrainFactory = botGrainFactory;
        _preprocessingLogic = preprocessingLogic;
        _processingLogic = processingLogic;
        _postprocessingLogic = postprocessingLogic;
    }

    public async Task Go(string arenaName)
    {
        var (arena, allBots, bots) = await _arenaGrainFactory.FromGrain(arenaName, async arenaGrain =>
        {
            var arenaDetails = await arenaGrain.GetArenaDetails();
            var activeBots = await arenaGrain.GetAllActiveBots();
            var liveBots = await arenaGrain.GetAllLiveBots();
            return (arenaDetails, activeBots, liveBots);
        });

        var context = ProcessingContext.Build(arena, bots);

        await _preprocessingLogic.Go(context);

        await _processingLogic.Go(context);

        await _postprocessingLogic.Go(context);

        for (int i = 0; i < allBots.Count; i++)
        {
            Contracts.BotDto? bot = allBots[i];
            if (bot.TimeOfDeath < DateTime.UtcNow.AddSeconds(-10))
            {
                await _botGrainFactory.FromGrain(bot.BotId, g => g.DeleteBot(false));
            }
        }

        for (int i = 0; i < context.Bots.Count; i++)
        {
            Contracts.BotDto? bot = context.Bots[i];
            await _botGrainFactory.FromGrain(bot.BotId, g => g.UpdateState(bot));
        }
    }
}