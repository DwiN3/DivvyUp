﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DivvyUp_Shared.Models
{
    public class Product
    {
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Receipt")]
        [Column("receipt_id")]
        public int ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("price")]
        public decimal Price { get; set; }

        [Required]
        [Column("additional_price")]
        public decimal AdditionalPrice { get; set; }

        [Required] 
        [Column("is_divisible")] 
        public bool Divisible { get; set; }

        [Required]
        [Column("max_quantity")]
        public int MaxQuantity { get; set; }

        [Column("available_quantity")]
        public int AvailableQuantity { get; set; }

        [Column("compensation_price")]
        public decimal CompensationPrice { get; set; }

        [Column("discount_percentage")]
        public int DiscountPercentage { get; set; }

        [Column("purchased_quantity")]
        public int PurchasedQuantity { get; set; }

        [Column("total_price")]
        public decimal TotalPrice { get; set; }

        [Column("is_settled")]
        public bool Settled { get; set; }
    }
}
