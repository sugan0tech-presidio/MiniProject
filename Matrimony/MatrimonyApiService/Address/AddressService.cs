﻿using AutoMapper;
using MatrimonyApiService.Commons;

namespace MatrimonyApiService.Address;

public class AddressService(IBaseRepo<Address> addressRepo, IMapper mapper, ILogger<AddressService> logger) : IAddressService
{
    /// <inheritdoc/>
    public async Task<AddressDto> GetAddressById(int id)
    {
        try
        {
            var addressEntity = await addressRepo.GetById(id);
            return mapper.Map<AddressDto>(addressEntity);

        }
        catch (KeyNotFoundException e)
        {
            logger.LogError(e.Message);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<List<AddressDto>> GetAllAddresses()
    {
        var addressEntities = await addressRepo.GetAll();
        var addressDtos = new List<AddressDto>();
        foreach (var addressEntity in addressEntities)
        {
            addressDtos.Add(mapper.Map<AddressDto>(addressEntity));
        }

        return addressDtos;
    }

    /// <inheritdoc/>
    public async Task<AddressDto> AddAddress(AddressDto addressDto)
    {
        var addressEntity = mapper.Map<Address>(addressDto);
        var addedAddressEntity = await addressRepo.Add(addressEntity);
        return mapper.Map<AddressDto>(addedAddressEntity);
    }

    /// <inheritdoc/>
    public async Task<AddressDto> UpdateAddress(AddressDto addressDto)
    {
        try
        {
            if (addressDto == null)
                throw new ArgumentNullException(nameof(addressDto));
            var updatedAddressEntity = mapper.Map<Address>(addressDto);
            updatedAddressEntity.Id = addressDto.AddressId; // Ensure the ID is set to the correct value
            var result = await addressRepo.Update(updatedAddressEntity);
            return mapper.Map<AddressDto>(result);
        }
        catch(KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw ;
        }
    }

    /// <inheritdoc/>
    public async Task<AddressDto> DeleteAddressById(int id)
    {
        try
        {
            var deletedAddressEntity = await addressRepo.DeleteById(id);
            return mapper.Map<AddressDto>(deletedAddressEntity);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex.Message);
            throw;
        }
    }
}