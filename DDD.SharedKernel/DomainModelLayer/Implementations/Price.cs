using System;

namespace DDD.SharedKernel.DomainModelLayer.Implementations
{
    public class Price: Money 
    {
        protected Price()
        { }

        public Price(decimal amount, string currency)
            :base(amount, currency)
        {
            if (amount < 0) throw new Exception("Price can not be less then zero");
        }
    }
}
