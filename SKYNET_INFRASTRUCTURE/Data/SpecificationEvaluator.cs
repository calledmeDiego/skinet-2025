using SKYNETCORE.Entities;
using SKYNETCORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYNET_INFRASTRUCTURE.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    // Método estático que aplica el criterio de una especificación a un IQueryable
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if (spec.Criteria != null)
        {
            // Aplica el filtro al conjunto de datos usando Where
            query = query.Where(spec.Criteria); // x => x.Brand == brand
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescen != null)
        {
            query = query.OrderByDescending(spec.OrderByDescen);
        }

        if (spec.IsDistinct)
        {
            query = query.Distinct();

        }

        return query;
    }

    public static IQueryable<TResult> GetQuery<TSpec,TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {
        if (spec.Criteria != null)
        {
            // Aplica el filtro al conjunto de datos usando Where
            query = query.Where(spec.Criteria); // x => x.Brand == brand
        }

        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if (spec.OrderByDescen != null)
        {
            query = query.OrderByDescending(spec.OrderByDescen);
        }

        var selectQuery = query as IQueryable<TResult>;

        if (spec.Select != null)
        {
            selectQuery = query.Select(spec.Select);
        }

        if (spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }

        return selectQuery ?? query.Cast<TResult>() ;
    }
}
