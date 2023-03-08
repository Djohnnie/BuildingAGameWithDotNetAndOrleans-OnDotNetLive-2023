﻿using AutoMapper;
using CSharpWars.Orleans.Contracts;
using CSharpWars.WebApi.Contracts;

namespace CSharpWars.Mappers;

public class StatusMapperProfile : Profile
{
    public StatusMapperProfile()
    {
        CreateMap<StatusDto, GetStatusResponse>();
    }
}