﻿namespace Core.Entities
{
    public class CheerfulDiscountCard : DiscountCard
    {
        private int _discount = 10;
        public override int Discount => _discount;
    }
}
