﻿using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Abstract
{
    public interface ITagService : IGenericService<Tag>
    {
        public bool isTagNameUnique(string name);
        public int AddWithReturn(Tag t, int collectionID);
    }
}
