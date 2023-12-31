﻿using MediatR;

namespace Discount.Application.Commands;

public sealed record DeleteDiscountCommand(string ProductName) : IRequest<bool>;