using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

public static class CSharp6Extension {
/* 
    public static String nameof<T, TT>(this Expression<Func<T, TT>> accessor)
    {
        return nameof(accessor.Body);
    }

    public static String nameof<T>(this Expression<Func<T>> accessor)
    {
        return nameof(accessor.Body);
    }

    public static String nameof<T, TT>(this T obj, Expression<Func<T, TT>> propertyAccessor)
    {
        return nameof(propertyAccessor.Body);
    }

    private static String nameof(Expression expression)
    {
        if (expression.NodeType == ExpressionType.MemberAccess)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression == null)
                return null;
            return memberExpression.Member.Name;
        }
        return null;
    }
*/
    public static T GetArgumentOrThrowException<T>(T arg, string argName ){
        if (arg != null){
            return arg;
        }

        throw new ArgumentNullException( argName );
    }

    public static TT TryGetValue<T, TT>(Dictionary<T, TT> d, T key){
       if (d.ContainsKey(key)){
           return d[key];
       }
       return default(TT);
    }

}

 public class Tuple<T1, T2>
 {
     public T1 First { get; private set; }
     public T2 Second { get; private set; }
     internal Tuple(T1 first, T2 second)
     {
         First = first;
         Second = second;
     }
 }
 
 public static class Tuple
 {
     public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
     {
         var tuple = new Tuple<T1, T2>(first, second);
         return tuple;
     }
 }

// A hack to mimic Expression.Assign that is not available in .Net 3.5
// https://stackoverflow.com/questions/4564639/equivalent-of-expression-assign-in-net-3-5#4606660
public class AssignableMemberAccess{
    MemberExpression memberAccess {get; set;}
}