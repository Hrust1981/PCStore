﻿namespace Core.Entities
{
    public class TransistorDiscountCard : DiscountCard
    {
        private int _discount = 10;
        public override int Discount => _discount;
    }
}
