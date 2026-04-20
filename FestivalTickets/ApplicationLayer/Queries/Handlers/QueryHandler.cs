using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.FestivalTickets.Core.ApplicationLayer.Queries.Handlers
{
    //zwraca EF dla prostych, nie wymagających logiki biznesowej zapytań, które nie mają dedykowanego handlera
    //zwraca Dapper dla Joinów
    internal class QueryHandler
    {
    }
}
