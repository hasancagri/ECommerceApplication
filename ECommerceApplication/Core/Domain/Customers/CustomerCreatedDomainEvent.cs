﻿namespace Domain.Customers;

public record CustomerCreatedDomainEvent(Guid Id, string Name, string Address, string Email)
    : IDomainEvent;
