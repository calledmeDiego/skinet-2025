using SKYNETAPI.DTOs;
using SKYNETCORE.Entities;

namespace SKYNETAPI.Extensions;

public static class AddressMapingExtensions
{
    public static AddressDTO? ToDTO(this Address address)
    {
        if (address == null) return null;

        return new AddressDTO
        {
            Line1 = address.Line1,
            Line2 = address.Line2,
            City = address.City,
            Country = address.Country,
            PostalCode = address.PostalCode,
            State = address.State            
        };
    }

    public static Address ToEntity(this AddressDTO addressDTO)
    {
        if (addressDTO == null) throw new ArgumentNullException(nameof(addressDTO));

        return new Address
        {
            Line1 = addressDTO.Line1,
            Line2 = addressDTO.Line2,
            City = addressDTO.City,
            Country = addressDTO.Country,
            PostalCode = addressDTO.PostalCode,
            State = addressDTO.State
        };
    }

    public static void UpdateFromDTO(this Address address, AddressDTO addressDTO)
    {
        if (address == null) throw new ArgumentNullException(nameof(address));
        if (addressDTO == null) throw new ArgumentNullException(nameof(addressDTO));



        address.Line1 = addressDTO.Line1;
        address.Line2 = addressDTO.Line2;
        address.City = addressDTO.City;
        address.Country = addressDTO.Country;
        address.PostalCode = addressDTO.PostalCode;
        address.State = addressDTO.State;
        
    }
}
