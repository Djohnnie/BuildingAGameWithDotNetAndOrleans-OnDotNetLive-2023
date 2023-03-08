﻿using CSharpWars.Enums;
using CSharpWars.Orleans.Contracts.Model;

namespace CSharpWars.Scripting.Moves;

public class SelfDestruct : BaseMove
{
    public SelfDestruct(BotProperties botProperties) : base(botProperties)
    {
    }

    public override BotResult Go()
    {
        // Build result based on current properties.
        var botResult = BotResult.Build(BotProperties);

        if (BotProperties.CurrentHealth > 0)
        {
            var botsInMaximumVicinity = FindBotsInVicinity(1);
            var botsInMediumVicinity = FindBotsInVicinity(2);
            var botsInMinimumVicinity = FindBotsInVicinity(3);

            foreach (var bot in botsInMinimumVicinity)
            {
                botResult.InflictDamage(bot.Id, Constants.SELF_DESTRUCT_MIN_DAMAGE);
            }

            foreach (var bot in botsInMediumVicinity)
            {
                botResult.InflictDamage(bot.Id, Constants.SELF_DESTRUCT_MED_DAMAGE);
            }

            foreach (var bot in botsInMaximumVicinity)
            {
                botResult.InflictDamage(bot.Id, Constants.SELF_DESTRUCT_MAX_DAMAGE);
            }

            botResult.CurrentHealth = 0;
            botResult.Move = Move.SelfDestruct;
        }
        else
        {
            botResult.Move = Move.Died;
        }

        return botResult;
    }

    private IList<Bot> FindBotsInVicinity(int range)
    {
        var botsInVicinity = new List<Bot>();
        for (int x = BotProperties.X - range; x <= BotProperties.X + range; x++)
        {
            for (int y = BotProperties.Y - range; y <= BotProperties.Y + range; y++)
            {
                if (x != BotProperties.X || y != BotProperties.Y)
                {
                    var bot = BotProperties.Bots.FirstOrDefault(b => b.X == x && b.Y == y);
                    if (bot != null)
                    {
                        botsInVicinity.Add(bot);
                    }
                }
            }
        }
        return botsInVicinity;
    }
}