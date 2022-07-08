using Antomi.DataAccsessLayer;
using Antomi.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Antomi.Services
{
    public class DiscountDelete : IDiscountDelete
    {
        private readonly AntomiDbContext context;

        public DiscountDelete(AntomiDbContext context)
        {
            this.context = context;
        }
        public async Task Delete()
        {
         
                List<Discount> discounts = context.Discounts.Where(x => x.EndDate <= DateTime.Now).ToList();
                context.Discounts.RemoveRange(discounts);
                await context.SaveChangesAsync();
            
           
        }
    }
}
