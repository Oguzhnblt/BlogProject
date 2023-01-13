using BlogProject.Core.Entity;
using BlogProject.Core.Entity.Enum;
using BlogProject.Core.Service;
using BlogProject.Entities.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().Where(x => x.Status == Status.Active);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public List<T> GetAll() => context.Set<T>().ToList();
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

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
                using (TransactionScope ts = new TransactionScope())
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

                        if (operationResult)
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
            T item = GetById(id);
            item.Status = Status.Active;
            return Update(item);
        }

        public void DetachEntity(T item)
        {
            context.Entry<T>(item).State = EntityState.Detached; // Bir entry'i takip etmeyi bırakmak için kullanılır.
        }


    }
}

// The BaseService class is a generic service class that provides various methods for interacting with a database using Entity Framework. The class is designed to work with any entity type that is specified when the service is used.

// The Add method adds a single item to the database. If the operation is successful, it returns true. If an exception is thrown, it returns false.

// The Add method that takes a list of items adds the items to the database within a transaction scope. If the operation is successful, it returns true. If an exception is thrown, it returns false.

// The Any method checks if there are any items in the database that match the specified expression. It returns true if there are any matching items, and false if there are none.

// The GetActive method retrieves a list of all active items from the database.

// The GetAll method retrieves a list of all items from the database.

// The GetByDefault method retrieves the first item from the database that matches the specified expression.

// The GetById method retrieves an item from the database by its ID.

// The GetDefault method retrieves a list of items from the database that match the specified expression.

// The Remove method sets the status of a single item to "deleted" and updates it in the database.

// The Remove method that takes an ID retrieves the item with the specified ID, sets its status to "deleted", and updates it in the database within a transaction scope.

// The RemoveAll method sets the status of all items that match the specified expression to "deleted" and updates them in the database within a transaction scope. If all the updates are successful, the transaction scope is completed and the changes are persisted to the database.

// The Update method updates an item in the database. If the operation is successful, it returns true. If an exception is thrown, it returns false.

// The Save method saves any changes made to the database and returns the number of affected rows.