﻿using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Concrete
{
    public class CollectionAnalyticManager : ICollectionAnalyticService
    {
        ICollectionAnalyticDal _collectionAnalytic;
        public CollectionAnalyticManager(ICollectionAnalyticDal collectionAnalytic)
        {
            _collectionAnalytic = collectionAnalytic;
        }

        public void Add(CollectionAnalytic t)
        {
            _collectionAnalytic.Insert(t);
        }

        public void Delete(CollectionAnalytic t)
        {
            _collectionAnalytic.Delete(t);
        }

        public CollectionAnalytic GetByID(int id)
        {
            return _collectionAnalytic.GetByID(id);
        }

        public List<CollectionAnalytic> GetList()
        {
            return _collectionAnalytic.GetListAll();
        }

        public void InitializeAnalytics(int collectionID)
        {
            List<string> keyList = new List<string>{ "Total Artworks", "Total Layers", "Total Layer Items"};
            foreach (string key in keyList)
            {
                CollectionAnalytic collectionAnalytic = new CollectionAnalytic();
                collectionAnalytic.CollectionID = collectionID;
                collectionAnalytic.Key = key;
                collectionAnalytic.Value = 0;
                collectionAnalytic.CreatedAt = DateTime.Now;
                collectionAnalytic.UpdatedAt = DateTime.Now;
                _collectionAnalytic.Insert(collectionAnalytic);
            }
            
        }

        public void Update(CollectionAnalytic t)
        {
            _collectionAnalytic.Update(t);
        }

        public void UpdateAnalytic(int collectionID, string key, int value)
        {
            List<CollectionAnalytic> collectionAnalytics = _collectionAnalytic.List(x => x.CollectionID == collectionID);
            foreach (var item in collectionAnalytics)
            {
                if(item.Key == key)
                {
                    item.Value += value;
                    item.UpdatedAt = DateTime.Now;
                    _collectionAnalytic.Update(item);
                }
            }
        }
    }
}