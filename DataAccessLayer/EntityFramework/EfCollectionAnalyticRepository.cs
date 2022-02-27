﻿using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.EntityFramework
{
    public class EfCollectionAnalyticRepository : GenericRepository<CollectionAnalytic>, ICollectionAnalyticDal
    {
    }
}
