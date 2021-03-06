﻿using SGA.Application.Core;
using SGA.Application.Domain.Core;
using SGA.Infra.CrossCutting.Messages;
using System.Collections.Generic;
using System.Linq;

namespace SGA.Domain.Commands.Core
{
    public abstract class Command<T> : ICommand<T>
    {
        private readonly List<string> _errors = new List<string>();
        private readonly IUnitOfWork _uow;

        protected Command(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public bool HasErrors()
        {
            return _errors.Any();
        }

        public List<string> GetErrors()
        {
            return _errors;
        }

        public abstract void Execute(T obj);

        protected void AddError(string error)
        {
            _errors.Add(error);
        }

        protected void AddErros(List<string> errors)
        {
            _errors.AddRange(errors);
        }

        protected bool Commit()
        {
            if (HasErrors()) return false;

            var commandResponse = _uow.Commit();

            if (commandResponse.HasSuccess()) return true;

            AddError(Message.MS_003);

            return false;
        }
    }
}