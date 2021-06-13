using Data;
using Data.Models;
using Services.Interfaces;

namespace Services.Repositories
{
    public class ExtratoRepository : Repository<Extrato>, IExtratoRepository
    {
        public ExtratoRepository(Context Context) : base(Context)
        {
        }
    }
}
