using System;

namespace AdventureWorksApi.Models
{
    public class ProductDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string ProductNumber { get; set; }

        public bool MakeFlag { get; set; }

        public bool FinishedGoodsFlag { get; set; }

        public short SafetyStockLevel { get; set; }

        public short ReorderPoint { get; set; }

        public decimal StandartCost { get; set; }

        public decimal ListPrice { get; set; }

        public int DaysToManufacture { get; set; }

        public DateTime SellStartDate { get; set; }

        public Guid rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
