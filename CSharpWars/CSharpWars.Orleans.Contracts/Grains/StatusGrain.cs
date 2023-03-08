﻿namespace CSharpWars.Orleans.Contracts.Grains;

public interface IStatusGrain : IGrainWithGuidKey
{
    Task<StatusDto> GetStatus();
}