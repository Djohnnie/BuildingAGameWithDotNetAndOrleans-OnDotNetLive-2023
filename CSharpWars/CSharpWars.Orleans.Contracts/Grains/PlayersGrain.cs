﻿namespace CSharpWars.Orleans.Contracts.Grains;

public interface IPlayersGrain : IGrainWithGuidKey
{
    Task<PlayerDto> Login(string username, string password);
    Task DeleteAllPlayers();
}