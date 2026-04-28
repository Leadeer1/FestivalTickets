using System;
using System.Collections.Generic;
using DDD.SharedKernel.DomainModelLayer.Implementations;

namespace DDD.FestivalTickets.Core.DomainModelLayer.ValueObjects
{
    public enum CustomerTypeValue
    {
        Regular        = 0,
        KrakowResident = 1,
        Student        = 2
    }

    /// <summary>
    /// Value Object reprezentujący typ uczestnika festiwalu.
    /// Typ klienta jest jedyną informacją potrzebną DiscountPolicyFactory
    /// do wybrania właściwej polityki rabatowej.
    ///
    /// Mapowanie EF (OwnsOne w CustomerConfiguration):
    ///   CustomerType_Value (int)
    /// </summary>
    public class CustomerType : ValueObject
    {
        public CustomerTypeValue Value { get; protected set; }

        // wymagany przez EF Core
        protected CustomerType() { }

        public CustomerType(CustomerTypeValue value) => Value = value;

        // metody pomocnicze używane w politykach i warunkach domenowych
        public bool IsStudent()        => Value == CustomerTypeValue.Student;
        public bool IsKrakowResident() => Value == CustomerTypeValue.KrakowResident;
        public bool IsRegular()        => Value == CustomerTypeValue.Regular;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value switch
        {
            CustomerTypeValue.Student        => "Student",
            CustomerTypeValue.KrakowResident => "Mieszkaniec Krakowa",
            _                                => "Zwykły uczestnik"
        };
    }
}
