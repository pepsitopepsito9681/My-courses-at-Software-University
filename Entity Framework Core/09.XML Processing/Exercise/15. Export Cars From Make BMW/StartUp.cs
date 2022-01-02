using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using CarDealer.Data;
using CarDealer.DTO.ExportDto;
using CarDealer.DTO.ImportDto;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext dbContext = new CarDealerContext();

            //ResetDb(dbContext);
            //string inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //string inputXml = File.ReadAllText("../../../Datasets/parts.xml");
            //string inputXml = File.ReadAllText("../../../Datasets/cars.xml");
            //string inputXml = File.ReadAllText("../../../Datasets/customers.xml");
            //string inputXml = File.ReadAllText("../../../Datasets/sales.xml");
            //ImportSuppliers(dbContext, inputXml);
            //ImportParts(dbContext, inputXml);
            //ImportCars(dbContext, inputXml);
            //ImportCustomers(dbContext, inputXml);
            //string result = ImportSales(dbContext, inputXml);
            //string result = GetCarsWithDistance(dbContext);
            string result = GetCarsFromMakeBmw(dbContext);

            Console.WriteLine(result);
        }
        //suppliers.xml
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportSupplierDto[] dtos = (ImportSupplierDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Supplier> suppliers = new HashSet<Supplier>();

            foreach (ImportSupplierDto supplierDto in dtos)
            {
                Supplier s = new Supplier
                {
                    Name = supplierDto.Name,
                    IsImporter = bool.Parse(supplierDto.IsImporter)
                };

                suppliers.Add(s);
            }
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            //parts.xml

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Parts");
            XmlSerializer xmlSerializer = new XmlSerializer(
               typeof(ImportPartDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportPartDto[] partDtos = (ImportPartDto[])
                xmlSerializer.Deserialize(stringReader);

            ICollection<Part> parts = new HashSet<Part>();

            foreach (ImportPartDto partDto in partDtos)
            {
                Supplier supplier = context
                    .Suppliers
                    .Find(partDto.SupplierId);

                if (supplier == null)
                {
                    continue;
                }
                Part p = new Part
                {
                    Name = partDto.Name,
                    Price = decimal.Parse(partDto.Price),
                    Quantity = partDto.Quantity,
                    Supplier = supplier
                };
                parts.Add(p);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            //cars.xml

            XmlSerializer xmlSerializer = GenerateXmlSerializer("Cars", typeof(ImportCarDto[]));
            using StringReader stringReader = new StringReader(inputXml);
            ImportCarDto[] carDtos = (ImportCarDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Car> cars = new HashSet<Car>();
            //ICollection<PartCar> partCars = new HashSet<PartCar>();
            foreach (ImportCarDto carDto in carDtos)
            {
                Car c = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TraveledDistance,
                };
                ICollection<PartCar> currentCarParts = new HashSet<PartCar>();

                foreach (int partId in carDto.Parts.Select(p => p.Id).Distinct())
                {
                    Part part = context
                        .Parts
                        .Find(partId);

                    if (part == null)
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        Car = c,
                        Part = part
                    };
                    currentCarParts.Add(partCar);
                }

                c.PartCars = currentCarParts;
                cars.Add(c);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            using var reader = new StringReader(inputXml);

            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            var customersDto = (ImportCustomerDto[])serializer.Deserialize(reader);

            var customers = customersDto
                .Select(cd => new Customer
                {
                    Name = cd.Name,
                    BirthDate = cd.BirthDate,
                    IsYoungDriver = cd.IsYoungDriver
                })
                .ToList();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            using var reader = new StringReader(inputXml);

            var serializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));
            var saleDtos = (ImportSaleDto[])serializer.Deserialize(reader);

            var sales = saleDtos
                .Where(sd => context.Cars.Any(c => c.Id == sd.CarId))
                .Select(sd => new Sale
                {
                    CarId = sd.CarId,
                    CustomerId = sd.CustomerId,
                    Discount = sd.Discount
                })
                .ToList();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlSerializer xmlSerializer = GenerateXmlSerializer("cars", typeof(ExportCarsWithDistanceDto[]));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            ExportCarsWithDistanceDto[] carsdDtos = context.Cars.Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarsWithDistanceDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance.ToString()
                })
                .ToArray();
            
            xmlSerializer.Serialize(stringWriter, carsdDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new
                {
                    x.Id,
                    x.Make,
                    x.Model,
                    x.TravelledDistance

                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();

            XDocument doc = new XDocument();
            doc.Declaration = new XDeclaration("1.0", "UTF-8", null);
            doc.Add(new XElement("cars"));
            foreach (var car in cars)
            {
                XElement el =
                    new XElement("car", // parent
                        new XAttribute("id", car.Id), // children
                        new XAttribute("model", car.Model), // children
                        new XAttribute("travelled-distance", car.TravelledDistance)); // children

                doc.Root.Add(el);
            }

            var wr = new StringWriter();
            doc.Save(wr);
            return wr.ToString();
        }

        private static XmlSerializer GenerateXmlSerializer(string rootName, Type dtoType)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(
               dtoType, xmlRoot);

            return xmlSerializer;
        }

        private static void ResetDb(CarDealerContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            Console.WriteLine("Db reset was successful");
        }
    }
}