using System;
using System.Collections;
using System.Collections.Generic;

namespace EcommerceExtention
{
    public class CommonExtention<T> where T : class
    {
        public static List<T> ConvertIEnumToList(IEnumerable iEnum)
        {
            try
            {
                List<T> listEntity = new List<T>();
                foreach (T entity in iEnum)
                {
                    listEntity.Add(entity);
                }
                return listEntity;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
