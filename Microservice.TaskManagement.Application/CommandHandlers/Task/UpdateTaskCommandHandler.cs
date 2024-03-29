﻿using AutoMapper;
using Microservice.TaskManagement.Application.Commands.Task;
using Microservice.TaskManagement.Application.Interfaces;
using Microservice.TaskManagement.Domain.Entities;
using Microservice.TaskManagement.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microservice.TaskManagement.Application.CommandHandlers.Task
{
    public class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand, UpdateTaskCommand>
    {
        private readonly IRepository<TaskEntity> _repository;
        private readonly IMapper _mapper;
        public UpdateTaskCommandHandler(IRepository<TaskEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<UpdateTaskCommand> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TaskEntity>(request);
            var entityResult = await _repository.UpdateAsync(entity);
            var tags = await _repository.GetAsync<TagEntityTaskEntity>(x => x.TaskId == request.Id);
            List<TagEntityTaskEntity> listAdd = new List<TagEntityTaskEntity>();
            List<TagEntityTaskEntity> listRemove = new List<TagEntityTaskEntity>();
            foreach (var tag in tags)
            {
                if (!request.Tags.Any(x => x.Value == tag.TagId))
                {
                    listRemove.Add(tag);
                }
            }
            foreach(var row in request.Tags)
            {
                if (!tags.Any(x => x.TagId == row.Value))
                {
                    listAdd.Add(new TagEntityTaskEntity() { TagId = row.Value, TaskId = request.Id });
                }
            }
            if (listAdd.Count > 0)
            {
                await _repository.AddRangeAsync(listAdd);
            }
            if (listRemove.Count > 0)
            {
                await _repository.RemoveRangeAsync(listRemove);
            }
            var result = _mapper.Map<UpdateTaskCommand>(entityResult);

            return result;
        }
    }
}
