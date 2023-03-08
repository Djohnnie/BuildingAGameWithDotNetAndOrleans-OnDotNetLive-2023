﻿namespace CSharpWars.Web.Models;

public class PlayViewModel
{
    public string HappyMessage { get; set; }
    public string SadMessage { get; set; }
    public string PlayerName { get; set; }
    public string BotName { get; set; }
    public int BotHealth { get; set; }
    public int BotStamina { get; set; }
    public string Script { get; set; }
    public Guid SelectedScript { get; set; }
    public IList<Template> Scripts { get; set; }
}