﻿namespace MatrimonyApiService.AddressCQRS.Event;

public class AddressUpdatedEvent
{
    public int Id { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
}