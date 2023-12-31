﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TransactionMS.Models;

namespace TransactionMS.Data.Dtos
{
    public class MakeSaleDTO
    {
        public List<ProductOrderDTO> Products { get; set; }

        public Guid OrderId { get; set; }
        public double TotalCost { get; set; }
    }
}
