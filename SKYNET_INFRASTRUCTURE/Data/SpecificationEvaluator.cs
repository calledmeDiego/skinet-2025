﻿using Microsoft.EntityFrameworkCore;
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

        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }


        //var currentQuery = query; 
        // Valor inicial

        //foreach (var include in spec.Includes)
        //{
        //    currentQuery = currentQuery.Include(include); // Aplica cada include
        //}

        //query = currentQuery; ==v==
        
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));


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

        if (spec.IsPagingEnabled)
        {
            selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
        }


        return selectQuery ?? query.Cast<TResult>() ;
    }
}
