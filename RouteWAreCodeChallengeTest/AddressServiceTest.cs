using System;
using Xunit;
using RouteWareCodeChallenge.Data;

namespace RouteWAreCodeChallengeTest
{
    public class AddressServiceTest
    {
        [Fact]
        public void GetAddressesFromCSVFileTest()
        {
            AddressService addressService = new AddressService();
            addressService.GetAddressesFromCSVFile();
            Assert.NotNull(addressService.GetAddressesFromCSVFile());
        }
    }
}
