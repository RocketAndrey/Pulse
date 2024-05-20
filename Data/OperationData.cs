using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Pulse.Data
{
    public class OperationData
    {
        protected Pulse.Data.AsuContext _asuContext;

        public OperationData (AsuContext asuContext)
        {
            _asuContext = asuContext;
        }
        //public async List<Pulse.Models.GroupLaborOperation> GetUnlinkedOperations()
        //{
        //    List<Pulse.Models.GroupLaborOperation> Operations;
        //    Operations = await _asuContext.LaborOperations.FromSqlRaw("[dbo].[sp_PulseGetEmptyLaborOpreations]").ToListAsync();

        //    return Operations;  

            
        //}

    }
}
