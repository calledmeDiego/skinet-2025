using SKYNETCORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SKYNETCORE.Specifications;

// Clase base para crear especificaciones genéricas
public class BaseSpecification<T>(Expression<Func<T, bool>>? _criteria) : ISpecification<T>
{
    public BaseSpecification(): this(null)
    {
        
    }
    // Constructor recibe un criterio y lo asigna a esta propiedad Criteria
    public Expression<Func<T, bool>>? Criteria => _criteria;

    public Expression<Func<T, object>>? OrderBy {  get; private set; }

    public Expression<Func<T, object>>? OrderByDescen {  get; private set; }

    public bool IsDistinct { get; private set; }

    protected void AddOrderBy(Expression<Func<T, object>>? orderBy)
    {
        OrderBy = orderBy;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>>? orderByDesc)
    {
        OrderByDescen = orderByDesc;
    }

    protected void ApplyDistinct()
    {
        IsDistinct = true;
    }
}

public class BaseSpecification<T, TResult>(Expression<Func<T, bool>> criteria) : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    public BaseSpecification(): this(null!)
    {
        
    }
    public Expression<Func<T, TResult>>? Select { get; private set; }

    
    protected void AddSelect(Expression<Func<T, TResult>>? selectExpression)  
    { 
        Select = selectExpression; 
    }
}
