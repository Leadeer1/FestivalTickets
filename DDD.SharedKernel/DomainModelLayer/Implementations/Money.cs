using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DDD.SharedKernel.DomainModelLayer.Implementations
{
    // Na podstawie:
    // https://enterprisecraftsmanship.com/posts/value-object-better-implementation/
    // https://bottega.com.pl/ddd-cqrs-sample-project

    //public class Color
    //{
    //    public int Red { get; private set; }
    //    public int Green { get; private set; }
    //    public int Blue { get; private set; }

    //    public Color(int red, int green, int blue)
    //    {
    //        Red = red;
    //        Green = green;
    //        Blue = blue;
    //    }

    //    public Color MixIn(Color other)
    //    {
    //        return new Color(
    //            (Red + other.Red)/2, 
    //            (Green + other.Green)/2, 
    //            (Blue + other.Blue)/2);
    //    }
    //}

    public class Money : ValueObject
    {
        public static readonly string DefaultCurrency = NumberFormatInfo.CurrentInfo.CurrencySymbol;
        public static readonly Money Zero = new Money(0);

        public string Currency { get; protected set; }
        public decimal Amount { get; protected set; }

        protected Money()
        { }

        //public int Id { get; protected set; }

        public Money(decimal amount, string currency)
        {
            Currency = currency;
            Amount = amount;
        }

        public Money(decimal amount)
        {
            Currency = DefaultCurrency;
            Amount = amount;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency.ToUpper();
            yield return Math.Round(Amount, 2);
        }

        public static Money operator +(Money m, Money m2)
        {
            if (!AreCompatibleCurrencies(m, m2))
            {
                throw new ArgumentException("Currency mismatch");
            }
            return new Money(m.Amount + m2.Amount, m.Currency);
        }

        public static Money operator -(Money m, Money m2)
        {
            if (!AreCompatibleCurrencies(m, m2))
            {
                throw new ArgumentException("Currency mismatch");
            }
            return new Money(m.Amount - m2.Amount, m.Currency);
        }

        public Money MultiplyBy(double multiplier)
        {
            return MultiplyBy((decimal)multiplier);
        }
        public Money MultiplyBy(int multiplier)
        {
            return MultiplyBy((decimal)multiplier);
        }

        public Money MultiplyBy(decimal multiplier)
        {
            return new Money(Amount * multiplier, Currency);
        }

        /// <summary>
        /// Currency is compatible if the same or either money object has zero value.
        /// </summary>
        /// 

        public Money DiscountAmount(decimal percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentException("Procent rabatu musi być między 0 a 100.");
            return new Money(Math.Round(Amount * percent / 100, 2), Currency);
        }

        public Money ApplyPercentageDiscount(decimal percent)
        {
            if (percent < 0 || percent > 100)
                throw new ArgumentException("Procent rabatu musi być między 0 a 100.");
            return new Money(Math.Round(Amount * (1 - percent / 100), 2), Currency);
        }

        private static bool AreCompatibleCurrencies(Money m, Money m2)
        {
            return IsZero(m.Amount) || IsZero(m2.Amount) || m.Currency.Equals(m2.Currency);
        }

        private static bool IsZero(decimal testedValue)
        {
            return decimal.Zero.CompareTo(testedValue) == 0;
        }

        public static bool operator <(Money m, Money m2)
        {
            return m.Amount.CompareTo(m2.Amount) < 0;
        }

        public static bool operator >(Money m, Money m2)
        {
            return m.Amount.CompareTo(m2.Amount) > 0;
        }

        public static bool operator >=(Money m, Money m2)
        {
            return m.Amount.CompareTo(m2.Amount) >= 0;
        }

        public static bool operator <=(Money m, Money m2)
        {
            return m.Amount.CompareTo(m2.Amount) <= 0;
        }

        public override string ToString()
        {
            return string.Format("{0.2f} {1}", Amount, Currency);
        }
    }
}
