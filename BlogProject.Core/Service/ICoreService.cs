using BlogProject.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Core.Service
{
    public interface ICoreService<T> where T : CoreEntity
    {
        bool Add(T item);
        bool Add(List<T> items);
        bool Remove(T item);     
        bool Remove(Guid id); 
        bool RemoveAll(Expression<Func<T, bool>> expression); // Belli bir LINQ ifadesine göre filtreleyip silmek için yazılan servis metodu. Metodun içine LINQ ifadesi verilecektir.
        bool Update(T item);
        T GetById(Guid id);
        T GetByDefault(Expression<Func<T, bool>> expression); // FirstOrDefault'a benzer bir metot oluşturur.
        List<T> GetActive();
        List<T> GetDefault(Expression<Func<T, bool>> expression);
        List<T> GetAll();
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] ex);
        bool Activate(Guid id);
        bool Any(Expression<Func<T, bool>> expression);
        int Save(); // DB'de manipülasyon işleminden sonra 1 veya daha fazla satır etkilendiğinde bize kaç satırın etkilendiğini döndürecek metot.
        IQueryable<T> GetActive(params Expression<Func<T, object>>[] ex);
        void DetachEntity(T item);
    }
}
