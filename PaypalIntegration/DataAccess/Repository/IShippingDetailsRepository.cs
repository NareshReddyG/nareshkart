using PaypalIntegration.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaypalIntegration.DataAccess.Repository
{
    public interface IShippingDetailsRepository
    {
        ShippingDetailsDTO GetShippingDetails();
        ShippingDetailsDTO GetShippingDetailsById(int id);
        int SaveShippingDetails(ShippingDetailsDTO shippingDetailsDTO);
    }
}
