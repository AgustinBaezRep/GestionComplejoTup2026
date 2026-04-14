using GestionComplejo.Application.Abstractions.Infrastructure;
using GestionComplejo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestionComplejo.Infrastructure.Persistance.Repository
{
    public class CanchaRepository : BaseRepository<Cancha>, ICanchaRepository
    {
        public CanchaRepository(GestionComplejoDbContext context) : base(context)
        {
        }
    }
}
