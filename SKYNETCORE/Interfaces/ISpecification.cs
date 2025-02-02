using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SKYNETCORE.Interfaces;

// Interfaz que define una especificación genérica
public interface ISpecification<T>
{
    // Propiedad que representa el criterio de filtrado (expresión booleana)
    Expression<Func<T, bool>>? Criteria { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescen { get;  }
    bool IsDistinct { get; }

    int Take { get; }
    int Skip {  get; }
    bool IsPagingEnabled { get; }
    IQueryable<T> ApplyCriteria(IQueryable<T> query);
}

public interface ISpecification<T, TResult>: ISpecification<T>
{
    Expression<Func<T, TResult>>? Select { get; }
    
}
