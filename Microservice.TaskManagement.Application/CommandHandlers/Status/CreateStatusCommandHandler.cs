﻿using AutoMapper;
using Microservice.TaskManagement.Application.Commands.Status;
using Microservice.TaskManagement.Application.Interfaces;
using Microservice.TaskManagement.Domain.Entities;
using Microservice.TaskManagement.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.TaskManagement.Application.CommandHandlers.Status
{
    public class CreateStatusCommandHandler : ICommandHandler<CreateStatusCommand, CreateStatusCommand>
    {
        private readonly IRepository<StatusEntity> _repository;
        private readonly IMapper _mapper;
        public CreateStatusCommandHandler(IRepository<StatusEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CreateStatusCommand> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<StatusEntity>(request);
            var entityResult = await _repository.AddAsync(entity);
            var result = _mapper.Map<CreateStatusCommand>(entityResult);
            return result;
        }
    }
}
