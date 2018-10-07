using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PaypalIntegration.DTO;

namespace PaypalIntegration.DataAccess.Repository
{
    public class ShippingDetailsRepository : IShippingDetailsRepository
    {
        public ShippingDetailsDTO GetShippingDetails()
        {
            using (nareshkartEntities dbContext = new nareshkartEntities())
            {
                var shippingDetails = dbContext.ShippingDetails.FirstOrDefault() ?? new ShippingDetail();
                return new ShippingDetailsDTO
                {
                    Country = shippingDetails.Country ?? 0,
                    Email = shippingDetails.Email,
                    FirstName = shippingDetails.FirstName,
                    LandMark = shippingDetails.LandMark,
                    LastName = shippingDetails.LastName,
                    PhoneNumber = shippingDetails.PhoneNumber,
                    Address = shippingDetails.ShippingAddress,
                    State = shippingDetails.State,
                    Street = shippingDetails.Street,
                    Zip = shippingDetails.Zipcode,
                    UserId = shippingDetails.UserId
                };
            }
        }

        public ShippingDetailsDTO GetShippingDetailsById(int id)
        {
            using (nareshkartEntities dbContext = new nareshkartEntities())
            {
                var shippingDetails = dbContext.ShippingDetails.Where(x => x.Id == id).FirstOrDefault() ?? new ShippingDetail();
                return new ShippingDetailsDTO
                {
                    Country = shippingDetails.Country ?? 0,
                    Email = shippingDetails.Email,
                    FirstName = shippingDetails.FirstName,
                    LandMark = shippingDetails.LandMark,
                    LastName = shippingDetails.LastName,
                    PhoneNumber = shippingDetails.PhoneNumber,
                    Address = shippingDetails.ShippingAddress,
                    State = shippingDetails.State,
                    Street = shippingDetails.Street,
                    Zip = shippingDetails.Zipcode,
                    UserId = shippingDetails.UserId
                };
            }
        }

        public int SaveShippingDetails(ShippingDetailsDTO shippingDetailsDTO)
        {
            try
            {
                using (nareshkartEntities dbContext = new nareshkartEntities())
                {
                    var entity = new ShippingDetail()
                    {
                        Country = shippingDetailsDTO.Country,
                        Email = shippingDetailsDTO.Email,
                        FirstName = shippingDetailsDTO.FirstName,
                        LandMark = shippingDetailsDTO.LandMark,
                        LastName = shippingDetailsDTO.LastName,
                        PhoneNumber = shippingDetailsDTO.PhoneNumber,
                        ShippingAddress = shippingDetailsDTO.Address,
                        State = shippingDetailsDTO.State,
                        Street = shippingDetailsDTO.Street,
                        Zipcode = shippingDetailsDTO.Zip,
                        UserId = shippingDetailsDTO.UserId
                    };
                    dbContext.ShippingDetails.Add(entity);
                    dbContext.SaveChanges();
                    return entity.Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}