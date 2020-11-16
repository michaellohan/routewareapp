using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using GeoCoordinatePortable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace RouteWareCodeChallenge.Data
{
    public class Address
    {
        [Ignore]
        public int RowNumber { get; set; }
        [Name("Address")]
        public string Street { get; set; }
        [Name("City")]
        public string City { get; set; }
        [Name("State")]
        public string State { get; set; }
        [Name("Zip")]
        public string Zip { get; set; }
        [Name("Longitude")]
        public Decimal Longitude { get; set; }
        [Name("Latitude")]
        public Decimal Latitude { get; set; }

        public GeoCoordinate geoCoordinate {
            get { return new GeoCoordinate(Convert.ToDouble(Latitude), Convert.ToDouble(Longitude)); }
        }
    }

    public sealed class RowMapper : ClassMap<Address>
    {
        public RowMapper()
        {
            Map(m => m.Street).Index(0).Name("Address");
            Map(m => m.Zip);
            Map(m => m.City);
            Map(m => m.State);
            Map(m => m.Latitude);
            Map(m => m.Longitude);
            Map(m => m.RowNumber).ConvertUsing(row => row.Context.RawRow);
        }
    }
}


