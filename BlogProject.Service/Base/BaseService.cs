using BlogProject.Core.Entity;
using BlogProject.Core.Entity.Enum;
using BlogProject.Core.Service;
using BlogProject.Entities.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BlogProject.Service.Base
{
    public class BaseService<T> : ICoreService<T> where T : CoreEntity
    {
        private readonly BlogProjectContext context;

        public BaseService(BlogProjectContext context)
        {
            this.context = context;
        }
       
        public bool Add(T item)
        {
            try
            {
                context.Set<T>().Add(item);
                return Save() > 0; // 1 satır etkileniyorsa true döndürsün
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                

                using (TransactionScope ts = new TransactionScope())
                {
                    context.Set<T>().AddRange(items);

                    ts.Complete();  // Tüm işlemler başarılı olduğunda, yani tüm ekleme işlemleri başarılı olursa Complete() olacak.
                    return Save() > 0;  // Bir veya daha fazla satır etkileniyorsa..
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Any(Expression<Func<T, bool>> expression) => context.Set<T>().Any(expression);

        public List<T> GetActive() => context.Set<T>().Where(x => x.Status == Status.Active).ToList();

        public List<T> GetAll() => context.Set<T>().ToList();

        public T GetByDefault(Expression<Func<T, bool>> expression) => context.Set<T>().FirstOrDefault(expression);

        public T GetById(Guid id) => context.Set<T>().Find(id);

        public List<T> GetDefault(Expression<Func<T, bool>> expression) => context.Set<T>().Where(expression).ToList();

        public bool Remove(T item)
        {
            item.Status = Status.Deleted;
            return Update(item);
        }

        public bool Remove(Guid id)
        {
            try
            {
                using(TransactionScope ts = new TransactionScope())
                {
                    T item = GetById(id);
                    item.Status = Status.Deleted;
                    ts.Complete();

                    return Update(item);
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> expression)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                   var collection = GetDefault(expression);  // Verilen ifadeye göre ilgili nesneleri collection'a atıyoruz.
                    int counter = 0;
                    foreach (var item in collection)
                    {
                        item.Status = Status.Deleted;
                        bool operationResult = Update(item); // DB'den silmiyoruz. Durumunu silindi olarak ayarlıyoruz ve bunu da update metodu ile gerçekleştiriyoruz.

                        if(operationResult)
                        {
                            counter++;  // Sıradaki item'in silinme işlemi (yani silindi işaretleme) başarılı ise sayacı 1 arttırıyoruz.
                        }

                        if (collection.Count == counter) ts.Complete(); // Koleksiyondaki eleman sayısı ile silinme işlemi gerçekleşen eleman sayısı eşit ise bu işlemler başarılıdır.
                        else return false; // Aksi halde hiçbirinde değişiklik yapmadan false döndürür.
                    }

                    return true;

                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public bool Update(T item)
        {
            try
            {
                context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool Activate(Guid id)
        {
            T item= GetById(id);
            item.Status = Status.Active;
            return Update(item);
        }

        public void DetachEntity(T item)
        {
            context.Entry<T>(item).State = EntityState.Detached; // Bir entry'i takip etmeyi bırakmak için kullanılır.
        }

    }
}
