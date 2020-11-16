using CsvHelper;
using GeoCoordinatePortable;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RouteWareCodeChallenge.Data
{
    public class AddressService
    {
        List<Address> addresses = new List<Address>();
        public List<Address> GetAddressesFromExcelFile()
        {
            List<Address> addresses = new List<Address>();
            string fileName = "Data.csv";
            string filePath = Path.Combine(Environment.CurrentDirectory, @"Resource\", fileName);
            FileInfo fileInfo = new FileInfo(filePath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using(ExcelPackage excelPackage = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                int totalCollumn = workSheet.Dimension.End.Column;
                int totalRow = workSheet.Dimension.End.Row;
                for (int row = 1; row <= totalRow; row++)
                {
                    Address address = new Address();
                    address.RowNumber = row;
                    for (int col = 1; col <= totalCollumn; col++)
                    {
                        if (col == 1) address.Street = workSheet.Cells[row, col].Value.ToString();
                        if (col == 2) address.City = workSheet.Cells[row, col].Value.ToString();
                        if (col == 3) address.State = workSheet.Cells[row, col].Value.ToString();
                        if (col == 4) address.Zip = workSheet.Cells[row, col].Value.ToString();
                        if (col == 5) address.Latitude = Convert.ToDecimal(workSheet.Cells[row, col].Value);
                        if (col == 6) address.Longitude = Convert.ToDecimal(workSheet.Cells[row, col].Value);
                    }
                    addresses.Add(address);
                }

            }
            return addresses;
        }

        public List<Address> GetAddressesFromCSVFile()
        {
            //setup file path
            addresses = new List<Address>();
            string fileName = "Data.csv";
            string filePath = Path.Combine(Environment.CurrentDirectory, @"Resource\", fileName);

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<RowMapper>();
                addresses = csv.GetRecords<Address>().ToList();
            }

            return addresses;
        }

        public List<Address> GetNearestAddresses(int amountOfNearestAddresses, Decimal latitude, Decimal longitude)
        {
            var coord = new GeoCoordinate(Convert.ToDouble(latitude), Convert.ToDouble(longitude));
            var nearest = addresses.Select(x => x)
                       .OrderBy(x => x.geoCoordinate.GetDistanceTo(coord))
                       .Take(amountOfNearestAddresses);

            List<Address> nearestAddresses = nearest.ToList();
            
            return nearestAddresses;
        }

        public List<Address> GetSelectionOfAddresses(int skip, int take)
        {
           return addresses.GetRange(skip, take);
        }
    }
}
