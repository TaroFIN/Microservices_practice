﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.CQRS;

public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    where TCommand : ICommand
{

}

public interface ICommandHandler<in TCommand, TReponse> : IRequestHandler<TCommand, TReponse>
    where TCommand : ICommand<TReponse>
    where TReponse : notnull
{

}

