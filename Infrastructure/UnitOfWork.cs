﻿using Application.Interfaces;
using Domain;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public IRepository<Schedule> Schedules { get; private set; }
        public IRepository<Session> Sessions { get; private set; }
        public IRepository<Branch> Branches { get; private set; }
        public IRepository<Class> Classes { get; private set; }
        public IRepository<Trainer> Trainers { get; private set; }

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IDatabaseTransaction BeginTransaction()
        {
            return new EFDatabaseTransaction(_context.Database.BeginTransaction());
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}