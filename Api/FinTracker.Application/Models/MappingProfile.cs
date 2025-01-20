using AutoMapper;
using FinTracker.Domain.Entities;
using FinTracker.Application.Models.DTOs;
using System.Net;
using System;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<BankAccount, BankAccountDTO>();
        CreateMap<BankTransaction, BankTransactionDTO>();
    }
}