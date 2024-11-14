﻿namespace DivvyUp_Shared.Dto
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly Date { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Settled { get; set; }

        public ReceiptDto()
        {
            Id = 0;
            Name = string.Empty;
            Date = DateOnly.FromDateTime(DateTime.Now);
            TotalPrice = 0;
            Settled = false;
        }

        public ReceiptDto(int id, string name, DateOnly date, decimal totalPrice, bool settled)
        {
            Id = id;
            Name = name;
            Date = date;
            TotalPrice = totalPrice;
            Settled = settled;
        }
    }
}
